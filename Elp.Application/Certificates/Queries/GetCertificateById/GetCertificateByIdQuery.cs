using MediatR;

namespace Elp.Application.Certificates.Queries.GetCertificateById;

public record GetCertificateByIdQuery(Guid Id) : IRequest<CertificateDto?>;