using Application.Common.Interfaces;
using Application.Features.Definition.Queries.GetByType;
using Domain.Events;
using MediatR;

namespace Application.Events;

public class DefinitionUpdatedEventHandler(IRedisCache redisCache) : INotificationHandler<DefinitionUpdatedEvent>
{
    public async Task Handle(DefinitionUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var definition = notification.Definition;
        var cacheKey = $"definition:type:{definition.Type}";

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

            await redisCache.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
        }
    }
}