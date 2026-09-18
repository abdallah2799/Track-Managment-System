namespace TrackManagement.Domain.Entities;

public class Dsp
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<TrackDistribution> Distributions { get; set; } = new List<TrackDistribution>();
}
