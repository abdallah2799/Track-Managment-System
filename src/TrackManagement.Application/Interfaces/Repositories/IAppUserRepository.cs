using TrackManagement.Domain.Entities;

namespace TrackManagement.Application.Interfaces.Repositories;

public interface IAppUserRepository
{
    Task<AppUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
