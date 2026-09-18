using TrackManagement.Application.DTOs;

namespace TrackManagement.Application.Interfaces.Services;

public interface ITrackService
{
    Task<List<TrackDto>> GetFilteredAsync(TrackFilter filter, CancellationToken cancellationToken = default);
    Task<TrackDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TrackDto> CreateAsync(CreateTrackRequest request, CancellationToken cancellationToken = default);
    Task<TrackDetailDto> DistributeAsync(Guid trackId, DistributeTrackRequest request, CancellationToken cancellationToken = default);
    Task<TrackDto> UpdateStatusAsync(Guid trackId, UpdateTrackStatusRequest request, CancellationToken cancellationToken = default);
}
