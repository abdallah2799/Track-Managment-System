using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Persistence;

namespace TrackManagement.Infrastructure.Repositories;

public class TrackRepository(TrackManagementDbContext dbContext) : ITrackRepository
{
    public Task<Track?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Tracks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public Task<Track?> GetByIdWithDistributionsAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Tracks
            .Include(t => t.Artist)
            .Include(t => t.Distributions)
                .ThenInclude(d => d.Dsp)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<List<Track>> GetFilteredAsync(TrackFilter filter, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Tracks.Include(t => t.Artist).AsQueryable();

        if (filter.ArtistId is { } artistId)
        {
            query = query.Where(t => t.ArtistId == artistId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Genre))
        {
            query = query.Where(t => t.Genre == filter.Genre);
        }

        if (filter.Status is { } status)
        {
            query = query.Where(t => t.Status == status);
        }

        return await query.AsNoTracking().OrderBy(t => t.Title).ToListAsync(cancellationToken);
    }

    public Task<bool> IsrcExistsAsync(string isrc, CancellationToken cancellationToken = default) =>
        dbContext.Tracks.AnyAsync(t => t.Isrc == isrc, cancellationToken);

    public async Task AddAsync(Track track, CancellationToken cancellationToken = default) =>
        await dbContext.Tracks.AddAsync(track, cancellationToken);
}
