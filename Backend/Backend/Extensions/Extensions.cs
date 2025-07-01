using Backend.Repositories;
using Backend.Services;

namespace Backend.Extensions;

public static class Extensions {
    public static IServiceCollection AddJobModule(this IServiceCollection services) {
        services.AddScoped<JobRepository>();
        services.AddScoped<JobService>();
        return services;
    }

    public static IServiceCollection AddAuthModule(this IServiceCollection services) {
        services.AddScoped<AuthService>();
        services.AddScoped<AuthRepository>();
        return services;
    }
    
    public static IServiceCollection AddUserModule(this IServiceCollection services) {
        services.AddScoped<UserRepository>();
        return services;
    }
}