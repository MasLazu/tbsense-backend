using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreateAiSessionChatRequestValidator : AbstractValidator<CreateAiSessionChatRequest>
{
    public CreateAiSessionChatRequestValidator()
    {
        RuleFor(x => x.SessionId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Content)
            .NotEmpty();
    }
}

public class UpdateAiSessionChatRequestValidator : AbstractValidator<UpdateAiSessionChatRequest>
{
    public UpdateAiSessionChatRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.SessionId)
            .NotEmpty()
            .When(x => x.SessionId.HasValue);

        RuleFor(x => x.Role)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Role));

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Order.HasValue);

        RuleFor(x => x.Content)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Content));
    }
}