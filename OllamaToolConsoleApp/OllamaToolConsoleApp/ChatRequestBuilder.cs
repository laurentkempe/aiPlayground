namespace OllamaToolConsoleApp;

public class ChatRequestBuilder
{
    private readonly ChatRequest _chatRequest = new();
    private readonly List<Message> _messages = new();
    private readonly List<Tool> _tools = new();

    public ChatRequestBuilder SetModel(string model)
    {
        _chatRequest.Model = model;
        
        return this;
    }

    public ChatRequestBuilder AddMessage(string content, string role)
    {
        _messages.Add(new Message
        {
            Content = content,
            Role = role
        });
        
        return this;
    }

    public ChatRequestBuilder AddFunctions(Functions functions)
    {
        functions.GetFunctionDetails().ForEach(function => AddTool(this, function));

        return this;
    }

    public ChatRequest Build()
    {
        _chatRequest.Messages = _messages;
        _chatRequest.Tools = _tools;

        return _chatRequest;
    }

    public static ChatRequestBuilder AddTool(ChatRequestBuilder builder, FunctionDetails functionDetails)
    {
        builder.AddTool(
            "function",
            functionDetails.Name,
            functionDetails.Description,
            "object",
            new Dictionary<string, Properties>
            {
                { "city", new Properties { Type = "string", Description = "The city to get the weather for" } }
            },
            ["city"]);

        return builder;
    }

    private ChatRequestBuilder AddTool(
        string type,
        string functionName,
        string functionDescription,
        string parameterType,
        Dictionary<string, Properties> properties,
        List<string> required)
    {
        _tools.Add(new Tool
        {
            Type = type,
            Function = new Function
            {
                Name = functionName,
                Description = functionDescription,
                Parameters = new Parameters
                {
                    Type = parameterType,
                    Properties = properties,
                    Required = required
                }
            }
        });
        
        return this;
    }
}