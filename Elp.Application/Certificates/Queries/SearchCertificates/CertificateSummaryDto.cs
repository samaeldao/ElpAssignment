namespace Elp.Application.Certificates.Queries.SearchCertificates;

public class CertificateSummaryDto
{
    public Guid Id { get; set; }
    public string PersonalId { get; set; } = string.Empty;
    public Guid MedicalProfessionalId { get; set; }
    public DateTime IssueDate { get; set; }
    public string StatusCode { get; set; } = string.Empty;
}