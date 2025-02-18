using Application.Common.Interfaces;
using Application.Features.Definition.Queries.GetByType;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events;

public sealed class DefinitionDeletedEventHandler(IRedisCache redisCache, ILogger<DefinitionDeletedEventHandler> logger)
    : INotificationHandler<DefinitionDeletedEvent>
{
    private readonly IRedisCache _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));

    private readonly ILogger<DefinitionDeletedEventHandler> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task Handle(DefinitionDeletedEvent notification, CancellationToken cancellationToken)
    {
        var definition = notification.Definition;
        var cacheKey = $"definition:type:{definition.Type}";

        try
        {
            var cachedData = await _redisCache.GetAsync<List<GetDefinitionByTypeDto>>(cacheKey);

            if (cachedData != null)
            {
                // 📌 Remove the deleted Definition from cache
                var itemToRemove = cachedData.FirstOrDefault(d => d.Id == definition.Id);
                if (itemToRemove != null)
                {
                    cachedData.Remove(itemToRemove);
                    await _redisCache.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
                }
            }

            _logger.LogInformation(
                "Definition deleted. Id: {DefinitionId}, Name: {DefinitionName}, Type: {DefinitionType}",
                definition.Id, definition.Name, definition.Type);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating cache for deleted definition Id: {DefinitionId}",
                definition.Id);
        }
    }
}