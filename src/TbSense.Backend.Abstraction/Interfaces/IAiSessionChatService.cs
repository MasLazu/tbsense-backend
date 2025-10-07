using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Abstraction.Interfaces;

public interface IAiSessionChatService : ICrudService<AiSessionChatDto, CreateAiSessionChatRequest, UpdateAiSessionChatRequest>
{
}