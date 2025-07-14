using Wealth.BuildingBlocks.API;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceModules();

// builder.Services.AddHttpClient<ICurrencyService, CurrencyService>(client =>
// {
//     client.BaseAddress = new("http://instrument");
// });
builder.Services.AddServiceDiscovery();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.Run();