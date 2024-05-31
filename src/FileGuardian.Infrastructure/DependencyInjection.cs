global using File = FileGuardian.Domain.Entities.File;
using FileGuardian.Application.Abstractions;
using FileGuardian.Infrastructure.Persistence;
using FileGuardian.Infrastructure.Persistence.JoinTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileGuardian.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FileGuardianDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("fileGuardian"));
        });

        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFileUserRepository, FileUserRepository>();
        services.AddScoped<IFileGroupRepository, FileGroupRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IGroupUserRepository, GroupUserRepository>();

        return services;
    }
}
