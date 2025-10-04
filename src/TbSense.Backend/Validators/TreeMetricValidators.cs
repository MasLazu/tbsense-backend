using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreateTreeMetricRequestValidator : AbstractValidator<CreateTreeMetricRequest>
{
    public CreateTreeMetricRequestValidator()
    {
        RuleFor(x => x.TreeId)
            .NotEmpty();

        RuleFor(x => x.SoilMoisture)
            .InclusiveBetween(0, 100);

        RuleFor(x => x.SoilTemperature)
            .InclusiveBetween(-50, 50);

        RuleFor(x => x.AirTemperature)
            .InclusiveBetween(-50, 50);

        RuleFor(x => x.Timestamp)
            .NotEmpty();
    }
}

public class UpdateTreeMetricRequestValidator : AbstractValidator<UpdateTreeMetricRequest>
{
    public UpdateTreeMetricRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.TreeId)
            .NotEmpty()
            .When(x => x.TreeId.HasValue);

        RuleFor(x => x.SoilMoisture)
            .InclusiveBetween(0, 100)
            .When(x => x.SoilMoisture.HasValue);

        RuleFor(x => x.SoilTemperature)
            .InclusiveBetween(-50, 50)
            .When(x => x.SoilTemperature.HasValue);

        RuleFor(x => x.AirTemperature)
            .InclusiveBetween(-50, 50)
            .When(x => x.AirTemperature.HasValue);

        RuleFor(x => x.Timestamp)
            .NotEmpty()
            .When(x => x.Timestamp.HasValue);
    }
}