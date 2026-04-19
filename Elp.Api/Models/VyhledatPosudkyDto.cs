namespace Elp.Api.Models;

public class VyhledatPosudkyDto
{
    // Filter by specific driver
    public string? Rid { get; set; }

    // Filter by specific doctor
    public Guid? KrzpId { get; set; }

    // Filter by status (e.g., "VYDANO")
    public string? StavPosudku { get; set; }

    // Date range filters
    public DateTime? DatumOd { get; set; }
    public DateTime? DatumDo { get; set; }

    // Pagination (with safe defaults)
    public int Stranka { get; set; } = 1;
    public int VelikostStranky { get; set; } = 20;
}