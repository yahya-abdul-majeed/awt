using ibex.Data;
using ibex.Services;

namespace ibex
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<DapperContext>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IWebsiteService, WebsiteService>();
            services.AddScoped<IBugService, BugService>();
            services.AddScoped<ISkeletonService, SkeletonService>();
            services.AddScoped<IToolService,ToolService>();
            return services;

        }
    }
}
