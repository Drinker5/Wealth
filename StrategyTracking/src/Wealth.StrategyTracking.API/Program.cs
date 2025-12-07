using Wealth.BuildingBlocks.API;
using Wealth.StrategyTracking.API.APIs;
using Wealth.StrategyTracking.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceDiscovery();
builder.AddServiceModules();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapStrategiesApi();
app.MapStrategyApi();
app.MapGrpcServices();
app.Run();