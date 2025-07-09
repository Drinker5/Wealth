using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.API;
using Wealth.PortfolioManagement.API.APIs;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddServiceModules();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapPortfolioApi();
app.Run();