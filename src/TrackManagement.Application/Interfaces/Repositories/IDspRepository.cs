using TrackManagement.Domain.Entities;

namespace TrackManagement.Application.Interfaces.Repositories;

public interface IDspRepository
{
    Task<List<Dsp>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Dsp>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
