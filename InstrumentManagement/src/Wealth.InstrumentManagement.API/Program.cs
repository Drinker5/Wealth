using Wealth.BuildingBlocks.API;
using Wealth.InstrumentManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceDiscovery();
builder.AddServiceModules();
builder.AddApplicationServices();
builder.Services.AddGrpc();
builder.Configuration.AddJsonFile("appsettings.Secrets.json", optional: true);

var app = builder.Build();

app.MapEndpoints();
app.Run();