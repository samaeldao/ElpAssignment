using AutoMapper;
using Elp.Api.Models;
using Elp.Application.Certificates.Queries.GetCodebookItems;
using Elp.Application.Certificates.Queries.GetCodebooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elp.Api.Controllers;

[ApiController]
[Route("api/v2/ciselniky")]
public class CodebooksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<CodebooksController> _logger; // ADDED: The Logger

    // ADDED: Inject the logger into the constructor
    public CodebooksController(IMediator mediator, IMapper mapper, ILogger<CodebooksController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: /api/v2/ciselniky/{kod}/polozky
    [HttpGet("{kod}/polozky")]
    [ProducesResponseType(typeof(List<CiselnikPolozkaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCodebookItems(
        [FromRoute] string kod,
        [FromHeader(Name = "Accept-Language")] string? acceptLanguage)
    {
        var language = acceptLanguage ?? "cs";

        _logger.LogInformation("Fetching items for codebook {CodebookCode} with language {Language}",
            kod,
            language);

        var query = new GetCodebookItemsQuery(kod, language);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            // LOG: Warning if a client asks for a dictionary that doesn't exist
            _logger.LogWarning("Codebook items lookup failed. Codebook {CodebookCode} was not found", kod);
            return NotFound();
        }

        var response = _mapper.Map<List<CiselnikPolozkaDto>>(result);

        _logger.LogInformation("Successfully retrieved {ItemCount} items for codebook {CodebookCode}",
            response.Count,
            kod);

        return Ok(response);
    }

    // GET: /api/v2/ciselniky
    [HttpGet]
    [ProducesResponseType(typeof(List<CodebookDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCodebooks(
        [FromHeader(Name = "Accept-Language")] string? acceptLanguage)
    {
        var language = acceptLanguage ?? "cs";

        // LOG: Track requests for the master dictionary list
        _logger.LogInformation("Fetching all available codebooks with language {Language}", language);

        var query = new GetCodebooksQuery(language);
        var result = await _mediator.Send(query);

        // LOG: Track successful master list retrieval
        _logger.LogInformation("Successfully retrieved {CodebookCount} master codebooks", result.Count);

        return Ok(result);
    }
}