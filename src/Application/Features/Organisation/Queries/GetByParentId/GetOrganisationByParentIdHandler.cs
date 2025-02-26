using System.Data;
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
        // 📌 Cache key will be different if parentId is null
        var cacheKey = request.ParentId.HasValue
            ? $"organisation:parentId:{request.ParentId}"
            : "organisation:organisationType:0";

        var cachedData = await redisCache.GetAsync<IEnumerable<GetOrganisationByParentIdDto>>(cacheKey);
        if (cachedData != null && cachedData.Any())
        {
            return cachedData.ToList();
        }

        try
        {
            using var connection = sqlConnectionFactory.CreateConnection();
            string sql;
            object parameters;

            if (request.ParentId.HasValue)
            {
                sql = @"
                    SELECT 
                        d.id, 
                        d.name, 
                        d.organisation_type AS OrganisationType, 
                        d.parent_id AS ParentId,
                        p.name AS ParentName
                    FROM organisations d
                    LEFT JOIN organisations p ON d.parent_id = p.id
                    WHERE d.parent_id = @ParentId";
                parameters = new { ParentId = request.ParentId };
            }
            else
            {
                sql = @"
                    SELECT 
                        d.id, 
                        d.name, 
                        d.organisation_type AS OrganisationType, 
                        d.parent_id AS ParentId,
                        p.name AS ParentName
                    FROM organisations d
                    LEFT JOIN organisations p ON d.parent_id = p.id
                    WHERE d.organisation_type = 0";
                parameters = null;
            }

            var organisations = await connection.QueryAsync<GetOrganisationByParentIdDto>(
                sql,
                parameters,
                commandType: CommandType.Text);

            var result = organisations.ToList();

            if (result.Any())
            {
                await redisCache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            }

            logger.LogInformation(
                "Organizations were successfully retrieved from the database.");

            return result;
        }
        catch (SqlException ex)
        {
            logger.LogError(ex,
                "An error occurred while importing organizations from the database: {Message}", ex.Message);
            throw new ApplicationException(
                $"An error occurred while importing organizations from the database: {ex.Message}",
                ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "An unexpected error occurred while importing organizations.");
            throw new ApplicationException(
                "An unexpected error occurred while importing organizations.",
                ex);
        }
    }
}