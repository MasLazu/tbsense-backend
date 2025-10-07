using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class AiSessionChatService : CrudService<AiSessionChat, AiSessionChatDto, CreateAiSessionChatRequest, UpdateAiSessionChatRequest>, IAiSessionChatService
{
    public AiSessionChatService(
        IRepository<AiSessionChat> repository,
        IReadRepository<AiSessionChat> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<AiSessionChat> propertyMap,
        IPaginationValidator<AiSessionChat> paginationValidator,
        ICursorPaginationValidator<AiSessionChat> cursorPaginationValidator,
        IValidator<CreateAiSessionChatRequest>? createValidator = null,
        IValidator<UpdateAiSessionChatRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}