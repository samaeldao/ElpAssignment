﻿using AutoMapper;
using Elp.Api.Models;
using Elp.Application.Certificates.Commands.CreateCertificate;
using Elp.Application.Certificates.Commands.InvalidateCertificateCommand;
using Elp.Application.Certificates.Queries.CheckAuthorization;
using Elp.Application.Certificates.Queries.GetCertificateById;
using Elp.Application.Certificates.Queries.GetCertificateHistory;
using Elp.Application.Certificates.Queries.SearchCertificates;
using Elp.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent; // ADDED: Required for ILogger

namespace Elp.Api.Controllers;

[ApiController]
[Route("api/v2/posudky/ridicskeOpravneni")]
public class CertificatesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<CertificatesController> _logger; // ADDED: The Logger

    // ADDED: Inject the logger into the constructor
    public CertificatesController(IMediator mediator, IMapper mapper, ILogger<CertificatesController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PosudekRoCreateResultDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] PosudekRoCreateDto requestDto,
        [FromHeader(Name = "X-Correlation-Id")] string? correlationId)
    {
        // LOG: Start of a major business action
        _logger.LogInformation("Received request to create certificate for driver {DriverRid} by doctor {DoctorKrzpId}",
            "***REDACTED***", // Removed PII (RID) from logs
            requestDto.KrzpId);

        var command = _mapper.Map<CreateCertificateCommand>(requestDto);
        var newCertificateId = await _mediator.Send(command);

        var resultDto = new PosudekRoCreateResultDto
        {
            Id = newCertificateId
        };

        // LOG: Successful completion
        _logger.LogInformation("Successfully created certificate {CertificateId} for driver {DriverRid}",
            newCertificateId,
            "***REDACTED***");

        return CreatedAtAction(nameof(GetById), new { id = newCertificateId }, resultDto);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PosudekRoDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        _logger.LogInformation("Fetching details for certificate {CertificateId}", id);

        var query = new GetCertificateByIdQuery(id);
        var certificateDto = await _mediator.Send(query);

        if (certificateDto == null)
        {
            _logger.LogWarning("Certificate lookup failed. ID {CertificateId} was not found", id);
            return NotFound();
        }

        var responseDto = _mapper.Map<PosudekRoDetailDto>(certificateDto);

        // ADDED: Give the client the ETag in the Response Headers!
        // We wrap it in quotes as per the RFC standard for ETags (e.g., "AAAAAAAAAAA=")
        if (!string.IsNullOrEmpty(responseDto.RowVersion))
        {
            Response.Headers.ETag = $"\"{responseDto.RowVersion}\"";
        }

        return Ok(responseDto);
    }

    [HttpPatch("{id:guid}/zneplatnit")]
    [ProducesResponseType(typeof(PosudekRoDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)] 
    public async Task<IActionResult> Invalidate(
            [FromRoute] Guid id,
            [FromHeader(Name = "If-Match")] string? ifMatch)
    {
        _logger.LogInformation("Received request to invalidate certificate {CertificateId}", id);

        // The client sends the ETag with quotes, and potentially as a Weak ETag (W/). 
        // We strip them out to get the raw base64 string.
        var rowVersion = ifMatch?.Replace("\"", "").Replace("W/", "");

        var command = new InvalidateCertificateCommand(id, rowVersion);

        try
        {
            var success = await _mediator.Send(command);

            if (!success)
            {
                _logger.LogWarning("Invalidation failed. Certificate {CertificateId} was not found", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully invalidated certificate {CertificateId}", id);
            
            var query = new GetCertificateByIdQuery(id);
            var certificateDto = await _mediator.Send(query);
            var responseDto = _mapper.Map<PosudekRoDetailDto>(certificateDto);
            
            if (!string.IsNullOrEmpty(responseDto.RowVersion))
            {
                Response.Headers.ETag = $"\"{responseDto.RowVersion}\"";
            }

            return Ok(responseDto);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
        {
            // ADDED: If EF Core detects a version mismatch, it throws this exception!
            _logger.LogWarning(ex, "Concurrency conflict! Certificate {CertificateId} was modified by someone else.", id);

            // Return the 409 Conflict status required by the OpenAPI spec
            return StatusCode(StatusCodes.Status409Conflict, "The certificate was modified by another user. Please refresh and try again.");
        }
    }

    [HttpPost("vyhledat")]
    [ProducesResponseType(typeof(PagedResult<CertificateSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] VyhledatPosudkyDto requestDto)
    {
        _logger.LogInformation("Executing certificate search. Driver: {DriverRid}, Doctor: {DoctorKrzpId}, Status: {StatusCode}, Page: {PageNumber}",
            requestDto.Rid,
            requestDto.KrzpId,
            requestDto.StavPosudku,
            requestDto.Stranka);

        var query = new SearchCertificatesQuery(
            requestDto.Rid,
            requestDto.KrzpId,
            requestDto.StavPosudku,
            requestDto.DatumOd,
            requestDto.DatumDo,
            requestDto.Stranka,
            requestDto.VelikostStranky
        );

        var result = await _mediator.Send(query);

        _logger.LogInformation("Search completed. Returning {ResultCount} total items for current query", result.TotalCount);

        return Ok(result);
    }

    [HttpGet("{id:guid}/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadPdf([FromRoute] Guid id)
    {
        _logger.LogInformation("Generating PDF document for certificate {CertificateId}", id);

        // 1. Fetch the data using your existing MediatR query
        var query = new GetCertificateByIdQuery(id);
        CertificateDto? certificateDto = await _mediator.Send(query);

        if (certificateDto == null)
        {
            _logger.LogWarning("PDF generation failed. Certificate {CertificateId} not found", id);
            return NotFound();
        }

        // Map to your Czech DTO
        var responseDto = _mapper.Map<PosudekRoDetailDto>(certificateDto);

        // 2. Generate the PDF byte array
        var document = new Documents.MedicalCertificateDocument(responseDto);
        var pdfBytes = document.GeneratePdf();

        // 3. Return as a downloadable file!
        var fileName = $"Posudek_{responseDto.Rid}_{DateTime.Now:yyyyMMdd}.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }

    // POST /api/v2/posudky/ridicskeOpravneni/zalozeni/opravneni
    [HttpPost("zalozeni/opravneni")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckAuthorization([FromBody] CheckAuthorizationDto requestDto)
    {
        _logger.LogInformation("Checking authorization for doctor {DoctorId} to create certificate for driver {DriverId}",
            requestDto.KrzpId, requestDto.Rid);

        var query = new CheckAuthorizationQuery(requestDto.KrzpId, requestDto.Rid);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // GET /api/v2/posudky/ridicskeOpravneni/{id}/historie
    [HttpGet("{id:guid}/historie")]
    [ProducesResponseType(typeof(List<CertificateHistoryItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHistory([FromRoute] Guid id)
    {
        _logger.LogInformation("Fetching history for certificate {CertificateId}", id);

        var query = new GetCertificateHistoryQuery(id);
        var result = await _mediator.Send(query);

        if (result == null || !result.Any())
            return NotFound();

        return Ok(result);
    }
}