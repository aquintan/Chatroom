using System;
using Chatroom.Core.Messaging;
using Chatroom.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chatroom.Bot.Infrastructure.Installers
{
    using Core.Interfaces;
    using Contracts;
    using Services;
    using System.Configuration;

    public class RegisterApiResources : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            services.Configure<RabbitMqConfiguration>(config.GetSection("RabbitMq"));

            services.AddHttpClient<IStockService, StockService>(client =>
            {
                client.BaseAddress = new Uri(config["StockApiInfo:Url"]);
                client.DefaultRequestHeaders.Accept.Clear();
            });

            services.AddTransient<IStockMessageSender, StockMessageSender>();
        }
    }
}