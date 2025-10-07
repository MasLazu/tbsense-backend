using MasLazu.AspNet.Framework.Domain.Entities;

namespace TbSense.Backend.Domain.Entities;

public class KnowledgeBase : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}