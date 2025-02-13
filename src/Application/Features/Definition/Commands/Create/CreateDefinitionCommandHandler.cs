using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using Application.Features.Definition.Queries.GetByType;
using MediatR;

namespace Application.Features.Definition.Commands.Create;

public sealed class CreateDefinitionCommandHandler(IApplicationDbContext dbContext, IRedisCache redisCache)
    : IRequestHandler<CreateDefinitionCommand, ResponseDto<Guid>>
{
    public async Task<ResponseDto<Guid>> Handle(CreateDefinitionCommand request, CancellationToken cancellationToken)
    {
        var definition = Domain.Entities.Definition.Create(request.Name, request.Type, request.Code, request.ParentId);

        dbContext.Definitions.Add(definition);

        var cacheKey = $"definition:type:{request.Type}";

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            var cachedData = await redisCache.GetAsync<List<GetDefinitionByTypeDto>>(cacheKey);

            if (cachedData != null)
            {
                cachedData.Add(new GetDefinitionByTypeDto
                {
                    Id = definition.Id,
                    Name = definition.Name,
                    Type = (int)definition.Type,
                    ParentId = definition.ParentId,
                });

                await redisCache.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
            }


            return ResponseDto<Guid>.Success(definition.Id, "Definition created successfully.");
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while creating the definition.", ex);
        }
    }
}