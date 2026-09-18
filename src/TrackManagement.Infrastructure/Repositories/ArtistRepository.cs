using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Persistence;

namespace TrackManagement.Infrastructure.Repositories;

public class ArtistRepository(TrackManagementDbContext dbContext) : IArtistRepository
{
    public Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Artists.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public Task<List<Artist>> GetAllAsync(CancellationToken cancellationToken = default) =>
        dbContext.Artists.AsNoTracking().OrderBy(a => a.Name).ToListAsync(cancellationToken);

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default) =>
        dbContext.Artists.AnyAsync(a => a.Email == email, cancellationToken);

    public async Task AddAsync(Artist artist, CancellationToken cancellationToken = default) =>
        await dbContext.Artists.AddAsync(artist, cancellationToken);
}
