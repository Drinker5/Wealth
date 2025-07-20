using Wealth.BuildingBlocks.API;
using Wealth.DepositManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.AddServiceModules();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DepositsServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
