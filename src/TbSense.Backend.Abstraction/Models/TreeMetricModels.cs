using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreateTreeMetricRequest(
    Guid TreeId,
    float SoilMoisture,
    float SoilTemperature,
    float AirTemperature,
    DateTime Timestamp
);

public record TreeMetricDto(
    Guid Id,
    Guid TreeId,
    float SoilMoisture,
    float SoilTemperature,
    float AirTemperature,
    DateTime Timestamp,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdateTreeMetricRequest(
    Guid Id,
    Guid? TreeId,
    float? SoilMoisture,
    float? SoilTemperature,
    float? AirTemperature,
    DateTime? Timestamp
) : BaseUpdateRequest(Id);