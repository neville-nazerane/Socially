using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using Socially.Server.DataAccess;
using Socially.Server.Managers.Utils;
using Socially.WebAPI.Services;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.Sources.Clear();
            configBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"authOptions:secret", "ahyujnhgctufihopktgjuerwfdmklsfjerdf43ew435teygih68iuvyguhoijnklhjlskdfdkjaflsdkj" },
                {"authOptions:audiences", "tester" },
                {"authOptions:issuer", "integration_tests" },
                { "sendGridApiKey", "IamNotNull" },
                { "signalR", "signalTests" }
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove the app's ApplicationDbContext registration.
            services.RemoveService<DbContextOptions<ApplicationDbContext>>()
                    .RemoveService<ApplicationDbContext>();

            // Add ApplicationDbContext using an in-memory database for testing.
            string dbname = Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(dbname));

            services.ReplaceServiceWithMock<ISendGridClient>()
                    .ReplaceServiceWithMock<IBlobAccess>()
                    .ReplaceServiceWithMock<ITableAccess>()
                    .ReplaceServiceWithMock<ISignalRPublisher>()
                    .ReplaceServiceWithMock<ICurrentContextProvider>();

            // Set context as singleton
            //services.RemoveService<CurrentContext>()
            //        .AddSingleton<CurrentContext>();

        });
    }

}