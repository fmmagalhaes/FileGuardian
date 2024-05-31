global using File = FileGuardian.Domain.Entities.File;
using FileGuardian.Application.Services;
using FileGuardian.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FileGuardian.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGroupService, GroupService>();

        return services;
    }
}
