using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;

namespace TbSense.Backend.Services;

public class SystemPromptService : CrudService<SystemPrompt, SystemPromptDto, CreateSystemPromptRequest, UpdateSystemPromptRequest>, ISystemPromptService
{
    public SystemPromptService(
        IRepository<SystemPrompt> repository,
        IReadRepository<SystemPrompt> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<SystemPrompt> propertyMap,
        IPaginationValidator<SystemPrompt> paginationValidator,
        ICursorPaginationValidator<SystemPrompt> cursorPaginationValidator,
        IValidator<CreateSystemPromptRequest>? createValidator = null,
        IValidator<UpdateSystemPromptRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
    }
}