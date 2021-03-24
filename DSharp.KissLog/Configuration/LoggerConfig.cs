using KissLog;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Text;

namespace DSharp.KissLog.Configuration
{
    public static class LoggerConfig
    {
        public static void AddLoggerConfig(this IServiceCollection services)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILogger>((context) =>
            {
                return Logger.Factory.Get();
            });

            services.AddLogging(logging =>
            {
                logging.AddKissLog();
            });
        }

        public static void UseLoggerConfig(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseKissLogMiddleware(options =>
            {
                ConfigureKissLog(options,configuration);
            });
        }

        private static void ConfigureKissLog(IOptionsBuilder options, IConfiguration configuration)
        {
            options.Options
                .AppendExceptionDetails((Exception ex) =>
                {
                    StringBuilder sb = new StringBuilder();

                    if (ex is System.NullReferenceException nullRefException)
                    {
                        sb.AppendLine("Important: check for null references");
                    }

                    return sb.ToString();
                });

            // KissLog internal logs
            options.InternalLog = (message) =>
            {
                Debug.WriteLine(message);
            };

            // register logs output
            RegisterKissLogListeners(options,configuration);
        }

        private static void RegisterKissLogListeners(IOptionsBuilder options, IConfiguration configuration)
        {
            // multiple listeners can be registered using options.Listeners.Add() method
            options.Listeners.Add(new RequestLogsApiListener(new Application(
                configuration["KissLog.OrganizationId"],
                configuration["KissLog.ApplicationId"])
            )
            {
                ApiUrl = configuration["KissLog.ApiUrl"]
            });
        }
    }
}
