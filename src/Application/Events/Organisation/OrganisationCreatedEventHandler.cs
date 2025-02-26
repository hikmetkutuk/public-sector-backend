using Application.Common.Interfaces;
using Application.Features.Organisation.Queries.GetByParentId;
using Domain.Enums;
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
        var cacheKeyById = $"organisation:id:{organisation.Id}";

        try
        {
            logger.LogInformation(
                "Organisation created. Id: {OrganisationId}, Name: {OrganisationName}, Type: {OrganisationType}, ParentId: {ParentId}",
                organisation.Id, organisation.Name, organisation.OrganisationType, organisation.ParentId);

            // 📌 Add new organization to cache with its own ID
            var organisationDto = new GetOrganisationByParentIdDto
            {
                Id = organisation.Id,
                Name = organisation.Name,
                OrganisationType = (OrganisationType)organisation.OrganisationType!,
                ParentId = organisation.ParentId,
            };
            await redisCache.SetAsync(cacheKeyById, organisationDto, TimeSpan.FromMinutes(10));
            logger.LogInformation("Cached organisation with key: {CacheKey}", cacheKeyById);

            // 📌 Update caches based on ParentId or OrganisationType
            if (organisation.ParentId.HasValue)
            {
                var cacheKeyByParent = $"organisation:parentId:{organisation.ParentId}";
                var cachedParentData =
                    await redisCache.GetAsync<IEnumerable<GetOrganisationByParentIdDto>>(cacheKeyByParent);
                var updatedParentData = cachedParentData?.ToList() ?? new List<GetOrganisationByParentIdDto>();
                updatedParentData.Add(organisationDto);
                await redisCache.SetAsync(cacheKeyByParent, updatedParentData, TimeSpan.FromMinutes(10));
                logger.LogInformation("Updated parent cache with key: {CacheKey}, Total items: {Count}",
                    cacheKeyByParent, updatedParentData.Count);
            }
            else if (organisation.OrganisationType == 0)
            {
                var cacheKeyByType = "organisation:organisationType:0";
                var cachedTypeData =
                    await redisCache.GetAsync<IEnumerable<GetOrganisationByParentIdDto>>(cacheKeyByType);
                var updatedTypeData = cachedTypeData?.ToList() ?? new List<GetOrganisationByParentIdDto>();
                updatedTypeData.Add(organisationDto);
                await redisCache.SetAsync(cacheKeyByType, updatedTypeData, TimeSpan.FromMinutes(10));
                logger.LogInformation("Updated type cache with key: {CacheKey}, Total items: {Count}",
                    cacheKeyByType, updatedTypeData.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while handling organisation creation with Id: {OrganisationId}",
                organisation.Id);
            throw;
        }
    }
}