using FluentValidation;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Exceptions;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Application.Interfaces.Services;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Application.Services;

public class ArtistService(
    IArtistRepository artistRepository,
    IUnitOfWork unitOfWork,
    IValidator<CreateArtistRequest> createValidator) : IArtistService
{
    public async Task<List<ArtistDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var artists = await artistRepository.GetAllAsync(cancellationToken);
        return artists.Select(ToDto).ToList();
    }

    public async Task<ArtistDto> CreateAsync(CreateArtistRequest request, CancellationToken cancellationToken = default)
    {
        await createValidator.ValidateAndThrowAsync(request, cancellationToken);

        if (await artistRepository.EmailExistsAsync(request.Email, cancellationToken))
        {
            throw new ConflictException($"An artist with email '{request.Email}' already exists.");
        }

        var artist = new Artist
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Country = request.Country
        };

        await artistRepository.AddAsync(artist, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(artist);
    }

    private static ArtistDto ToDto(Artist artist) =>
        new(artist.Id, artist.Name, artist.Email, artist.Country);
}
