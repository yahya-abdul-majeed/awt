using lynx.Data;
using lynx.Options;
using lynx.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System.Text;

namespace lynx
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<RPCClient>();
            services.AddSingleton<DapperContext>();
            services.AddScoped<ISubmissionService, SubmissionService>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IContestService, ContestService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IAssignmentProvider, AssignmentProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAssignmentService,AssignmentService>();

            var ibex_ip = Environment.GetEnvironmentVariable("ASPNETCORE_IBEX_HOSTIP"); 

            services.AddRefitClient<IbexClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(ibex_ip));

            //services.AddRefitClient<IbexClient>()
            //    .ConfigureHttpClient(c => c.BaseAddress = new Uri(config["ibex:BaseAddress"]));

            return services;
        }

        public static IServiceCollection AddJWTSupport(this IServiceCollection services, IConfiguration config)
        {
            var Jwtoptions = config.GetSection("JwtOptions").Get<JwtOptions>();    
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = Jwtoptions.ValidateIssuer,
                        ValidateAudience = Jwtoptions.ValidateAudience,
                        ValidateLifetime = Jwtoptions.ValidateLifetime,
                        ValidateIssuerSigningKey = Jwtoptions.ValidateLifetime,
                        ValidIssuer = Jwtoptions.Issuer,
                        ValidAudience = Jwtoptions.Audience,
                        IssuerSigningKey = Jwtoptions.GetIssuerSigningKey() 
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RolePolicy", policy =>
                {
                    policy.RequireRole("Level-1");
                });
            });

            return services;
        }

        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOptions>(
                   config.GetSection(nameof(JwtOptions))
                );

            return services;
        }
    }
}
