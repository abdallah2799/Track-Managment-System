using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Persistence;

namespace TrackManagement.Infrastructure.Repositories;

public class DspRepository(TrackManagementDbContext dbContext) : IDspRepository
{
    public Task<List<Dsp>> GetAllAsync(CancellationToken cancellationToken = default) =>
        dbContext.Dsps.AsNoTracking().OrderBy(d => d.Name).ToListAsync(cancellationToken);

    public Task<List<Dsp>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default) =>
        dbContext.Dsps.Where(d => ids.Contains(d.Id)).ToListAsync(cancellationToken);
}
