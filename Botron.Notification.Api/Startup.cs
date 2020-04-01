using System.Collections.Generic;
using Botron.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Botron.Notification.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        ServiceConfiguration ServiceConfiguration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ServiceConfiguration = new ServiceConfiguration(configuration);

            //Mapper.Configure(mapper =>
            //{
            //    mapper.MapType<UserProfileRequest, UserProfileModel>();
            //});
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ServiceConfiguration.ConfigureServices(services);
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            }).AddMessagePackProtocol();
            //services.AddHostedService<NotificationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ServiceConfiguration.Configure(app, env, loggerFactory);

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/Notification");
            });

        }

    }
}
