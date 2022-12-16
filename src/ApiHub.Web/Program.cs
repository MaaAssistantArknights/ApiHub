// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ApiHub.Web;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<HttpClient>(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();

await builder.Build().RunAsync();
