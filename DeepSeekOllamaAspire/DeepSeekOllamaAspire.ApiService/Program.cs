using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// 👇🏼 IOC registration of OllamaSharp client as IChatClient
builder.AddOllamaSharpChatClient("chat");

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 👇🏼 Web API getting IChatClient from IOC
app.MapGet("/chat", async (IChatClient chatClient, string question) =>
{
    // 👇🏼 Calling Ollama API to get an answer from DeepSeek-R1
    var response = await chatClient.CompleteAsync(question);
    return new Response(response.Message.Text ?? "Failed to generate response.");
})
.WithName("Chat");

app.MapDefaultEndpoints();

app.Run();

public record Response(string Value);