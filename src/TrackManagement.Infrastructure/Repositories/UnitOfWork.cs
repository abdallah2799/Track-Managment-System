using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Infrastructure.Persistence;

namespace TrackManagement.Infrastructure.Repositories;

public class UnitOfWork(TrackManagementDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
