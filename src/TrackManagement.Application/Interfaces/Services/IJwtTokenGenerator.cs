using TrackManagement.Domain.Entities;

namespace TrackManagement.Application.Interfaces.Services;

public record GeneratedToken(string Token, DateTime ExpiresAt);

public interface IJwtTokenGenerator
{
    GeneratedToken GenerateToken(AppUser user);
}
