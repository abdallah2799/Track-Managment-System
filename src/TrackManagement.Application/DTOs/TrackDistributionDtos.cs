using TrackManagement.Domain.Enums;

namespace TrackManagement.Application.DTOs;

public record TrackDistributionDto(
    Guid Id,
    Guid DspId,
    string DspName,
    DateTime SubmittedAt,
    DistributionStatus Status);

public record DistributeTrackRequest(List<Guid> DspIds);
