using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.BLL.Mapping;
using PeerDrop.BLL.Services;
using PeerDrop.BLL.Validators;

namespace PeerDrop.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        // Add Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHashService, BCryptPasswordHasher>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Add HttpContextAccessor (required for CurrentUserService)
        services.AddHttpContextAccessor();

        // Add AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
