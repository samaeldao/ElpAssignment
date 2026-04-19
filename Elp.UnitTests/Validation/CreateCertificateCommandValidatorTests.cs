using Elp.Application.Certificates.Commands.CreateCertificate;
using Elp.Application.Resources;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;

namespace Elp.UnitTests.Validation;

public class CreateCertificateCommandValidatorTests
{
    private readonly CreateCertificateCommandValidator _validator;
    private readonly Mock<IStringLocalizer<SharedResource>> _localizerMock;

    public CreateCertificateCommandValidatorTests()
    {
        _localizerMock = new Mock<IStringLocalizer<SharedResource>>();

        _localizerMock.Setup(l => l[It.IsAny<string>()])
                      .Returns((string key) => new LocalizedString(key, key));

        _validator = new CreateCertificateCommandValidator(_localizerMock.Object);
    }

    [Fact]
    public void Should_Have_Error_When_PersonalId_Is_Empty()
    {
        var command = new CreateCertificateCommand(string.Empty, Guid.NewGuid(), DateTime.UtcNow, "VYDANO");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PersonalId)
              .WithErrorMessage("PersonalIdRequired");
    }

    [Fact]
    public void Should_Have_Error_When_MedicalProfessionalId_Is_Empty()
    {
        var command = new CreateCertificateCommand("ABC-123", Guid.Empty, DateTime.UtcNow, "VYDANO");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.MedicalProfessionalId)
              .WithErrorMessage("MedicalProfessionalIdRequired");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var validPastDate = DateTime.UtcNow.AddDays(-1);

        var command = new CreateCertificateCommand("ABC-123", Guid.NewGuid(), validPastDate, "VYDANO");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}