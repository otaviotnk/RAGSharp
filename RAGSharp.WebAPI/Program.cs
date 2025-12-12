using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Extensions;
using RAGSharp.Providers.Mock;
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
