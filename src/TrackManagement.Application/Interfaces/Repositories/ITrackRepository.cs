using TrackManagement.Application.DTOs;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Application.Interfaces.Repositories;

public interface ITrackRepository
{
    Task<Track?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Track?> GetByIdWithDistributionsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Track>> GetFilteredAsync(TrackFilter filter, CancellationToken cancellationToken = default);
    Task<bool> IsrcExistsAsync(string isrc, CancellationToken cancellationToken = default);
    Task AddAsync(Track track, CancellationToken cancellationToken = default);
}
