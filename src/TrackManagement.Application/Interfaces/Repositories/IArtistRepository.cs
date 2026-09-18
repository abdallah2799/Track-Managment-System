using TrackManagement.Domain.Entities;

namespace TrackManagement.Application.Interfaces.Repositories;

public interface IArtistRepository
{
    Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Artist>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(Artist artist, CancellationToken cancellationToken = default);
}
