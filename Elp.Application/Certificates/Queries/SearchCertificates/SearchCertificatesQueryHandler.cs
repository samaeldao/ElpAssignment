using MediatR;
using Microsoft.EntityFrameworkCore;
using Elp.Application.Common.Interfaces;
using Elp.Application.Common.Models;

namespace Elp.Application.Certificates.Queries.SearchCertificates;

public class SearchCertificatesQueryHandler : IRequestHandler<SearchCertificatesQuery, PagedResult<CertificateSummaryDto>>
{
    private readonly IApplicationDbContext _context;

    public SearchCertificatesQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<PagedResult<CertificateSummaryDto>> Handle(SearchCertificatesQuery request, CancellationToken cancellationToken)
    {
        // 1. Start the query. AsNoTracking makes it lightning fast for read-only operations.
        var query = _context.DriverFitnessCertificates.AsNoTracking().AsQueryable();

        // 2. Dynamically stack the filters based on what the frontend sent
        if (!string.IsNullOrWhiteSpace(request.PersonalId))
            query = query.Where(c => c.PersonalId == request.PersonalId);

        if (request.MedicalProfessionalId.HasValue)
            query = query.Where(c => c.MedicalProfessionalId == request.MedicalProfessionalId.Value);

        if (!string.IsNullOrWhiteSpace(request.StatusCode))
            query = query.Where(c => c.StatusCode == request.StatusCode);

        if (request.DateFrom.HasValue)
            query = query.Where(c => c.IssueDate >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            query = query.Where(c => c.IssueDate <= request.DateTo.Value);

        // 3. Count the total matching records BEFORE applying pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // 4. Apply sorting, pagination, and projection (Mapping)
        var items = await query
            .OrderByDescending(c => c.IssueDate) // Usually, users want to see the newest records first
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CertificateSummaryDto
            {
                Id = c.Id,
                PersonalId = c.PersonalId,
                MedicalProfessionalId = c.MedicalProfessionalId,
                IssueDate = c.IssueDate,
                StatusCode = c.StatusCode
            })
            .ToListAsync(cancellationToken);

        // 5. Wrap it all in our Pagination envelope
        return new PagedResult<CertificateSummaryDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}