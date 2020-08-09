using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace Chatroom.App.Infrastructure.Installers
{
    using Contracts;
    using HealthChecks;
    using Core.Interfaces;

    internal class RegisterHealthChecks : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            //Register HealthChecks and UI
            services.AddHealthChecks()
                .AddCheck("Google Ping", new PingHealthCheck("www.google.com", 100))
                .AddCheck("Bing Ping", new PingHealthCheck("www.bing.com", 100))
                .AddUrlGroup(new Uri(config["ApiResourceBaseUrls:AuthServer"]),
                    name: "Auth Server",
                    failureStatus: HealthStatus.Degraded)
                .AddUrlGroup(new Uri(config["ApiResourceBaseUrls:BotApi"]),
                    name: "Bot Api",
                    failureStatus: HealthStatus.Degraded);
        }
    }
}