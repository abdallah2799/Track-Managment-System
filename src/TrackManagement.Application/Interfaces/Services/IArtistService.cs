using TrackManagement.Application.DTOs;

namespace TrackManagement.Application.Interfaces.Services;

public interface IArtistService
{
    Task<List<ArtistDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ArtistDto> CreateAsync(CreateArtistRequest request, CancellationToken cancellationToken = default);
}
