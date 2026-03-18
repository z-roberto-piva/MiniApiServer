using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniApiServer.Worker;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services));

builder.Services.AddHostedService<WorkerBootstrapService>();

await builder.Build().RunAsync();
