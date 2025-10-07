using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class AiSession : BaseEntity
{
    public string Title { get; set; } = null!;
    public DateTime LastActivityAt { get; set; }

    public ICollection<AiSessionChat> AiSessionChats { get; set; } = new List<AiSessionChat>();
}