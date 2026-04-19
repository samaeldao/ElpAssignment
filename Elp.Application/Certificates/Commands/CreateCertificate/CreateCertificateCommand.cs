using MediatR;

namespace Elp.Application.Certificates.Commands.CreateCertificate;

public class CreateCertificateCommand : IRequest<Guid>
{
    // Add this parameterless constructor for AutoMapper
    private CreateCertificateCommand() { }

    public CreateCertificateCommand(string personalId, Guid medicalProfessionalId, DateTime issueDate, string statusCode)
    {
        PersonalId = personalId;
        MedicalProfessionalId = medicalProfessionalId;
        IssueDate = issueDate;
        StatusCode = statusCode;
    }

    public string PersonalId { get; set; } = string.Empty;
    public Guid MedicalProfessionalId { get; set; }
    public DateTime IssueDate { get; set; }
    public string StatusCode { get; set; } = string.Empty;
}