using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Definition.Commands.Update;

public sealed class UpdateDefinitionCommandValidator : AbstractValidator<UpdateDefinitionCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateDefinitionCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Definition name is required.")
            .MaximumLength(100)
            .WithMessage("Definition name must not exceed 100 characters.")
            .MinimumLength(3)
            .WithMessage("Definition name must be at least 3 characters long.");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Definition type is required.");

        RuleFor(x => x.Id)
            .MustAsync(DefinitionExistsAsync)
            .WithMessage("The specified definition does not exist.");
    }

    private async Task<bool> DefinitionExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Definitions.AnyAsync(d => d.Id == id, cancellationToken);
    }
}