using System;
using System.Linq;
using Chatroom.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chatroom.Core
{
    public static class ServiceRegistrationExtension
    {
        public static void AddServicesInAssembly<T>(this IServiceCollection services, IConfiguration configuration)
        {
            var appServices = typeof(T).Assembly.DefinedTypes
                .Where(x => typeof(IServiceRegistration)
                                .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IServiceRegistration>().ToList();

            appServices.ForEach(svc => svc.RegisterAppServices(services, configuration));
        }
    }
}
