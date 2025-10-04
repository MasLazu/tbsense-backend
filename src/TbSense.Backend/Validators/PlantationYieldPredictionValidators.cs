using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreatePlantationYieldPredictionRequestValidator : AbstractValidator<CreatePlantationYieldPredictionRequest>
{
    public CreatePlantationYieldPredictionRequestValidator()
    {
        RuleFor(x => x.PlantationId)
            .NotEmpty();

        RuleFor(x => x.ModelId)
            .NotEmpty();

        RuleFor(x => x.Timestamp)
            .NotEmpty();
    }
}

public class UpdatePlantationYieldPredictionRequestValidator : AbstractValidator<UpdatePlantationYieldPredictionRequest>
{
    public UpdatePlantationYieldPredictionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.PlantationId)
            .NotEmpty()
            .When(x => x.PlantationId.HasValue);

        RuleFor(x => x.ModelId)
            .NotEmpty()
            .When(x => x.ModelId.HasValue);

        RuleFor(x => x.Timestamp)
            .NotEmpty()
            .When(x => x.Timestamp.HasValue);
    }
}