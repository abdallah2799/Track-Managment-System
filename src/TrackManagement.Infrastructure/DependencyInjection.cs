using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Infrastructure.Persistence;
using TrackManagement.Infrastructure.Repositories;

namespace TrackManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? "Data Source=trackmanagement.db";

        services.AddDbContext<TrackManagementDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<ITrackRepository, TrackRepository>();
        services.AddScoped<IDspRepository, DspRepository>();
        services.AddScoped<ITrackDistributionRepository, TrackDistributionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
