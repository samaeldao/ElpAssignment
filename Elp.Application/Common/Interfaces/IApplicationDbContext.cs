using Elp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Elp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<DriverFitnessCertificate> DriverFitnessCertificates { get; }
    DbSet<FitnessGroup> FitnessGroups { get; }
    DbSet<Codebook> Codebooks { get; }
    DbSet<CodebookItem> CodebookItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}