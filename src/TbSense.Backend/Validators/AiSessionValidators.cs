using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreateAiSessionRequestValidator : AbstractValidator<CreateAiSessionRequest>
{
    public CreateAiSessionRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.LastActivityAt)
            .NotEmpty();
    }
}

public class UpdateAiSessionRequestValidator : AbstractValidator<UpdateAiSessionRequest>
{
    public UpdateAiSessionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.LastActivityAt)
            .NotEmpty()
            .When(x => x.LastActivityAt.HasValue);
    }
}