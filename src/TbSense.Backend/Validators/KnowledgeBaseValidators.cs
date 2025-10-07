using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreateKnowledgeBaseRequestValidator : AbstractValidator<CreateKnowledgeBaseRequest>
{
    public CreateKnowledgeBaseRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty();
    }
}

public class UpdateKnowledgeBaseRequestValidator : AbstractValidator<UpdateKnowledgeBaseRequest>
{
    public UpdateKnowledgeBaseRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Content)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Content));
    }
}