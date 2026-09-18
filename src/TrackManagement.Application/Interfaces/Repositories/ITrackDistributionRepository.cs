using TrackManagement.Domain.Entities;

namespace TrackManagement.Application.Interfaces.Repositories;

public interface ITrackDistributionRepository
{
    Task<List<TrackDistribution>> GetByTrackIdAsync(Guid trackId, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TrackDistribution> distributions, CancellationToken cancellationToken = default);
}
