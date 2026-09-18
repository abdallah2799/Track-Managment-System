using FluentValidation;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Exceptions;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Application.Interfaces.Services;

namespace TrackManagement.Application.Services;

public class AuthService(
    IAppUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IValidator<LoginRequest> loginValidator) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        await loginValidator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid username or password.");
        }

        var token = jwtTokenGenerator.GenerateToken(user);
        return new LoginResponse(token.Token, token.ExpiresAt);
    }
}
