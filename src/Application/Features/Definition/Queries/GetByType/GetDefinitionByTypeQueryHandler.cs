using Application.Common.Interfaces;
using Dapper;
using MediatR;

namespace Application.Features.Definition.Queries.GetByType;

public sealed class GetDefinitionByTypeQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IRequestHandler<GetDefinitionByTypeQuery, IEnumerable<GetDefinitionByTypeDto>>
{
    public async Task<IEnumerable<GetDefinitionByTypeDto>> Handle(GetDefinitionByTypeQuery request, CancellationToken cancellationToken)
    {
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
                new { Type = request.Type }
            );
            return definitions;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while retrieving definitions.", ex);
        }
    }
}