using crow.services;

namespace crow;

public static class ServiceExtensions{
    public static IServiceCollection AddServices(this IServiceCollection services){
        services.AddSingleton<ISubmissionService,SubmissionService>();
        return services;
    }
}