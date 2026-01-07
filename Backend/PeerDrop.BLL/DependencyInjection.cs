using CloudinaryDotNet;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.ExternalServices;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.BLL.Mapping;
using PeerDrop.BLL.Services;
using PeerDrop.BLL.Validators.Auth;
using PeerDrop.Shared.Constants;

namespace PeerDrop.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHashService, BCryptPasswordHasher>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IFileService, FileService>();
        

        // Add HttpContextAccessor (required for CurrentUserService)
        services.AddHttpContextAccessor();

        // Add AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        services.AddFluentValidationAutoValidation();
        
        // Add Cloudinary
        services.AddCloudStorage(configuration);
        
        // Add External Service
        services.AddScoped<IFileStorageService, CloudinaryFileStorageService>();


        return services;
    }

    private static IServiceCollection AddCloudStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var cloudinarySection = configuration.GetSection("Cloudinary");
        var cloudName = cloudinarySection["CloudName"];
        var apiKey = cloudinarySection["ApiKey"];
        var apiSecret = cloudinarySection["ApiSecret"];

        if (string.IsNullOrWhiteSpace(cloudName)
            || string.IsNullOrWhiteSpace(apiKey)
            || string.IsNullOrWhiteSpace(apiSecret))
        {
            throw new CloudStorageException(ErrorMessages.CloudStorageUnauthorized, ErrorCodes.CloudStorageUnauthorized);
        }

        var account = new Account(cloudName, apiKey, apiSecret);
        var cloudinary = new Cloudinary(account)
        {
            Api =
            {
                Secure = true
            }
        };

        services.AddSingleton(cloudinary);
        return services;
    }
}
