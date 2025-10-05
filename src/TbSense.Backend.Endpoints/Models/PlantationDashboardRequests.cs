namespace TbSense.Backend.Endpoints.Models;

public record PlantationIdRequest
{
    public Guid PlantationId { get; set; }
}

public record PlantationDashboardQueryRequest
{
    public Guid PlantationId { get; set; }
    public string? Period { get; set; }
    public int? Days { get; set; }
}
