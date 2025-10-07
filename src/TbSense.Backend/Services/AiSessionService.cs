using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class AiSessionService : CrudService<AiSession, AiSessionDto, CreateAiSessionRequest, UpdateAiSessionRequest>, IAiSessionService
{
    public AiSessionService(
        IRepository<AiSession> repository,
        IReadRepository<AiSession> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<AiSession> propertyMap,
        IPaginationValidator<AiSession> paginationValidator,
        ICursorPaginationValidator<AiSession> cursorPaginationValidator,
        IValidator<CreateAiSessionRequest>? createValidator = null,
        IValidator<UpdateAiSessionRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}