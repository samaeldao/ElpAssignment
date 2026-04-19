using MediatR;

namespace Elp.Application.Certificates.Commands.InvalidateCertificateCommand
{
    public record InvalidateCertificateCommand(Guid Id, string? RowVersion) : IRequest<bool>;
}
