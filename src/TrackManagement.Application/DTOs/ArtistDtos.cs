namespace TrackManagement.Application.DTOs;

public record ArtistDto(Guid Id, string Name, string Email, string Country);

public record CreateArtistRequest(string Name, string Email, string Country);
