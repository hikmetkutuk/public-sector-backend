using Application.Common.Interfaces;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Application.Features.Organisation.Queries.GetByParentId;

public sealed class GetOrganisationByParentIdHandler(
    ISqlConnectionFactory sqlConnectionFactory,
    IRedisCache redisCache,
    ILogger<GetOrganisationByParentIdHandler> logger)
    : IRequestHandler<GetOrganisationByParentIdQuery, IEnumerable<GetOrganisationByParentIdDto>>
{
    public async Task<IEnumerable<GetOrganisationByParentIdDto>> Handle(GetOrganisationByParentIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = request.ParentId.HasValue
            ? $"organisation:parentId:{request.ParentId}:withChildren"
            : "organisation:organisationType:0:withChildren";

        var cachedData = await redisCache.GetAsync<IEnumerable<GetOrganisationByParentIdDto>>(cacheKey);
        if (cachedData != null && cachedData.Any())
        {
            logger.LogInformation("Retrieved {Count} items from cache with key: {CacheKey}", cachedData.Count(),
                cacheKey);
            return cachedData.ToList();
        }

        try
        {
            using var connection = sqlConnectionFactory.CreateConnection();
            string sql;
            object parameters;

            // 📌 Single query to fetch both parent and child organizations
            if (request.ParentId.HasValue)
            {
                sql = @"
                    SELECT 
                        p.id AS Id, 
                        p.name AS Name, 
                        p.organisation_type AS OrganisationType, 
                        p.parent_id AS ParentId,
                        pp.name AS ParentName,
                        c.id AS Id, 
                        c.name AS Name, 
                        c.organisation_type AS OrganisationType, 
                        c.parent_id AS ParentId,
                        p.name AS ParentName
                    FROM organisations p
                    LEFT JOIN organisations pp ON p.parent_id = pp.id
                    LEFT JOIN organisations c ON c.parent_id = p.id
                    WHERE p.parent_id = @ParentId";
                parameters = new { ParentId = request.ParentId };
            }
            else
            {
                sql = @"
                    SELECT 
                        p.id AS Id, 
                        p.name AS Name, 
                        p.organisation_type AS OrganisationType, 
                        p.parent_id AS ParentId,
                        pp.name AS ParentName,
                        c.id AS Id, 
                        c.name AS Name, 
                        c.organisation_type AS OrganisationType, 
                        c.parent_id AS ParentId,
                        p.name AS ParentName
                    FROM organisations p
                    LEFT JOIN organisations pp ON p.parent_id = pp.id
                    LEFT JOIN organisations c ON c.parent_id = p.id
                    WHERE p.organisation_type = 0";
                parameters = null;
            }

            // 📌 Dictionary to store parent organizations and their children
            var organisationDict = new Dictionary<Guid, GetOrganisationByParentIdDto>();

            await connection
                .QueryAsync<GetOrganisationByParentIdDto, GetOrganisationByParentIdDto, GetOrganisationByParentIdDto>(
                    sql,
                    (parent, child) =>
                    {
                        // 📌 Ensure parent exists in dictionary
                        if (!organisationDict.TryGetValue(parent.Id, out var parentEntry))
                        {
                            parentEntry = parent;
                            organisationDict.Add(parent.Id, parentEntry);
                        }

                        // 📌 If child exists, add it to the parent's Children collection
                        if (child != null && child.Id != Guid.Empty) // Check to avoid null/empty child rows
                        {
                            parentEntry.Children.Add(child);
                        }

                        return parentEntry;
                    },
                    parameters,
                    splitOn: "Id");

            var result = organisationDict.Values.ToList();

            if (result.Any())
            {
                await redisCache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));
                logger.LogInformation("Organizations with children were successfully retrieved from the database.");
            }
            else
            {
                logger.LogInformation("No organizations found for the given criteria.");
            }

            return result;
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "An error occurred while importing organizations from the database: {Message}",
                ex.Message);
            throw new ApplicationException(
                $"An error occurred while importing organizations from the database: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while importing organizations.");
            throw new ApplicationException("An unexpected error occurred while importing organizations.", ex);
        }
    }
}