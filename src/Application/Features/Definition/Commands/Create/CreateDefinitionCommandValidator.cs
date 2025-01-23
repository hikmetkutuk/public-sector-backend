using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Definition.Commands.Create;

public sealed class CreateDefinitionCommandValidator : AbstractValidator<CreateDefinitionCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateDefinitionCommandValidator(IApplicationDbContext dbContext)
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
    }

    private async Task<bool> IsDefinitionNameUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return !await _dbContext
            .Definitions
            .AnyAsync(c => c.Name.ToLower() == name.ToLower(), cancellationToken);
    }
}