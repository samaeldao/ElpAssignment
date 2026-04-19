namespace Elp.Api.Models
{
    public class PosudekRoDetailDto
    {
        public string Rid { get; set; } = string.Empty;
        public string KrzpId { get; set; } = string.Empty;
        public DateTime DatumVystaveni { get; set; }
        public StavPosudkuDto StavPosudku { get; set; } = new();

        public string? RowVersion { get; set; }
    }
}
