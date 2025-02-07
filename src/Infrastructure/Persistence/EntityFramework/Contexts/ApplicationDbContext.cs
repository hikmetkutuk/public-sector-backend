using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFramework.Contexts;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Definition> Definitions { get; set; }
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<OrganisationLog> OrganisationLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Organisation>()
            .HasOne(o => o.ConvertedFrom)
            .WithMany()
            .HasForeignKey(o => o.ConvertedFromId);
    }
}