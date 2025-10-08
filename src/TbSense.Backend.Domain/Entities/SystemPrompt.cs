using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class SystemPrompt : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Prompt { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}