using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreateTreeRequestValidator : AbstractValidator<CreateTreeRequest>
{
    public CreateTreeRequestValidator()
    {
        RuleFor(x => x.PlantationId)
            .NotEmpty();
    }
}

public class UpdateTreeRequestValidator : AbstractValidator<UpdateTreeRequest>
{
    public UpdateTreeRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.PlantationId)
            .NotEmpty()
            .When(x => x.PlantationId.HasValue);
    }
}