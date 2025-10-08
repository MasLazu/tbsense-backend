using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreateSystemPromptRequestValidator : AbstractValidator<CreateSystemPromptRequest>
{
    public CreateSystemPromptRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Prompt)
            .NotEmpty();
    }
}

public class UpdateSystemPromptRequestValidator : AbstractValidator<UpdateSystemPromptRequest>
{
    public UpdateSystemPromptRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Prompt)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Prompt));
    }
}