using Elp.Api.Models;

public class PosudekRoCreateDto
{
    public string Rid { get; set; } = string.Empty;

    public Guid? KrzpId { get; set; }

    public DateTime? DatumVystaveni { get; set; }

    public StavPosudkuDto StavPosudku { get; set; } = new();
}