using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TrackManagement.Infrastructure.Persistence;

public class TrackManagementDbContextFactory : IDesignTimeDbContextFactory<TrackManagementDbContext>
{
    public TrackManagementDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TrackManagementDbContext>();
        optionsBuilder.UseSqlite("Data Source=trackmanagement.db");

        return new TrackManagementDbContext(optionsBuilder.Options);
    }
}
