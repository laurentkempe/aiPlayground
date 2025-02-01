using System.ComponentModel;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// 👇🏼 Add the endpoint to your app to serve the OpenAPI document
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Glacial", "Revigorant", "Frais", "Froid", "Doux", "Chaud", "Doux", "Très chaud", "Caniculaire", "Brûlant"
};

app.MapGet("/weatherforecast", (ILogger<Program> logger) =>
    {
        logger.LogInformation("Weather forecast endpoint was called.");
        
        var forecast = Enumerable.Range(0, 4).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-15, 40),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        
        var forecastJson = JsonSerializer.Serialize(forecast, new JsonSerializerOptions { WriteIndented = true });
        logger.LogInformation("Forecast Data: {Forecast}", forecastJson);
        
        return forecast;
    })
    .WithName("GetWeatherForecast")
    // 👇🏼 Add some more description to OpenAPI, helping the SLM to determine which function to call  
    .WithDescription("API to retrieve the weather forecast for Illzach, France.")
    .WithSummary("Get weather forecast for Illzach, France.")
    .Produces<IEnumerable<WeatherForecast>>();

app.Run();

internal record WeatherForecast(
    // 👇🏼 Add some property description so that the OpenAPI spec is more descriptive for the LLM  
    [property: Description("Date for the weather forecast.")]DateOnly Date, 
    [property: Description("Temperature in celcius for the weather forecast.")]int TemperatureC,
    [property: Description("Word summarizing the weather forecast for the date.")]string? Summary);