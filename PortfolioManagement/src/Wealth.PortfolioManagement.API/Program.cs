using Wealth.BuildingBlocks.API;
using Wealth.PortfolioManagement.API.APIs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddServiceDiscovery();
builder.AddServiceModules();
builder.Configuration.AddJsonFile("appsettings.Secrets.json", optional: true);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapPortfolioApi();
app.Run();