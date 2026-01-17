using Serilog;
using Wealth.BuildingBlocks.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var logConfig = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration);

builder.Services.AddHttpClient();
builder.Services.AddLogging(o => o.ClearProviders().AddSerilog(logConfig.CreateLogger(), dispose: true));
builder.AddServiceModules();
builder.Services.AddControllers();
builder.Services.AddServiceDiscovery();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();