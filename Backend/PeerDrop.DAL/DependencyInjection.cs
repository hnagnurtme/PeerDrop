using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeerDrop.DAL.DbContexts;
using PeerDrop.DAL.Repositories;

namespace PeerDrop.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext (PostgreSQL) với MigrationsHistoryTable
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                o => o.MigrationsHistoryTable("__EFMigrationsHistory", "public")
            )
        );


        // Add Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
