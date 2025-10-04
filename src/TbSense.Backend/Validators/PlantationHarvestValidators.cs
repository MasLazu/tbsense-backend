using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreatePlantationHarvestRequestValidator : AbstractValidator<CreatePlantationHarvestRequest>
{
    public CreatePlantationHarvestRequestValidator()
    {
        RuleFor(x => x.PlantationId)
            .NotEmpty();

        RuleFor(x => x.YieldKg)
            .GreaterThan(0);

        RuleFor(x => x.HarvestDate)
            .NotEmpty();
    }
}

public class UpdatePlantationHarvestRequestValidator : AbstractValidator<UpdatePlantationHarvestRequest>
{
    public UpdatePlantationHarvestRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.PlantationId)
            .NotEmpty()
            .When(x => x.PlantationId.HasValue);

        RuleFor(x => x.YieldKg)
            .GreaterThan(0)
            .When(x => x.YieldKg.HasValue);

        RuleFor(x => x.HarvestDate)
            .NotEmpty()
            .When(x => x.HarvestDate.HasValue);
    }
}