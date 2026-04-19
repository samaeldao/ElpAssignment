using MediatR;
using Elp.Domain.Entities;
using Elp.Application.Common.Interfaces;

namespace Elp.Application.Certificates.Commands.CreateCertificate;

public class CreateCertificateCommandHandler : IRequestHandler<CreateCertificateCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCertificateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
    {
        var certificate = DriverFitnessCertificate.Create(
            request.PersonalId,
            request.MedicalProfessionalId,
            request.IssueDate,
            request.StatusCode,
            null);

        _context.DriverFitnessCertificates.Add(certificate);
        await _context.SaveChangesAsync(cancellationToken);

        return certificate.Id;
    }
}