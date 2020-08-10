﻿using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace Chatroom.App.Infrastructure.Installers
{
    using Core.Options;
    using Contracts;
    using Configs;
    using Handlers;
    using Services;
    using Core.Interfaces;

    internal class RegisterApiResources : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            var policyConfigs = new HttpClientPolicyConfiguration();
            config.Bind("HttpClientPolicies", policyConfigs);

            services.Configure<RabbitMqConfiguration>(config.GetSection("RabbitMq"));

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(policyConfigs.RetryTimeoutInSeconds));

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(policyConfigs.RetryCount, _ => TimeSpan.FromMilliseconds(policyConfigs.RetryDelayInMs));

            var circuitBreakerPolicy = HttpPolicyExtensions
               .HandleTransientHttpError()
               .CircuitBreakerAsync(policyConfigs.MaxAttemptBeforeBreak, TimeSpan.FromSeconds(policyConfigs.BreakDurationInSeconds));

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            //Register custom Bearer Token Handler. The DelegatingHandler has to be registered as a Transient Service
            services.AddTransient<ProtectedApiBearerTokenHandler>();

            //Register a Typed Instance of HttpClientFactory for a Protected Resource
            //More info see: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.0

            services.AddHttpClient<IBotService, BotService>(client =>
                {
                    client.BaseAddress = new Uri(config["ApiResourceBaseUrls:BotApi"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContentMediaTypes.JSON));
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(policyConfigs.HandlerTimeoutInMinutes))
                .AddHttpMessageHandler<ProtectedApiBearerTokenHandler>()
                .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOpPolicy)
                .AddPolicyHandler(timeoutPolicy)
                .AddPolicyHandler(circuitBreakerPolicy);

            //Register a Typed Instance of HttpClientFactory for AuthService
            services.AddHttpClient<IAuthServerConnect, AuthServerConnect>();

            services.AddSingleton<IMessageProcessorService, MessageProcessorService>();

            // Register the DiscoveryCache in DI and will use the HttpClientFactory to create clients. Cached for 24hrs by default
            services.AddSingleton<IDiscoveryCache>(r =>
            {
                var factory = r.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(config["ApiResourceBaseUrls:AuthServer"], () => factory.CreateClient());
            });

            services.AddSingleton<IRabbitMQService, RabbitMQService>();
        }
    }
}