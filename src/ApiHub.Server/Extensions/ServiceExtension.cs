// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiHub.Server.Extensions;

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

                var scope = configSection.GetValue<string>("Scope")?
                                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            ?? new [] { "profile", "email", "openid" };
                
                c.Scope.Clear();
                foreach (var s in scope)
                {
                    c.Scope.Add(s);
                }
                
                c.CallbackPath = "/oidc-callback";
                c.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                c.GetClaimsFromUserInfoEndpoint = true;
                c.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = configSection.GetValue("RolesClaim", "roles"),
                    NameClaimType = configSection.GetValue("UsernameClaim", "preferred_username")
                };
                
                c.MetadataAddress = configSection.GetValue<string>("MetadataAddress");
                c.ClientId = configSection.GetValue<string>("ClientId");
                c.ClientSecret = configSection.GetValue<string>("ClientSecret");
            });
    }

    public static void AddApiVersion(this IServiceCollection services)
    {
        services.AddApiVersioning(o =>
        {
            o.DefaultApiVersion = ApiVersion.Parse("1");
            o.AssumeDefaultVersionWhenUnspecified = true;
        });
    }
    
    public static void AddForwardedHeaders(this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
        });
    }
}
