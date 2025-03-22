Console.WriteLine("Hello, official MCP csharp-sdk and MCP Server!");

var message = "What is the current (CET) time in Illzach, France?";
Console.WriteLine(message);

// 👇🏼 Configure the MCP client options
McpClientOptions options = new()
{
    ClientInfo = new() { Name = "Time Client", Version = "1.0.0" }
};

// 👇🏼 Configure the Model Context Protocol server to use
var config = new McpServerConfig
{
    Id = "time",
    Name = "Time MCP Server",
    TransportType = TransportTypes.StdIo,
    TransportOptions = new Dictionary<string, string>
    {
        // 👇🏼 The command executed to start the MCP server
        ["command"] = @"..\..\..\..\MCPServer\bin\Debug\net9.0\MCPServer.exe"
    }
};

using var factory =
    LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));

// 👇🏼 Get an MCP session scope used to get the MCP tools
await using var mcpClient = 
    await McpClientFactory.CreateAsync(config, options, loggerFactory: factory);

// 👇🏼 Use Ollama as the chat client
var ollamaChatClient =
    new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.2:3b");
var client = new ChatClientBuilder(ollamaChatClient)
    // 👇🏼 Add logging to the chat client, wrapping the function invocation client 
    .UseLogging(factory)
    // 👇🏼 Add function invocation to the chat client, wrapping the Ollama client
    .UseFunctionInvocation()
    .Build();
    
IList<ChatMessage> messages =
[
    new(ChatRole.System, """
                         You are a helpful assistant delivering time in one sentence
                         in a short format, like 'It is 10:08 in Paris, France.'
                         """),
    new(ChatRole.User, message)
];

// 👇🏼 Pass the MCP tools to the chat client
var mcpTools = await mcpClient.GetAIFunctionsAsync();
var response =
    await client.GetResponseAsync(
        messages,
        new ChatOptions { Tools = [..mcpTools] });

Console.WriteLine(response);