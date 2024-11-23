Console.WriteLine("Hello, Semantic Kernel, Ollama and OpenAPI!");

var builder = Kernel.CreateBuilder();
builder.Services.AddOllamaChatCompletion("llama3.2:3b", new Uri("http://localhost:11434"));
var kernel = builder.Build();

// 👇🏼 Import as a plugin the service using the OpenAPI document
await kernel.ImportPluginFromOpenApiAsync(
    pluginName: "weatherforecast",
    // 👇🏼 URI pointing to the WebApi project OpenAPI document
    uri: new Uri("http://localhost:5118/openapi/v1.json")
);

var chatService = kernel.GetRequiredService<IChatCompletionService>();
var settings = new OllamaPromptExecutionSettings
{
    // 👇🏼 Allow automatic function calling
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

ChatHistory chat = new("You are an AI assistant that helps people find information.");
StringBuilder responseBuilder = new();

while (true)
{
    Console.Write("Question: ");
    chat.AddUserMessage(Console.ReadLine()!);

    responseBuilder.Clear();

    var messages = await chatService.GetChatMessageContentsAsync(chat, settings, kernel);

    foreach (var message in messages) Console.Write(message);
    Console.WriteLine();
    chat.AddAssistantMessage(responseBuilder.ToString());
    Console.WriteLine();
}