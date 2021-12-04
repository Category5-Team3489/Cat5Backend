using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.

// FRICK YOU SSL

//https://enable-cors.org/server_aspnet.html

//app.UseHttpsRedirection();
app.Urls.Add("http://*:80");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/", async (ctx) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    ctx.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    await ctx.Response.WriteAsJsonAsync(forecast);
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}