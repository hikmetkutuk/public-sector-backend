using Application.Common.Interfaces;
using Application.Features.Definition.Queries.GetByType;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events;

public class DefinitionUpdatedEventHandler(IRedisCache redisCache, ILogger<DefinitionUpdatedEventHandler> logger)
    : INotificationHandler<DefinitionUpdatedEvent>
{
    public async Task Handle(DefinitionUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var definition = notification.Definition;
        var cacheKey = $"definition:type:{definition.Type}";

        try
        {
            // 📌 Fetch data from cache
            var cachedData = await redisCache.GetAsync<List<GetDefinitionByTypeDto>>(cacheKey);

            if (cachedData != null)
            {
                var existingItem = cachedData.FirstOrDefault(d => d.Id == definition.Id);
                if (existingItem != null)
                {
                    existingItem.Name = definition.Name;
                    existingItem.Type = (int)definition.Type;
                    existingItem.Code = existingItem.Code;
                    existingItem.ParentId = definition.ParentId;
                }

                // 📌 Update cache
                await redisCache.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
            }

            logger.LogInformation(
                "Definition updated. Id: {DefinitionId}, Name: {DefinitionName}, Type: {DefinitionType}",
                definition.Id, definition.Name, definition.Type);
        }
        catch (Exception ex)
        {
            // 📌 Error log
            logger.LogError(ex, "An error occurred while updating cache for definition Id: {DefinitionId}",
                definition.Id);
        }
    }
}