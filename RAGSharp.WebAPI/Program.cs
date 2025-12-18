using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Extensions;
using RAGSharp.Providers.Mock;
using RAGSharp.Providers.OpenAI;
using RAGSharp.Storage.InMemory;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Core
services.AddRagSharpCore();

// Providers
services.AddSingleton<IEmbedder, MockEmbedder>();
services.AddSingleton<ILLMClient, MockLLMClient>();
services.AddSingleton<IVectorStore, InMemoryVectorStore>();

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

builder.Services.AddOpenAI(options =>
{
    options.ApiKey = builder.Configuration["OpenAI:ApiKey"]!;
    options.ChatModel = builder.Configuration["OpenAI:ChatModel"]!;
    options.EmbeddingModel = builder.Configuration["OpenAI:EmbeddingModel"]!;
});


var app = builder.Build();
app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RAGSharp API");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();
