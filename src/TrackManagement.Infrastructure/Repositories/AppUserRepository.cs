using Microsoft.EntityFrameworkCore;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Persistence;

namespace TrackManagement.Infrastructure.Repositories;

public class AppUserRepository(TrackManagementDbContext dbContext) : IAppUserRepository
{
    public Task<AppUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
        dbContext.AppUsers.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
}
