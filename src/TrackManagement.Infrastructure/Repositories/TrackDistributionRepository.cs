using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Persistence;

namespace TrackManagement.Infrastructure.Repositories;

public class TrackDistributionRepository(TrackManagementDbContext dbContext) : ITrackDistributionRepository
{
    public Task<List<TrackDistribution>> GetByTrackIdAsync(Guid trackId, CancellationToken cancellationToken = default) =>
        dbContext.TrackDistributions
            .Include(d => d.Dsp)
            .Where(d => d.TrackId == trackId)
            .ToListAsync(cancellationToken);

    public async Task AddRangeAsync(IEnumerable<TrackDistribution> distributions, CancellationToken cancellationToken = default) =>
        await dbContext.TrackDistributions.AddRangeAsync(distributions, cancellationToken);
}
