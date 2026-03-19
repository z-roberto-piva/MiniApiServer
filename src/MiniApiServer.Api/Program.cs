using Serilog;
using MiniApiServer.Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MiniApiServer.Api",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniApiServer.Api v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
