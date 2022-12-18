// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace ApiHub.Server.Extensions;

public static class MiddlewareExtension
{
    public static void UseAllowCors(this IApplicationBuilder app)
    {
        app.UseCors(options =>
        {
            options.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    }
}
