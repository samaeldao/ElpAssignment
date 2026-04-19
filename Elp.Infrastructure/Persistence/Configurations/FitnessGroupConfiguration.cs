using Elp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elp.Infrastructure.Persistence.Configurations;

public class FitnessGroupConfiguration : IEntityTypeConfiguration<FitnessGroup>
{
    public void Configure(EntityTypeBuilder<FitnessGroup> builder)
    {
        builder.ToTable("FitnessGroups");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DriverGroupCode)
            .IsRequired();

        builder.Property(x => x.ResultCode)
            .IsRequired();

        // Configure the One-to-Many relationship
        builder.HasOne(x => x.Certificate)
            .WithMany(x => x.FitnessGroups)
            .HasForeignKey(x => x.DriverFitnessCertificateId)
            .OnDelete(DeleteBehavior.Cascade); // If a certificate is deleted, delete its groups
    }
}