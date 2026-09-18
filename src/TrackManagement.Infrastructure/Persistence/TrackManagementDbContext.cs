using Microsoft.EntityFrameworkCore;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Infrastructure.Persistence;

public class TrackManagementDbContext(DbContextOptions<TrackManagementDbContext> options) : DbContext(options)
{
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<Dsp> Dsps => Set<Dsp>();
    public DbSet<TrackDistribution> TrackDistributions => Set<TrackDistribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
