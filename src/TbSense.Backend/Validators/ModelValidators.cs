using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Validators;

public class CreateModelRequestValidator : AbstractValidator<CreateModelRequest>
{
    public CreateModelRequestValidator()
    {
        RuleFor(x => x.TrainingStatus)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.TrainingDataStart)
            .NotEmpty();

        RuleFor(x => x.TrainingDataEnd)
            .NotEmpty()
            .GreaterThan(x => x.TrainingDataStart);

        RuleFor(x => x.Accuracy)
            .InclusiveBetween(0, 1)
            .When(x => x.Accuracy.HasValue);

        RuleFor(x => x.MAE)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MAE.HasValue);

        RuleFor(x => x.RMSE)
            .GreaterThanOrEqualTo(0)
            .When(x => x.RMSE.HasValue);

        RuleFor(x => x.R2Score)
            .InclusiveBetween(-1, 1)
            .When(x => x.R2Score.HasValue);
    }
}

public class UpdateModelRequestValidator : AbstractValidator<UpdateModelRequest>
{
    public UpdateModelRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.TrainingStatus)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.TrainingStatus));

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.FilePath));

        RuleFor(x => x.TrainingDataStart)
            .NotEmpty()
            .When(x => x.TrainingDataStart.HasValue);

        RuleFor(x => x.TrainingDataEnd)
            .NotEmpty()
            .GreaterThan(x => x.TrainingDataStart)
            .When(x => x.TrainingDataEnd.HasValue && x.TrainingDataStart.HasValue);

        RuleFor(x => x.Accuracy)
            .InclusiveBetween(0, 1)
            .When(x => x.Accuracy.HasValue);

        RuleFor(x => x.MAE)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MAE.HasValue);

        RuleFor(x => x.RMSE)
            .GreaterThanOrEqualTo(0)
            .When(x => x.RMSE.HasValue);

        RuleFor(x => x.R2Score)
            .InclusiveBetween(-1, 1)
            .When(x => x.R2Score.HasValue);
    }
}

public class TrainModelRequestValidator : AbstractValidator<TrainModelRequest>
{
    public TrainModelRequestValidator()
    {
        RuleFor(x => x.TrainingDataStart)
            .NotEmpty()
            .WithMessage("Training data start date is required");

        RuleFor(x => x.TrainingDataEnd)
            .NotEmpty()
            .WithMessage("Training data end date is required")
            .GreaterThan(x => x.TrainingDataStart)
            .WithMessage("Training data end date must be after start date");
    }
}

public class TrainingCompleteRequestValidator : AbstractValidator<TrainingCompleteRequest>
{
    public TrainingCompleteRequestValidator()
    {
        RuleFor(x => x.ModelId)
            .NotEmpty()
            .WithMessage("Model ID is required");

        RuleFor(x => x.Accuracy)
            .InclusiveBetween(0, 1)
            .WithMessage("Accuracy must be between 0 and 1");

        RuleFor(x => x.MAE)
            .GreaterThanOrEqualTo(0)
            .WithMessage("MAE must be greater than or equal to 0");

        RuleFor(x => x.RMSE)
            .GreaterThanOrEqualTo(0)
            .WithMessage("RMSE must be greater than or equal to 0");

        RuleFor(x => x.R2Score)
            .InclusiveBetween(-1, 1)
            .WithMessage("R2 Score must be between -1 and 1");
    }
}
