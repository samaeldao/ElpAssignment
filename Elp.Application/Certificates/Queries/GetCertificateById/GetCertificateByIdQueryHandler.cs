using MediatR;
using Elp.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elp.Application.Certificates.Queries.GetCertificateById;

public class GetCertificateByIdQueryHandler : IRequestHandler<GetCertificateByIdQuery, CertificateDto?>
{
    private readonly IApplicationDbContext _context;

    public GetCertificateByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CertificateDto?> Handle(GetCertificateByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.DriverFitnessCertificates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null) return null;

        return new CertificateDto
        {
            Id = entity.Id,
            PersonalId = entity.PersonalId,
            MedicalProfessionalId = entity.MedicalProfessionalId,
            IssueDate = entity.IssueDate,
            StatusCode = entity.StatusCode,
            RowVersion = entity.RowVersion
        };
    }
}