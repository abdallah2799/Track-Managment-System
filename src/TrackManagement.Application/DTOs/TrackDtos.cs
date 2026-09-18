using TrackManagement.Domain.Enums;

namespace TrackManagement.Application.DTOs;

public record TrackDto(
    Guid Id,
    string Title,
    Guid ArtistId,
    string ArtistName,
    string Isrc,
    DateOnly ReleaseDate,
    string Genre,
    TrackStatus Status);

public record TrackDetailDto(
    Guid Id,
    string Title,
    Guid ArtistId,
    string ArtistName,
    string Isrc,
    DateOnly ReleaseDate,
    string Genre,
    TrackStatus Status,
    List<TrackDistributionDto> Distributions);

public record CreateTrackRequest(
    string Title,
    Guid ArtistId,
    string Isrc,
    DateOnly ReleaseDate,
    string Genre);

public record UpdateTrackStatusRequest(TrackStatus Status);

public record TrackFilter(Guid? ArtistId, string? Genre, TrackStatus? Status);
