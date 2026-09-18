using FluentValidation;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Exceptions;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Application.Interfaces.Services;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Enums;

namespace TrackManagement.Application.Services;

public class TrackService(
    ITrackRepository trackRepository,
    IArtistRepository artistRepository,
    IDspRepository dspRepository,
    ITrackDistributionRepository trackDistributionRepository,
    IUnitOfWork unitOfWork,
    IValidator<CreateTrackRequest> createValidator,
    IValidator<DistributeTrackRequest> distributeValidator,
    IValidator<UpdateTrackStatusRequest> updateStatusValidator) : ITrackService
{
    public async Task<List<TrackDto>> GetFilteredAsync(TrackFilter filter, CancellationToken cancellationToken = default)
    {
        var tracks = await trackRepository.GetFilteredAsync(filter, cancellationToken);
        return tracks.Select(t => ToDto(t)).ToList();
    }

    public async Task<TrackDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var track = await trackRepository.GetByIdWithDistributionsAsync(id, cancellationToken);
        return track is null ? null : ToDetailDto(track);
    }

    public async Task<TrackDto> CreateAsync(CreateTrackRequest request, CancellationToken cancellationToken = default)
    {
        await createValidator.ValidateAndThrowAsync(request, cancellationToken);

        var artist = await artistRepository.GetByIdAsync(request.ArtistId, cancellationToken)
            ?? throw new NotFoundException($"Artist '{request.ArtistId}' was not found.");

        if (await trackRepository.IsrcExistsAsync(request.Isrc, cancellationToken))
        {
            throw new ConflictException($"A track with ISRC '{request.Isrc}' already exists.");
        }

        var track = new Track
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            ArtistId = artist.Id,
            Isrc = request.Isrc,
            ReleaseDate = request.ReleaseDate,
            Genre = request.Genre,
            Status = TrackStatus.Draft
        };

        await trackRepository.AddAsync(track, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(track, artist.Name);
    }

    public async Task<TrackDetailDto> DistributeAsync(Guid trackId, DistributeTrackRequest request, CancellationToken cancellationToken = default)
    {
        await distributeValidator.ValidateAndThrowAsync(request, cancellationToken);

        var track = await trackRepository.GetByIdWithDistributionsAsync(trackId, cancellationToken)
            ?? throw new NotFoundException($"Track '{trackId}' was not found.");

        var requestedDsps = await dspRepository.GetByIdsAsync(request.DspIds, cancellationToken);
        var foundDspIds = requestedDsps.Select(d => d.Id).ToHashSet();
        var missingDspIds = request.DspIds.Where(id => !foundDspIds.Contains(id)).ToList();
        if (missingDspIds.Count > 0)
        {
            throw new NotFoundException($"Unknown DSP id(s): {string.Join(", ", missingDspIds)}.");
        }

        var alreadyDistributedDspIds = track.Distributions.Select(d => d.DspId).ToHashSet();
        var newDistributions = request.DspIds
            .Where(dspId => !alreadyDistributedDspIds.Contains(dspId))
            .Select(dspId => new TrackDistribution
            {
                Id = Guid.NewGuid(),
                TrackId = track.Id,
                DspId = dspId,
                SubmittedAt = DateTime.UtcNow,
                Status = DistributionStatus.Pending
            })
            .ToList();

        if (newDistributions.Count > 0)
        {
            // EF Core's change-tracker fixup already adds these into the already-loaded
            // track.Distributions collection once tracked, since Track and TrackDistribution
            // are related via TrackId. Adding them again here would duplicate the in-memory
            // list (and therefore the response), even though only one row is ever persisted.
            await trackDistributionRepository.AddRangeAsync(newDistributions, cancellationToken);
        }

        if (track.Status == TrackStatus.Draft)
        {
            track.Status = TrackStatus.Submitted;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDetailDto(track);
    }

    public async Task<TrackDto> UpdateStatusAsync(Guid trackId, UpdateTrackStatusRequest request, CancellationToken cancellationToken = default)
    {
        await updateStatusValidator.ValidateAndThrowAsync(request, cancellationToken);

        var track = await trackRepository.GetByIdWithDistributionsAsync(trackId, cancellationToken)
            ?? throw new NotFoundException($"Track '{trackId}' was not found.");

        track.Status = request.Status;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(track, track.Artist?.Name ?? string.Empty);
    }

    private static TrackDto ToDto(Track track, string? artistName = null) => new(
        track.Id,
        track.Title,
        track.ArtistId,
        artistName ?? track.Artist?.Name ?? string.Empty,
        track.Isrc,
        track.ReleaseDate,
        track.Genre,
        track.Status);

    private static TrackDetailDto ToDetailDto(Track track) => new(
        track.Id,
        track.Title,
        track.ArtistId,
        track.Artist?.Name ?? string.Empty,
        track.Isrc,
        track.ReleaseDate,
        track.Genre,
        track.Status,
        track.Distributions.Select(d => new TrackDistributionDto(
            d.Id,
            d.DspId,
            d.Dsp?.Name ?? string.Empty,
            d.SubmittedAt,
            d.Status)).ToList());
}
