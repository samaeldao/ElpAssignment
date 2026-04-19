namespace Elp.Application.Certificates.Queries.GetCertificateHistory
{
    public class CertificateHistoryItemDto
    {
        public DateTime ChangedAt { get; set; }
        public string Action { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
    }
}
