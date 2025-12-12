using Microsoft.Extensions.DependencyInjection;
using RAGSharp.Core.Abstractions;
using RAGSharp.Core.Chunking;
using RAGSharp.Core.Pipelines;

namespace RAGSharp.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRagSharpCore(this IServiceCollection services)
    {
        // Chunker padrão
        services.AddSingleton<IChunker, SimpleChunker>();

        // Pipeline principal
        services.AddSingleton<IRagPipeline, SimpleRagPipeline>();

        return services;
    }
}
