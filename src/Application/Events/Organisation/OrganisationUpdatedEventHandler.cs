using Application.Common.Interfaces;
using Application.Features.Organisation.Queries.GetByParentId;
using Domain.Enums;
using Domain.Events.Organisation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Organisation;

public class OrganisationUpdatedEventHandler(IRedisCache redisCache, ILogger<OrganisationUpdatedEventHandler> logger)
    : INotificationHandler<OrganisationUpdatedEvent>
{
    public async Task Handle(OrganisationUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var organisation = notification.Organisation;
        var cacheKey = $"organisation:organisationType:{organisation.OrganisationType}";

        try
        {
            // 📌 Fetch data from cache
            var cachedData = await redisCache.GetAsync<List<GetOrganisationByParentIdDto>>(cacheKey);

            if (cachedData != null)
            {
                var existingItem = cachedData.FirstOrDefault(d => d.Id == organisation.Id);
                if (existingItem != null)
                {
                    existingItem.Name = organisation.Name;
                    existingItem.OrganisationType = (OrganisationType)organisation.OrganisationType!;
                    existingItem.ParentId = organisation.ParentId;
                    existingItem.Address = organisation.Address;
                    existingItem.Email = organisation.Email;
                    existingItem.PhoneNumber = organisation.PhoneNumber;
                    existingItem.Latitude = organisation.Latitude;
                    existingItem.Longitude = organisation.Longitude;
                    existingItem.CenterClass = organisation.CenterClass;
                    existingItem.LocalStatus = (int?)organisation.LocalStatus;
                    existingItem.AtmCount = organisation.AtmCount;
                    existingItem.BranchCount = organisation.BranchCount;
                    existingItem.DistributionStatus = (int?)organisation.DistributionStatus;
                    existingItem.WageTakeMachine = organisation.WageTakeMachine;
                    existingItem.WagePayMachine = organisation.WagePayMachine;
                    existingItem.DocumentDate = organisation.DocumentDate;
                    existingItem.DocumentNumber = organisation.DocumentNumber;
                    existingItem.Note = organisation.Note;
                    existingItem.ProcedureRegionDefId = organisation.ProcedureRegionDefId;
                    existingItem.GeographicRegionDefId = organisation.GeographicRegionDefId;
                }

                // 📌 Update cache
                await redisCache.SetAsync(cacheKey, cachedData, TimeSpan.FromMinutes(10));
            }

            logger.LogInformation(
                "Organisation updated. Id: {OrganisationId}, Name: {OrganisationName}, Type: {OrganisationType}",
                organisation.Id, organisation.Name, organisation.OrganisationType);
        }
        catch (Exception ex)
        {
            // 📌 Error log
            logger.LogError(ex, "An error occurred while updating cache for organisation Id: {OrganisationId}",
                organisation.Id);
        }
    }
}