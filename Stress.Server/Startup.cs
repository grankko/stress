using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stress.Game;
using Stress.Server.Hubs;
using Stress.Server.Middleware;
using Stress.Server.Services;
using System;

namespace Stress.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();
            services.AddHsts(options => { options.MaxAge = TimeSpan.FromDays(365); options.IncludeSubDomains = true; });
            services.AddSingleton<ISessionManagementService, SessionManagementService>();
            services.AddTransient<IGameplay, Gameplay>();
            services.AddTransient<IGameSessionService>(p =>
                new GameSessionService(p.GetService<ISessionManagementService>().GenerateNewSessionKey(), p.GetRequiredService<IGameplay>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHsts();
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    SecurityHeadersMiddleware.SetSecurityHeaders(ctx.Context.Response.HttpContext);
                }
            });
            app.UseHttpsRedirection();
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<StressHub>("/stresshub");
            });
        }
    }
}
