using TrackManagement.Domain.Enums;

namespace TrackManagement.Domain.Entities;

public class TrackDistribution
{
    public Guid Id { get; set; }
    public Guid TrackId { get; set; }
    public Guid DspId { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DistributionStatus Status { get; set; } = DistributionStatus.Pending;

    public Track? Track { get; set; }
    public Dsp? Dsp { get; set; }
}
