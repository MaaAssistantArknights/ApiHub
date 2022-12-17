// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using ApiHub.Api;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Debugging;

var configuration = Initializer.GetConfiguration();
var logger = Initializer.GetLogger(configuration);

Log.Logger = logger;
SelfLog.Enable(Console.Error);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(configuration);

builder.Host.UseSerilog();

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
});
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});

var app = builder.Build();

app.UseForwardedHeaders();
app.UseCors(options =>
{
    options.SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
