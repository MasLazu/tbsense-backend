using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class SystemPrompt : BaseEntity
{
    public string Prompt { get; set; } = null!;
}