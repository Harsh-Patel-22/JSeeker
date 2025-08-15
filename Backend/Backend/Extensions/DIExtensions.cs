using Backend.Repositories;
using Backend.Services;
using Backend.Services.Query;

namespace Backend.Extensions;

public static class DIExtensions {
    public static IServiceCollection AddJobModule(this IServiceCollection services) {
        services.AddScoped<JobRepository>();
        services.AddScoped<InterviewRepository>();
        services.AddScoped<ApplicationRepository>();
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
        services.AddScoped<AddressRepository>();
        services.AddScoped<HirerService>();
        services.AddScoped<UserService>();
        services.AddScoped<ProjectsRepository>();
        return services;
    }

    public static IServiceCollection AddMetricsModule(this IServiceCollection services) {
        services.AddScoped<MetricsQueryService>();
        return services;
    }
    
    public static IServiceCollection AddHttpClients(this IServiceCollection services) {
        services.AddHttpClient<GithubService>();
        services.AddHttpClient<AIService>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services) {
        services.AddScoped<ResumeBuilderService>();
        services.AddScoped<PdfService>();
        services.AddScoped<RatingService>();
        services.AddScoped<ValidationService>();
        return services;
    }

    public static IServiceCollection AddQueryServices(this IServiceCollection services) {
        services.AddScoped<JobsAggregateQueryService>();
        services.AddScoped<MetricsQueryService>();
        return services;
    }
}