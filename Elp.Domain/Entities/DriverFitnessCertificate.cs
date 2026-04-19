using Elp.Domain.Constants;

namespace Elp.Domain.Entities;

/// <summary>
/// Represents a Driver Fitness Medical Certificate. 
/// Maps to API concept: Posudek o zdravotní způsobilosti k řízení motorových vozidel (PosudekRo).
/// </summary>
public class DriverFitnessCertificate
{
    public Guid Id { get; private set; }

    // API: rid (Rodné číslo / Patient's Personal ID)
    public string PersonalId { get; private set; }

    // API: krzpId (ID of the medical professional)
    public Guid MedicalProfessionalId { get; private set; }

    // API: datumVystaveni
    public DateTime IssueDate { get; private set; }

    // API: platnostDo
    public DateTime? ExpirationDate { get; private set; }

    // API: stavPosudku (State of the certificate, e.g., Platný, Zneplatněný)
    public string StatusCode { get; private set; }

    // API: zpusobilosti (Assessments/Fitness groups assigned to this certificate)
    public ICollection<FitnessGroup> FitnessGroups { get; private set; } = new List<FitnessGroup>();

    // Add this specifically for the If-Match optimistic concurrency requirement
    public byte[] RowVersion { get; private set; }

    private DriverFitnessCertificate() { } // Required by EF Core

    public static DriverFitnessCertificate Create(
        string personalId,
        Guid medicalProfessionalId,
        DateTime issueDate,
        string statusCode,
        DateTime? expirationDate)
    {
        return new DriverFitnessCertificate
        {
            Id = Guid.NewGuid(),
            PersonalId = personalId,
            MedicalProfessionalId = medicalProfessionalId,
            IssueDate = issueDate,
            StatusCode = statusCode,
            ExpirationDate = expirationDate
        };
    }

    public void Invalidate()
    {
        StatusCode = CertificateStatusCodes.Invalidated;
    }

    // Add this inside the DriverFitnessCertificate class
    public void UpdateStatus(string newStatusCode)
    {
        StatusCode = newStatusCode;
    }
}