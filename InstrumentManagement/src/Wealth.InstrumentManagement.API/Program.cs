using Wealth.InstrumentManagement.API.Extensions;
using Wealth.InstrumentManagement.API.Services;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.AddApplicationServices();
builder.Services.ConfigureHttpClientDefaults(http =>
{
    // Turn on resilience by default
    http.AddStandardResilienceHandler();

    // Turn on service discovery by default
    http.AddServiceDiscovery();
});

builder.Services.AddHttpClient<ICurrencyService, CurrencyService>(client =>
{
    client.BaseAddress = new("http://currency");
});
builder.Services.AddServiceDiscovery();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<InstrumentsServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


app.Run();