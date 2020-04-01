using System;
using Botron.Http.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Botron.Api
{
    public class ServiceConfiguration
    {
        public string ApplicationName => Configuration["AppInfo:Name"];

        public string ApplicationVersion => Configuration["AppInfo:Version"];

        public IConfiguration Configuration { get; }


        public ServiceConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSwaggerGen(swagger =>
            {
                swagger.DescribeAllParametersInCamelCase();
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = ApplicationName, Version = $"v{ApplicationVersion}" });
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddLogging();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            string logName = string.IsNullOrEmpty(ApplicationName) ? "BotronService" : ApplicationName;
            loggerFactory.AddFile($"Logs/{logName.Replace(" ", "-")}-{{Date}}.log");

            if (env.IsDevelopment())
            {
                //Botron.Api.Logger.InitializeDevelopment(ApplicationName.Replace(" ", "-"));
                app.UseCors(DevelopmentCorsPolicy);
            }
            else
            {
                //Botron.Api.Logger.InitializeProduction(ApplicationName.Replace(" ", "-"));
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler(HandleGlobalException);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{ApplicationVersion}/swagger.json", $"{ApplicationName} v{ApplicationVersion}");
                c.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
            });
        }

        void DevelopmentCorsPolicy(CorsPolicyBuilder builder)
        {
            builder.SetIsOriginAllowed(origin =>
            {
                return new Uri(origin).Host == "localhost";
            })
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
        }

        void HandleGlobalException(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                IExceptionHandlerFeature exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionHandler != null)
                {
                    switch (exceptionHandler.Error)
                    {
                        case HttpException error:
                            context.Response.StatusCode = (int)error.StatusCode;
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(error.ToResponse()));
                            break;

                        default:
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ExceptionResponse
                            {
                                ["message"] = exceptionHandler.Error.Message
                            }));
                            break;
                    }

                    //logger.LogError($"Something went wrong: {contextFeature.Error}");
                }
            });
        }
    }
}
