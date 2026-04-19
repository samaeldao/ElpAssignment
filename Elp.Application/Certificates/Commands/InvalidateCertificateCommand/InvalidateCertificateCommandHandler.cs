using MediatR;
using Elp.Application.Common.Interfaces;
using Elp.Domain.Entities;

namespace Elp.Application.Certificates.Commands.InvalidateCertificateCommand;

public class InvalidateCertificateCommandHandler : IRequestHandler<InvalidateCertificateCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public InvalidateCertificateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(InvalidateCertificateCommand request, CancellationToken cancellationToken)
    {
        DriverFitnessCertificate? certificate = await _context.DriverFitnessCertificates.FindAsync(new object[] { request.Id }, cancellationToken);

        if (certificate == null)
            return false;

        if (!string.IsNullOrEmpty(request.RowVersion))
        {
            try
            {
                var clientVersion = Convert.FromBase64String(request.RowVersion);
                _context.Entry(certificate).Property(c => c.RowVersion).OriginalValue = clientVersion;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        certificate.Invalidate();

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}