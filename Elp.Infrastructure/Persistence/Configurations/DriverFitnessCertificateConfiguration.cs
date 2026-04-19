using Elp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elp.Infrastructure.Persistence.Configurations;

public class DriverFitnessCertificateConfiguration : IEntityTypeConfiguration<DriverFitnessCertificate>
{
    public void Configure(EntityTypeBuilder<DriverFitnessCertificate> builder)
    {
        builder.ToTable("DriverFitnessCertificates");

        builder.HasKey(x => x.Id);

        // Map English properties to matching constraints from the API spec
        builder.Property(x => x.PersonalId)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.MedicalProfessionalId)
            .IsRequired();

        builder.Property(x => x.StatusCode)
            .IsRequired();

        // THIS is the magic line that enables the If-Match concurrency check
        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}