using Application.Common.Interfaces;
using Dapper;
using MediatR;

namespace Application.Features.Definition.Queries.GetByType;

public sealed class GetDefinitionByTypeQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IRedisCache redisCache)
    : IRequestHandler<GetDefinitionByTypeQuery, IEnumerable<GetDefinitionByTypeDto>>
{
    public async Task<IEnumerable<GetDefinitionByTypeDto>> Handle(GetDefinitionByTypeQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"definition:type:{request.Type}";

        var cachedData = await redisCache.GetAsync<IEnumerable<GetDefinitionByTypeDto>>(cacheKey);
        if (cachedData != null)
        {
            var getDefinitionByTypeDtos = cachedData.ToList();

            return getDefinitionByTypeDtos;
        }

        try
        {
            using var connection = sqlConnectionFactory.CreateConnection();
            var sql = @"
                SELECT id, name 
                FROM definitions 
                WHERE type = @Type 
                ORDER BY name
            ";
            var definitions = await connection.QueryAsync<GetDefinitionByTypeDto>(
                sql,
                new { request.Type }
            );

            var getDefinitionByTypeDtos = definitions.ToList();
            if (getDefinitionByTypeDtos.Any())
            {
                await redisCache.SetAsync(cacheKey, getDefinitionByTypeDtos, TimeSpan.FromMinutes(10));
            }

            return getDefinitionByTypeDtos;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while retrieving definitions.", ex);
        }
    }
}