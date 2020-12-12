using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Socially.Core.Entities;
using Socially.Server.DataAccess;
using Socially.Server.Managers;
using Socially.Server.Services;
using Socially.WebAPI.Endpoints;
using Socially.WebAPI.EndpointUtils;

namespace Socially.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment Env)
        {
            Configuration = configuration;
            this.Env = Env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(c => c.UseSqlServer(Configuration.GetConnectionString("db")));
            services.AddIdentity<User, UserRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddHealthChecks()
                    .AddDbContextCheck<ApplicationDbContext>();
            services.AddControllers();
            
            services.AddSwaggerDocument();

            // managers
            services.AddTransient<IUserVerificationManager, UserVerificationManager>();
            services.AddTransient<IUserService, UserService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            if (Env.IsDevelopment())
            {
                app.UseOpenApi();
            }

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapCustom<AccountEndpoints>();

                endpoints.MapGet("/", c => c.Response.WriteAsync("Hello to the social world"));

                endpoints.MapHealthChecks("/health");

                endpoints.MapControllers();
            });
        }
    }
}
