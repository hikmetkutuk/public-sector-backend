using System.Data;
using Application.Common.Interfaces;
using Dapper;
using Domain.Enums;
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
            SELECT 
                d.id, 
                d.name, 
                d.type, 
                d.parent_id,
                p.name AS parent_name
            FROM definitions d
            LEFT JOIN definitions p ON d.parent_id = p.id
            WHERE d.type = @Type
            ORDER BY d.name";

            var definitions =
                await connection.QueryAsync<dynamic>(sql, new { request.Type }, commandType: CommandType.Text);

            var getDefinitionByTypeDtos = definitions.Select(d => new GetDefinitionByTypeDto
            {
                Id = d.id,
                Name = d.name,
                Type = d.type,
                ParentId = d.parent_id,
                ParentName = d.parent_name
            }).ToList();


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