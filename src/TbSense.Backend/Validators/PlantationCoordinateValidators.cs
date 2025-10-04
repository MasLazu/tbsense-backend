using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreatePlantationCoordinateRequestValidator : AbstractValidator<CreatePlantationCoordinateRequest>
{
    public CreatePlantationCoordinateRequestValidator()
    {
        RuleFor(x => x.PlantationId)
            .NotEmpty();

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90);
    }
}

public class UpdatePlantationCoordinateRequestValidator : AbstractValidator<UpdatePlantationCoordinateRequest>
{
    public UpdatePlantationCoordinateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.PlantationId)
            .NotEmpty()
            .When(x => x.PlantationId.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .When(x => x.Latitude.HasValue);
    }
}