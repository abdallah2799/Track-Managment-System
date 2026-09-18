using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TrackManagement.Application.Interfaces.Services;
using TrackManagement.Application.Services;

namespace TrackManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        services.AddScoped<IArtistService, ArtistService>();
        services.AddScoped<ITrackService, TrackService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
