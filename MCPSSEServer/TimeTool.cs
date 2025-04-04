using System;
using System.ComponentModel;
using ModelContextProtocol.Server;

namespace MCPSSEServer;

[McpServerToolType]
public class TimeTool
{
    // 👇 Mark a method as an MCP tools
    [McpServerTool, Description("Get the current time for a city")]
    public static string GetCurrentTime(string city) => 
        $"It is {DateTime.Now.Hour:00}:{DateTime.Now.Minute:00} in {city}.";
}