namespace Elp.Domain.Entities;

/// <summary>
/// Represents a specific fitness assessment for a driving group.
/// Maps to API concept: Způsobilosti (PosudekRoZpusobilostCreateDto).
/// </summary>
public class FitnessGroup
{
    public Guid Id { get; private set; }

    // Foreign Key back to the parent certificate
    public Guid DriverFitnessCertificateId { get; private set; }

    // API: skupinaZadateleRidic (e.g., "Group B", "Group C")
    public string DriverGroupCode { get; private set; }

    // API: vysledek (e.g., "Fit", "Fit with conditions")
    public string ResultCode { get; private set; }

    // Navigation property for EF Core
    public DriverFitnessCertificate Certificate { get; private set; } = null!;

    private FitnessGroup() { } // For EF Core

    public static FitnessGroup Create(string driverGroupCode, string resultCode)
    {
        return new FitnessGroup
        {
            Id = Guid.NewGuid(),
            DriverGroupCode = driverGroupCode,
            ResultCode = resultCode
        };
    }
}