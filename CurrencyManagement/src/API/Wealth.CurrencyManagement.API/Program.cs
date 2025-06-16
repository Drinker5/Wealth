using Wealth.CurrencyManagement.Application.Currency.Commands;
using Wealth.CurrencyManagement.Domain.Currency;
using Wealth.CurrencyManagement.Infrastructure;
using Wealth.CurrencyManagement.Infrastructure.Json;
using Wealth.CurrencyManagement.Infrastructure.Mediation;
using Wealth.CurrencyManagement.Infrastructure.RequestProcessing;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddLogging(o => o.AddConsole());

builder.Services.RegisterRequestProcessingModule();
builder.Services.RegisterUnitOfWorkModule();
builder.Services.RegisterNewtonsoftJsonModule();
builder.Services.RegisterMediatorModule();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.MapGet("/", () => "hello");

app.MapPost("/", async (CqrsInvoker invoker) =>
{
    var currencyId = new CurrencyId("FOO");
    await invoker.CommandAsync(new CreateCurrencyCommand(currencyId, "Foo", "F"));
});

app.Run();