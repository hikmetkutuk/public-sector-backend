using Application.Common.Interfaces;
using Application.Features.Definition.Queries.GetByType;
using Domain.Events;
using Domain.Events.Definition;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Definition;

public sealed class DefinitionCreatedEventHandler(IRedisCache redisCache, ILogger<DefinitionCreatedEventHandler> logger)
    : INotificationHandler<DefinitionCreatedEvent>
{
    public async Task Handle(DefinitionCreatedEvent notification, CancellationToken cancellationToken)
    {
        var definition = notification.Definition;
        var cacheKey = $"definition:type:{definition.Type}";

        try
        {
            // 📌 Fetch data from cache
            var cachedData = await redisCache.GetAsync<List<GetDefinitionByTypeDto>>(cacheKey);

            if (cachedData != null)
            {
                cachedData.Add(new GetDefinitionByTypeDto
                {
                    Id = definition.Id,
                    Name = definition.Name,
                    Type = (int)definition.Type,
                    Code = definition.Code,
                    ParentId = definition.ParentId
                });

                // 📌 Update cache
                await redisCache.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
            }

            // 📌 Adding a log record
            logger.LogInformation(
                "Definition created. Id: {DefinitionId}, Name: {DefinitionName}, Type: {DefinitionType}",
                definition.Id, definition.Name, definition.Type);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating cache for definition Id: {DefinitionId}",
                definition.Id);
        }
    }
}