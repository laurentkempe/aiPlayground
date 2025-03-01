var builder = Kernel.CreateBuilder();
// 👇🏼 Using llama3.2 with Ollama
builder.AddOllamaChatCompletion("llama3.2:3b", new Uri("http://localhost:11434"));

var kernel = builder.Build();

ChatCompletionAgent agent = new() // 👈🏼 Definition of the agent
{
    Instructions = """
                   Answer questions about different locations.
                   For France, use the time format: HH:MM. HH goes from 00 to 23 hours, MM goes from 00 to 59 minutes.
                   """,
    Name = "Location Agent",
    Kernel = kernel,
    // 👇🏼 Allows the model to decide whether to call the function
    Arguments = new KernelArguments(new PromptExecutionSettings 
        { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() })
};

// 👇🏼 Define a plugin time tool
var plugin =
    KernelPluginFactory.CreateFromFunctions("Time",
                                    "Get the current time for a city",
                                    [KernelFunctionFactory.CreateFromMethod(GetCurrentTime)]);
agent.Kernel.Plugins.Add(plugin);

ChatHistory chat =
[
    new ChatMessageContent(AuthorRole.User, "What time is it in Illzach, France?")
];

await foreach (var response in agent.InvokeAsync(chat))
{
    chat.Add(response);
    Console.WriteLine(response.Content);
}

// 👇🏼 Define a time tool
[Description("Get the current time for a city")]
string GetCurrentTime(string city) => $"It is {DateTime.Now.Hour}:{DateTime.Now.Minute} in {city}.";