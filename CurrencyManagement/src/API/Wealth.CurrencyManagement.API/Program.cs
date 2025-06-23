using Serilog;
using Serilog.Events;
using Wealth.CurrencyManagement.Infrastructure.Abstractions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

var logConfig = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration);

builder.Services.AddHttpClient();
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