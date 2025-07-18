using Wealth.BuildingBlocks.API;
using Wealth.InstrumentManagement.API.Extensions;
using Wealth.InstrumentManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.AddApplicationServices();
builder.AddServiceModules();
builder.Services.ConfigureHttpClientDefaults(http =>
{
    // Turn on resilience by default
    http.AddStandardResilienceHandler();

    // Turn on service discovery by default
    http.AddServiceDiscovery();
});

var app = builder.Build();
app.MapEndpoints();
app.Run();