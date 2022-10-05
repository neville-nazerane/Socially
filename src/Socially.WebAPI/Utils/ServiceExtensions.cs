using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Socially.Server.Managers;
using Socially.Server.Services.Models;
using Socially.WebAPI.Services;
using Socially.Website.Services;
using System.Text;

namespace Socially.WebAPI.Utils
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            return services.AddTransient<IUserProfileManager, UserProfileManager>()
                            .AddTransient<IImageManager, ImageManager>()
                            .AddTransient<IFriendManager, FriendManager>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddTransient<IUserService, UserService>()
                            .AddTransient<IImagesService, ImagesService>()
                            .AddTransient<IFriendsService, FriendsService>()
                            .AddSingleton<ICurrentContextProvider, CurrentContextProvider>()
                            .AddScoped<CurrentContext>()
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
