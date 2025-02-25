using Application.Common.Interfaces;
using Domain.Events.Organisation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Organisation;

public class OrganisationCreatedEventHandler(IRedisCache redisCache, ILogger<OrganisationCreatedEventHandler> logger)
    : INotificationHandler<OrganisationCreatedEvent>
{
    public async Task Handle(OrganisationCreatedEvent notification, CancellationToken cancellationToken)
    {
        var organisation = notification.Organisation;
        var cacheKey = $"organisation:id:{organisation.Id}";

        try
        {
            // 📌 Adding a log record
            logger.LogInformation(
                "Organisation created. Id: {OrganisationId}, Name: {OrganisationName}, Type: {OrganisationType}",
                organisation.Id, organisation.Name, organisation.OrganisationType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while ceating organisation with Id: {OrganisationId}",
                organisation.Id);
        }
    }
}