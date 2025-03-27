# AI playground

Some experiments around AI to learn.

## Projects

- [MCPServerDocker](https://github.com/laurentkempe/aiPlayground/tree/main/MCPServerDocker) Two simple console applications using `modelcontextprotocol / csharp-sdk`, one .NET C# MCP Server and its client, and a way to publish the server as a Docker image.
- [MCPServer](https://github.com/laurentkempe/aiPlayground/tree/main/MCPServer) Two simple console applications using modelcontextprotocol / csharp-sdk, Microsoft.Extensions.AI with Ollama & tools to build a Model Context Protocol (MCP) server and client!
- [OllamaMCPServerMicrosoftExtensions](https://github.com/laurentkempe/aiPlayground/tree/main/OllamaMCPServerMicrosoftExtensions) simple console application using Microsoft.Extensions.AI with Ollama & tools from a Model Context Protocol (MCP) server!
- [SKOllamaAgentWithFunction](https://github.com/laurentkempe/aiPlayground/tree/main/SKOllamaAgentWithFunction) demonstrates the way to create a Semantic Kernel Agent with function in C# using Ollama to run local SLM model like llama3.2.
- [SKOllamaAgent](https://github.com/laurentkempe/aiPlayground/tree/main/SKOllamaAgent) demonstrates the way to create a Semantic Kernel Agent in C# using Ollama to run local SLM model like phi4.
- [DeepSeekOllamaAspire](https://github.com/laurentkempe/aiPlayground/tree/main/DeepSeekOllamaAspire) explore how to run DeepSeek-R1 distilled model by harnessing the capabilities of .NET Aspire alongside Ollama on your local environment.
- [OllamaToolCallingMicrosoftExtensions](https://github.com/laurentkempe/aiPlayground/tree/main/OllamaToolCallingMicrosoftExtensions) a discovery project to learn how AI tool/function calling are working using Microsoft.Extensions.AI. Follows my own implementation [OllamaToolConsoleApp](https://github.com/laurentkempe/aiPlayground/tree/main/OllamaToolConsoleApp).
- [OllamaToolCallingOpenAPI](https://github.com/laurentkempe/aiPlayground/tree/main/OllamaToolCallingOpenAPI) a console application using Semantic Kernel and its OpenAPI plugin to call an ASP.NET Core minimal API, using the new .NET 9 OpenAPI support, all on your local machine with Ollama.
- [OllamaToolConsoleApp](https://github.com/laurentkempe/aiPlayground/tree/main/OllamaToolConsoleApp) a discovery project to learn how AI tool/function calling are working. A C# source generator is used to annotate a C# method which can then be called by Ollama using its tool support.
- [Phi3SKConsoleApp](https://github.com/laurentkempe/aiPlayground/tree/main/Phi3SKConsoleApp) a console application that interacts with Phi-3 on your machine using Ollama, C# and Semantic Kernel.
- [Phi3SKWebApp](https://github.com/laurentkempe/aiPlayground/tree/main/Phi3SKWebApp) A blazor web application leveraging Fluent UI Blazor components library that interacts with Phi-3 on your machine using Ollama, C# and Semantic Kernel. It demonstrates the streaming of chat completion from Phi-3 to the web application in real-time.

## Other projects

From my [.NET Aspire playground](https://github.com/laurentkempe/aspirePlayground)

- [LocalLLMSummarize](https://github.com/laurentkempe/aspirePlayground/tree/main/LocalLLMSummarize) a simple web application which get a saved article from omnivore.app and summarize the articles using a local LLM and display the summary to the user.
- [OpenAI](https://github.com/laurentkempe/aspirePlayground/tree/main/OpenAI) a simple web application which get a saved article from omnivore.app and summarize the articles using Azure OpenAI and display the summary to the user.
