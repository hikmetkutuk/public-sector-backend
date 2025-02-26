using MediatR;

namespace Application.Features.Organisation.Queries.GetByParentId;

public record GetOrganisationByParentIdQuery(Guid? ParentId) : IRequest<IEnumerable<GetOrganisationByParentIdDto>>;