using Microsoft.SemanticKernel;
using System.Text;
using Microsoft.SemanticKernel.ChatCompletion;

// Initialize the Semantic kernel
var kernelBuilder = Kernel.CreateBuilder();
var kernel = kernelBuilder
    .AddOpenAIChatCompletion(                        // We use Semantic Kernel OpenAI API
        modelId: "phi3",
        apiKey: null,
        endpoint: new Uri("http://localhost:11434")) // With Ollama OpenAI API endpoint
    .Build();

// Create a new chat
var ai = kernel.GetRequiredService<IChatCompletionService>();
ChatHistory chat = new("You are an AI assistant that helps people find information.");
StringBuilder builder = new();

// User question & answer loop
while (true)
{
    Console.Write("Question: ");
    chat.AddUserMessage(Console.ReadLine()!);

    builder.Clear();

    // Get the AI response streamed back to the console
    await foreach (var message in ai.GetStreamingChatMessageContentsAsync(chat, kernel: kernel))
    {
        Console.Write(message);
        builder.Append(message.Content);
    }
    Console.WriteLine();
    chat.AddAssistantMessage(builder.ToString());

    Console.WriteLine();
}