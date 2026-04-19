using MediatR;
using Elp.Application.Common.Models;

namespace Elp.Application.Certificates.Queries.SearchCertificates;

public record SearchCertificatesQuery(
    string? PersonalId,
    Guid? MedicalProfessionalId,
    string? StatusCode,
    DateTime? DateFrom,
    DateTime? DateTo,
    int Page,
    int PageSize
) : IRequest<PagedResult<CertificateSummaryDto>>;