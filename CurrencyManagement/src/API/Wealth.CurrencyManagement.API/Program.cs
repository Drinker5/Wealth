using Serilog;
using Serilog.Events;
using Wealth.CurrencyManagement.Application.Currencies.Commands;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Infrastructure;
using Wealth.CurrencyManagement.Infrastructure.Abstractions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

var logConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext();

const string outputTemplate = "[{Level:u3}] {Message:lj}{NewLine}{Exception}";
logConfig = logConfig.WriteTo.Console(outputTemplate: outputTemplate);

builder.Services.AddLogging(o => o.ClearProviders().AddSerilog(logConfig.CreateLogger(), dispose: true));
builder.Services.AddServiceModules(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();