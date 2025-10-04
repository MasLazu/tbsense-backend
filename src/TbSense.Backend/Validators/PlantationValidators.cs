using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreatePlantationRequestValidator : AbstractValidator<CreatePlantationRequest>
{
    public CreatePlantationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.LandAreaHectares)
            .GreaterThan(0);

        RuleFor(x => x.PlantedDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Planted date cannot be in the future");
    }
}

public class UpdatePlantationRequestValidator : AbstractValidator<UpdatePlantationRequest>
{
    public UpdatePlantationRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.LandAreaHectares)
            .GreaterThan(0)
            .When(x => x.LandAreaHectares.HasValue);

        RuleFor(x => x.PlantedDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Planted date cannot be in the future")
            .When(x => x.PlantedDate.HasValue);
    }
}