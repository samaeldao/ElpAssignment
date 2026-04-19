using MediatR;

namespace Elp.Application.Certificates.Queries.GetCertificateHistory
{
    public class GetCertificateHistoryQueryHandler : IRequestHandler<GetCertificateHistoryQuery, List<CertificateHistoryItemDto>>
    {
        public Task<List<CertificateHistoryItemDto>> Handle(GetCertificateHistoryQuery request, CancellationToken cancellationToken)
        {
            var history = new List<CertificateHistoryItemDto>
        {
            new() { ChangedAt = DateTime.UtcNow.AddDays(-2), Action = "Založení posudku" /* Created */, ChangedBy = "System" },
            new() { ChangedAt = DateTime.UtcNow, Action = "Změna stavu na ZNEPLATNENO", ChangedBy = "Doctor" }
        };

            return Task.FromResult(history);
        }
    }
}
