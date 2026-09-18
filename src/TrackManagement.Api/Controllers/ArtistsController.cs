using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Interfaces.Services;

namespace TrackManagement.Api.Controllers;

[ApiController]
[Route("api/artists")]
public class ArtistsController(IArtistService artistService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ArtistDto>>> GetAll(CancellationToken cancellationToken)
    {
        var artists = await artistService.GetAllAsync(cancellationToken);
        return Ok(artists);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ArtistDto>> Create(CreateArtistRequest request, CancellationToken cancellationToken)
    {
        var artist = await artistService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAll), artist);
    }
}
