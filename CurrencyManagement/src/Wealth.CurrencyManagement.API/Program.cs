using Serilog;
using Wealth.CurrencyManagement.API.Blazor;
using Wealth.CurrencyManagement.Infrastructure.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var logConfig = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration);

builder.Services.AddHttpClient();
builder.Services.AddLogging(o => o.ClearProviders().AddSerilog(logConfig.CreateLogger(), dispose: true));
builder.Services.AddServiceModules(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddServiceDiscovery();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();

app.Run();