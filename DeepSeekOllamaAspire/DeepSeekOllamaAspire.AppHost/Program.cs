var builder = DistributedApplication.CreateBuilder(args);

// 👇🏼 Configure Ollama server hosting integration
var ollama =
    builder.AddOllama("ollama") // 👈🏼 Add Ollama container to the app host
        .WithDataVolume() // 👈🏼 Adds a data volume to store models
        .WithGPUSupport() // 👈🏼 Use your beefy GPU 🎉
        .WithLifetime(ContainerLifetime.Persistent); // 👈🏼 Keep the container running when the app exits

// 👇🏼 Add DeepSeek-R1 model to Ollama server
var chat =
    ollama.AddModel("chat", "deepseek-r1:1.5b");

var apiService = 
    builder.AddProject<Projects.DeepSeekOllamaAspire_ApiService>("apiservice")
           // 👇🏼 Our Web API relies on Ollama chat and waits for it to start 
           .WithReference(chat)
           .WaitFor(chat);

builder.AddProject<Projects.DeepSeekOllamaAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
