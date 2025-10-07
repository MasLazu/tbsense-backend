using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class AiSessionChat : BaseEntity
{
    public Guid SessionId { get; set; }
    public string Role { get; set; } = null!;
    public int Order { get; set; }
    public string Content { get; set; } = null!;

    public AiSession AiSession { get; set; } = null!;
}