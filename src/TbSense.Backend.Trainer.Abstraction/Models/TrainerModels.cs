namespace TbSense.Backend.Trainer.Abstraction.Models;

public record TrainingRequest(
    Guid ModelId,
    List<TrainingDataRow> TrainingData
);

public record TrainingDataRow(
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

public record TrainingResponse(
    Guid ModelId,
    string Status,
    string Message
);

public record TrainingStatusResponse(
    Guid ModelId,
    string Status,
    string? Message,
    double? Progress,
    DateTime? StartedAt,
    DateTime? CompletedAt
);
public record TrainingCompletionNotification(
    Guid ModelId,
    string FilePath,
    double Accuracy,
    double MAE,
    double RMSE,
    double R2Score,
    string Status,
    string? ErrorMessage
);
