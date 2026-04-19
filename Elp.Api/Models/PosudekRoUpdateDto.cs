namespace Elp.Api.Models;

public class PosudekRoUpdateDto
{
    // Re-using the same sub-component we separated earlier!
    public StavPosudkuDto StavPosudku { get; set; } = new();
}