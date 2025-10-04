using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreateModelRequest(
    string TrainingStatus,
    string FilePath,
    bool IsUsed,
    DateTime TrainingDataStart,
    DateTime TrainingDataEnd,
    double? Accuracy,
    double? MAE,
    double? RMSE,
    double? R2Score
);

public record ModelDto(
    Guid Id,
    string TrainingStatus,
    string FilePath,
    bool IsUsed,
    DateTime TrainingDataStart,
    DateTime TrainingDataEnd,
    double? Accuracy,
    double? MAE,
    double? RMSE,
    double? R2Score,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdateModelRequest(
    Guid Id,
    string? TrainingStatus,
    string? FilePath,
    bool? IsUsed,
    DateTime? TrainingDataStart,
    DateTime? TrainingDataEnd,
    double? Accuracy,
    double? MAE,
    double? RMSE,
    double? R2Score
) : BaseUpdateRequest(Id);

// Training-related models
public record TrainModelRequest(
    DateTime TrainingDataStart,
    DateTime TrainingDataEnd
);

public record TrainModelResponse(
    Guid ModelId,
    string Message
);

public record ProcessedDataRow(
    Guid PlantationId,
    int PlantationAgeDays,
    double AvgTemperature,
    double AvgHumidity,
    double AvgSoilMoisture,
    double AvgLightIntensity,
    double TotalYieldKg,
    DateTime PeriodStart,
    DateTime PeriodEnd
);

public record TrainingCompleteRequest(
    Guid ModelId,
    double Accuracy,
    double MAE,
    double RMSE,
    double R2Score
);

public record TrainingCompleteWithFileRequest(
    Guid ModelId,
    Stream ModelFile,
    string FileName,
    double Accuracy,
    double MAE,
    double RMSE,
    double R2Score
);