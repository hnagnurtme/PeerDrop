using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeerDrop.Shared.Configurations;

namespace PeerDrop.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Configurations
        services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));

        return services;
    }
}

