var builder = Kernel.CreateBuilder();
// 👇🏼 Using Phi-4 with Ollama
builder.AddOllamaChatCompletion("phi4:latest", new Uri("http://localhost:11434"));

var kernel = builder.Build();

ChatCompletionAgent agent = new() // 👈🏼 Definition of the agent
{
    Instructions = "Answer questions about C# and .NET",
    Name = "C# Agent",
    Kernel = kernel
};

ChatHistory chat =
[
    new ChatMessageContent(AuthorRole.User, "What is the difference between a class and a record?")
];

await foreach (var response in agent.InvokeAsync(chat))
{
    chat.Add(response);
    Console.WriteLine(response.Content);
}