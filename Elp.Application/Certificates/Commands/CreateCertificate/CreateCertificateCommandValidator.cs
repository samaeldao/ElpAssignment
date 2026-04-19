using FluentValidation;
using Microsoft.Extensions.Localization;
using Elp.Application.Resources; // Ensure this points to your SharedResource.cs folder!

namespace Elp.Application.Certificates.Commands.CreateCertificate;

public class CreateCertificateCommandValidator : AbstractValidator<CreateCertificateCommand>
{
    public CreateCertificateCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(v => v.PersonalId)
            .NotEmpty().WithMessage(x => localizer["PersonalIdRequired"])
            .MaximumLength(50).WithMessage(x => localizer["PersonalIdMaxLength"]);

        RuleFor(v => v.MedicalProfessionalId)
            .NotEmpty().WithMessage(x => localizer["MedicalProfessionalIdRequired"]);

        RuleFor(v => v.IssueDate)
            .NotEmpty().WithMessage(x => localizer["IssueDateRequired"])
            .LessThanOrEqualTo(x => DateTime.UtcNow).WithMessage(x => localizer["IssueDateFuture"]);

        RuleFor(v => v.StatusCode)
            .NotEmpty().WithMessage(x => localizer["StatusCodeRequired"]);
    }
}