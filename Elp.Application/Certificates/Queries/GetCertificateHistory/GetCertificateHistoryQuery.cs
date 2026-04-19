using MediatR;

namespace Elp.Application.Certificates.Queries.GetCertificateHistory
{
    public record GetCertificateHistoryQuery(Guid CertificateId) : IRequest<List<CertificateHistoryItemDto>>;
}
