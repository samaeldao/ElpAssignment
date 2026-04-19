using Microsoft.EntityFrameworkCore;
using Elp.Domain.Entities;
using Elp.Application.Common.Interfaces;

namespace Elp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<DriverFitnessCertificate> DriverFitnessCertificates => Set<DriverFitnessCertificate>();
    public DbSet<Codebook> Codebooks => Set<Codebook>();
    public DbSet<CodebookItem> CodebookItems => Set<CodebookItem>();
    public DbSet<FitnessGroup> FitnessGroups => Set<FitnessGroup>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<DriverFitnessCertificate>(entity =>
        {
            // Tell EF Core to use this for concurrency tracking
            entity.Property(e => e.RowVersion)
                  .IsRowVersion();
        });

        // Define Primary Keys explicitly
        builder.Entity<Codebook>().HasKey(c => c.Code);

        // Composite key for Codebook Items (CodebookCode + ItemCode)
        builder.Entity<CodebookItem>().HasKey(ci => new { ci.CodebookCode, ci.Code });

        // Primary key for FitnessGroup
        builder.Entity<FitnessGroup>().HasKey(fg => fg.Id);


        builder.Entity<Codebook>().HasData(
            new Codebook { Code = "StavPosudku", NameCz = "Stavy lékařských posudků", NameEn = "Medical Certificate Statuses" },
            new Codebook { Code = "TypOpravneni", NameCz = "Typy řidičských oprávnění", NameEn = "Driving License Types" }
        );

        builder.Entity<CodebookItem>().HasData(
            // --- Statuses ---
            new CodebookItem { CodebookCode = "StavPosudku", Code = "VYDANO", DescriptionCz = "Posudek byl vydán", DescriptionEn = "Certificate Issued", IsActive = true },
            new CodebookItem { CodebookCode = "StavPosudku", Code = "ZAMITNUTO", DescriptionCz = "Posudek byl zamítnut", DescriptionEn = "Certificate Rejected", IsActive = true },
            new CodebookItem { CodebookCode = "StavPosudku", Code = "ZNEPLATNENO", DescriptionCz = "Posudek byl zneplatněn", DescriptionEn = "Certificate Invalidated", IsActive = true },

            // --- Driving License Types ---
            new CodebookItem { CodebookCode = "TypOpravneni", Code = "A", DescriptionCz = "Motocykly", DescriptionEn = "Motorcycles", IsActive = true },
            new CodebookItem { CodebookCode = "TypOpravneni", Code = "B", DescriptionCz = "Osobní automobily", DescriptionEn = "Passenger cars", IsActive = true },
            new CodebookItem { CodebookCode = "TypOpravneni", Code = "C", DescriptionCz = "Nákladní automobily", DescriptionEn = "Trucks", IsActive = true },
            new CodebookItem { CodebookCode = "TypOpravneni", Code = "D", DescriptionCz = "Autobusy", DescriptionEn = "Buses", IsActive = true },
            new CodebookItem { CodebookCode = "TypOpravneni", Code = "T", DescriptionCz = "Traktory", DescriptionEn = "Tractors", IsActive = true }
        );

        var doctorSmithId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var doctorNovakId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        builder.Entity<DriverFitnessCertificate>().HasData(
            new
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                PersonalId = "ABC-123-456",
                MedicalProfessionalId = doctorSmithId,
                IssueDate = new DateTime(2025, 1, 15, 8, 0, 0, DateTimeKind.Utc),
                StatusCode = "VYDANO",
            },
            new
            {
                Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                PersonalId = "XYZ-987-654",
                MedicalProfessionalId = doctorSmithId,
                IssueDate = new DateTime(2026, 2, 10, 9, 30, 0, DateTimeKind.Utc),
                StatusCode = "ZAMITNUTO",
            },
            new
            {
                Id = Guid.Parse("c3333333-3333-3333-3333-333333333333"),
                PersonalId = "ABC-123-456",
                MedicalProfessionalId = doctorNovakId,
                IssueDate = new DateTime(2020, 5, 20, 14, 15, 0, DateTimeKind.Utc),
                StatusCode = "ZNEPLATNENO",
            },
            new
            {
                Id = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
                PersonalId = "LMN-456-789",
                MedicalProfessionalId = doctorNovakId,
                IssueDate = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
                StatusCode = "VYDANO",
            }
        );
    }
}