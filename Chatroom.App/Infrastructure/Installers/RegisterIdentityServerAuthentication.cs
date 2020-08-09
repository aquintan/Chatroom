using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Chatroom.App.Infrastructure.Installers
{
    using Core.Interfaces;

    internal class RegisterIdentityServerAuthentication : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = config["ApiResourceBaseUrls:AuthServer"];
                    options.RequireHttpsMetadata = false;

                    options.ClientId = config["IdentityClient:ClientId"];
                    options.ClientSecret = config["IdentityClient:ClientSecret"];
                    //options.ResponseType = config["IdentityClient:ResponseType"];
                    options.ResponseType = "code";
                    options.UsePkce = true;
                    options.SignedOutRedirectUri = config["IdentityClient:SignedOutRedirectUri"];
                    options.SignedOutCallbackPath = config["IdentityClient:SignedOutCallbackPath"];

                    options.SaveTokens = true;

                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });
        }
    }
}