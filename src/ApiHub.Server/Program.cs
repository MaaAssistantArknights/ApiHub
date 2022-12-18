// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using ApiHub.Server;
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = ApiVersion.Parse("1");
    o.AssumeDefaultVersionWhenUnspecified = true;
});
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});
builder.Services.AddAuthentication(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

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

app.UseRouting();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
