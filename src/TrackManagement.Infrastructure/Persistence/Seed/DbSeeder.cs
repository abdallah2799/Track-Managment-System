using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.Interfaces.Services;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Enums;

namespace TrackManagement.Infrastructure.Persistence.Seed;

public static class DbSeeder
{
    public const string SeedAdminUsername = "admin";
    public const string SeedAdminPassword = "Admin@12345";

    public static async Task SeedAsync(
        TrackManagementDbContext context,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);

        if (await context.Artists.AnyAsync(cancellationToken))
        {
            return;
        }

        var amrDiab = new Artist { Id = Guid.NewGuid(), Name = "Amr Diab", Email = "amr.diab@example.com", Country = "Egypt" };
        var tamerHosny = new Artist { Id = Guid.NewGuid(), Name = "Tamer Hosny", Email = "tamer.hosny@example.com", Country = "Egypt" };
        var tamerAshour = new Artist { Id = Guid.NewGuid(), Name = "Tamer Ashour", Email = "tamer.ashour@example.com", Country = "Egypt" };
        var sherineAbdelwahab = new Artist { Id = Guid.NewGuid(), Name = "Sherine Abdelwahab", Email = "sherine.abdelwahab@example.com", Country = "Egypt" };

        await context.Artists.AddRangeAsync([amrDiab, tamerHosny, tamerAshour, sherineAbdelwahab], cancellationToken);

        var spotify = new Dsp { Id = Guid.NewGuid(), Name = "Spotify" };
        var anghami = new Dsp { Id = Guid.NewGuid(), Name = "Anghami" };
        var soundcloud = new Dsp { Id = Guid.NewGuid(), Name = "SoundCloud" };
        var youtubeMusic = new Dsp { Id = Guid.NewGuid(), Name = "YouTube Music" };
        var appleMusic = new Dsp { Id = Guid.NewGuid(), Name = "Apple Music" };

        await context.Dsps.AddRangeAsync([spotify, anghami, soundcloud, youtubeMusic, appleMusic], cancellationToken);

        var layaliAlQahira = new Track { Id = Guid.NewGuid(), Title = "ليالي القاهرة", ArtistId = amrDiab.Id, Isrc = "EGAD12400001", ReleaseDate = new DateOnly(2025, 3, 14), Genre = "Pop", Status = TrackStatus.Draft };
        var qalbiMaak = new Track { Id = Guid.NewGuid(), Title = "قلبي معاك", ArtistId = amrDiab.Id, Isrc = "EGAD12400002", ReleaseDate = new DateOnly(2025, 5, 2), Genre = "Dance", Status = TrackStatus.Submitted };
        var hikayatHob = new Track { Id = Guid.NewGuid(), Title = "حكاية حب", ArtistId = tamerHosny.Id, Isrc = "EGTH12400001", ReleaseDate = new DateOnly(2024, 11, 8), Genre = "Pop", Status = TrackStatus.Distributed };
        var winAlHikaya = new Track { Id = Guid.NewGuid(), Title = "وين الحكاية", ArtistId = tamerHosny.Id, Isrc = "EGTH12400002", ReleaseDate = new DateOnly(2025, 1, 20), Genre = "Ballad", Status = TrackStatus.Submitted };
        var laylWNas = new Track { Id = Guid.NewGuid(), Title = "ليل وناس", ArtistId = tamerAshour.Id, Isrc = "EGTA12400001", ReleaseDate = new DateOnly(2024, 8, 30), Genre = "Alternative", Status = TrackStatus.Distributed };
        var alaMahlak = new Track { Id = Guid.NewGuid(), Title = "على مهلك", ArtistId = tamerAshour.Id, Isrc = "EGTA12400002", ReleaseDate = new DateOnly(2025, 6, 17), Genre = "Fusion", Status = TrackStatus.Draft };
        var hamsetHanin = new Track { Id = Guid.NewGuid(), Title = "همسة حنين", ArtistId = sherineAbdelwahab.Id, Isrc = "EGSA12400001", ReleaseDate = new DateOnly(2025, 4, 5), Genre = "R&B", Status = TrackStatus.Submitted };
        var laylAlShita = new Track { Id = Guid.NewGuid(), Title = "ليل الشتا", ArtistId = sherineAbdelwahab.Id, Isrc = "EGSA12400002", ReleaseDate = new DateOnly(2024, 12, 1), Genre = "Pop", Status = TrackStatus.Distributed };
        var bidayaJadida = new Track { Id = Guid.NewGuid(), Title = "بداية جديدة", ArtistId = sherineAbdelwahab.Id, Isrc = "EGSA12400003", ReleaseDate = new DateOnly(2025, 7, 11), Genre = "Pop", Status = TrackStatus.Draft };

        await context.Tracks.AddRangeAsync(
            [layaliAlQahira, qalbiMaak, hikayatHob, winAlHikaya, laylWNas, alaMahlak, hamsetHanin, laylAlShita, bidayaJadida],
            cancellationToken);

        TrackDistribution Distribution(Track track, Dsp dsp, DistributionStatus status, DateTime submittedAt) => new()
        {
            Id = Guid.NewGuid(),
            TrackId = track.Id,
            DspId = dsp.Id,
            Status = status,
            SubmittedAt = submittedAt
        };

        var now = DateTime.UtcNow;
        var distributions = new List<TrackDistribution>
        {
            Distribution(hikayatHob, spotify, DistributionStatus.Live, now.AddDays(-60)),
            Distribution(hikayatHob, anghami, DistributionStatus.Live, now.AddDays(-60)),
            Distribution(laylWNas, spotify, DistributionStatus.Live, now.AddDays(-45)),
            Distribution(laylWNas, youtubeMusic, DistributionStatus.Live, now.AddDays(-45)),
            Distribution(laylAlShita, anghami, DistributionStatus.Live, now.AddDays(-30)),
            Distribution(laylAlShita, appleMusic, DistributionStatus.Pending, now.AddDays(-5)),
            Distribution(qalbiMaak, spotify, DistributionStatus.Pending, now.AddDays(-3)),
            Distribution(winAlHikaya, soundcloud, DistributionStatus.Pending, now.AddDays(-2)),
            Distribution(hamsetHanin, anghami, DistributionStatus.Pending, now.AddDays(-1)),
            Distribution(hamsetHanin, spotify, DistributionStatus.Rejected, now.AddDays(-4))
        };

        await context.TrackDistributions.AddRangeAsync(distributions, cancellationToken);

        var adminUser = new AppUser
        {
            Id = Guid.NewGuid(),
            Username = SeedAdminUsername,
            PasswordHash = passwordHasher.Hash(SeedAdminPassword)
        };

        await context.AppUsers.AddAsync(adminUser, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}
