using Wealth.BuildingBlocks.API;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceModules();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.Run();