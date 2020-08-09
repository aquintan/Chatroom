using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chatroom.Bot.Infrastructure.Installers
{
    using Core.Interfaces;
    using Contracts;
    using Services;

    public class RegisterApiResources : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IStockService, StockService>(client =>
            {
                client.BaseAddress = new Uri(config["StockApiInfo:Url"]);
                client.DefaultRequestHeaders.Accept.Clear();
            });
        }
    }
}