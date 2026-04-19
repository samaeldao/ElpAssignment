namespace Elp.Api.Models;

public class CiselnikPolozkaDto
{
    public string Kod { get; set; } = string.Empty;
    public string Popis { get; set; } = string.Empty;
    public bool Aktivni { get; set; }
}