const string userWeatherrMessage = "What is the weather in Illzach, France?";
const string userTimeMessage = "What time is it in Illzach, France?";

// Randomly choose between the two messages
var random = new Random();
var userMessage = random.Next(0, 100) > 50 ? userWeatherrMessage : userTimeMessage;

// 👇🏼 Building the request for OllamaSharp
var functions = new Functions();
var request = new ChatRequestBuilder()
    .SetModel("llama3.2:3b")
    .AddMessage(userMessage, "user")
    .AddFunctions(functions)
    .Build();

var jsonBuilder = new StringBuilder();

// 👇🏼 Calling OllamaSharp Chat and getting the response streamed back
var ollamaApiClient = new OllamaApiClient(new Uri("http://localhost:11434"));
await foreach (var responseStream in ollamaApiClient.Chat(request))
{
    jsonBuilder.Append(responseStream?.Message.Content);
}
Console.WriteLine($"User message: {userMessage}");
var json = jsonBuilder.ToString();
Console.WriteLine($"Received from SLM: {json}");
var result = functions.Execute(JsonSerializer.Deserialize<FunctionDetails>(json));
Console.WriteLine($"Function calling result: \"{result}\"");

// 👇🏼 Partial class annotated with the [Function] attribute used to generate functions.
public sealed partial class Functions
{
    [Function("Get the current time for a city")]
    string GetCurrentTime(string city) => $"It is {DateTime.Now.Hour}:{DateTime.Now.Minute} in {city}.";

    [Function("Get the current weather for a city")]
    string GetCurrentWeather(string city) => "The weather in " + city + " is sunny.";
}