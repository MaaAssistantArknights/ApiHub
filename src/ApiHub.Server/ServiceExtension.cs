// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using ApiHub.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace ApiHub.Server;

public static class ServiceExtension
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(o =>
        {
            o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
            .AddCookie()
            .AddOpenIdConnect(c =>
            {
                var configSection = configuration.GetSection("Auth:OIDC:Config");

                var serverAddress = configSection
                    .GetValue("ServerAddress", string.Empty).NotNull();
                var metadataAddress = configSection
                    .GetValue("MetadataAddress", string.Empty).NotNull();
                        
                var clientId = configSection
                    .GetValue("ClientId", string.Empty).NotNull();
                var clientSecret = configSection
                    .GetValue("ClientSecret", string.Empty).NotNull();
                        
                var scope = configSection.GetValue<string>("Scope")?
                                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            ?? new [] { "profile", "email", "openid" };
                
                var roleClaim = configSection.GetValue("RolesClaim", "roles").NotNull();
                var usernameClaim = configSection.GetValue("UsernameClaim", "preferred_username").NotNull();
                
                c.Scope.Clear();
                foreach (var s in scope)
                {
                    c.Scope.Add(s);
                }
                
                c.Authority = serverAddress;
                c.MetadataAddress = metadataAddress;
                c.ClientId = clientId;
                c.ClientSecret = clientSecret;
                c.CallbackPath = "/oidc-callback";
                c.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                c.GetClaimsFromUserInfoEndpoint = true;
                c.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = roleClaim,
                    NameClaimType = usernameClaim
                };
            });
    }
}
