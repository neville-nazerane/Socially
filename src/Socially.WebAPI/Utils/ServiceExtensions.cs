using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Socially.Server.DataAccess;
using Socially.Server.Managers;
using Socially.Server.Managers.Utils;
using Socially.WebAPI.Services;
using Socially.Website.Models;
using Socially.Website.Services;
using System.Text;
using System.Threading.Tasks;

namespace Socially.WebAPI.Utils
{
    public static class ServiceExtensions
    {


        public static IServiceCollection AddAzSignalR(this IServiceCollection services,
                                                      string connString)
        {
            // condition to handle integration tests
            if (connString is not null)
                services.AddSignalR().AddAzureSignalR(connString);
            else
                services.AddSignalR();

            return services;
        }

        public static IServiceCollection AddSqlServerDbContext(this IServiceCollection services,
                                                               string connString)
        {
            services.AddDbContext<ApplicationDbContext>(c => c.UseSqlServer(connString));

            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services,
                                                     IConfiguration configuration)
        {
            services.AddSingleton(p =>
            {
                var template = new ConfigsSettings();
                configuration.Bind(template);
                return template;
            });

            return services;
        }

        public static IServiceCollection AddAzBlob(this IServiceCollection services,
                                                   string connString)
        {
            services.AddSingleton<IBlobAccess>(p => new BlobAccess(connString));

            return services;
        }

        public static IServiceCollection AddAzStorage(this IServiceCollection services,
                                                      string connString)
        {
            services.AddSingleton<ITableAccess>(p => new TableAccess(connString));

            return services;
        }


        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            return services.AddTransient<IUserProfileManager, UserProfileManager>()
                            .AddTransient<IImageManager, ImageManager>()
                            .AddTransient<IPostManager, PostManager>()
                            .AddTransient<ISignalRStateManager, SignalRStateManager>()
                            .AddTransient<IFriendManager, FriendManager>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddTransient<IUserService, UserService>()
                            .AddTransient<IImagesService, ImagesService>()
                            .AddSingleton<ICurrentContextProvider, CurrentContextProvider>()
                            .AddScoped<CurrentContext>()
                            .AddSingleton<ISignalRPublisher, SignalRPublisher>()
                            .AddScoped<InitializeService>();
        }
        
        public static void AddSocalAuthentication(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddAuthentication("complete")
                    .AddJwtBearerCompletely(o =>
                    {
                        var configs = configuration.GetRequiredSection("authOptions");
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = configs["issuer"],
                            ValidAudiences = configs["audiences"].Split(","),
                            RequireExpirationTime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configs["secret"]))
                        };

                        o.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["access_token"];

                                // If the request is for our hub...
                                var path = context.HttpContext.Request.Path;
                                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/"))
                                {
                                    // Read the token out of the query string
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };

                    });
                        services.AddAuthorization(o =>
                            o.DefaultPolicy = new AuthorizationPolicyBuilder()
                                                            .AddAuthenticationSchemes("complete")
                                                            .RequireAuthenticatedUser()
                                                            .Build()
                        );
        }

    }
}
