using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Exceptions;
using TrackManagement.Application.Interfaces.Services;
using TrackManagement.Domain.Enums;

namespace TrackManagement.Api.Controllers;

[ApiController]
[Route("api/tracks")]
public class TracksController(ITrackService trackService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TrackDto>>> GetFiltered(
        [FromQuery] Guid? artistId,
        [FromQuery] string? genre,
        [FromQuery] TrackStatus? status,
        CancellationToken cancellationToken)
    {
        var tracks = await trackService.GetFilteredAsync(new TrackFilter(artistId, genre, status), cancellationToken);
        return Ok(tracks);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrackDetailDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var track = await trackService.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Track '{id}' was not found.");

        return Ok(track);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TrackDto>> Create(CreateTrackRequest request, CancellationToken cancellationToken)
    {
        var track = await trackService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = track.Id }, track);
    }

    [HttpPost("{id:guid}/distribute")]
    [Authorize]
    public async Task<ActionResult<TrackDetailDto>> Distribute(Guid id, DistributeTrackRequest request, CancellationToken cancellationToken)
    {
        var track = await trackService.DistributeAsync(id, request, cancellationToken);
        return Ok(track);
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize]
    public async Task<ActionResult<TrackDto>> UpdateStatus(Guid id, UpdateTrackStatusRequest request, CancellationToken cancellationToken)
    {
        var track = await trackService.UpdateStatusAsync(id, request, cancellationToken);
        return Ok(track);
    }
}
