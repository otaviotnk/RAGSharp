using Microsoft.Extensions.DependencyInjection;
using RAGSharp.Core.Abstractions;

namespace RAGSharp.Providers.Claude
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClaude(
            this IServiceCollection services,
            Action<ClaudeOptions> configure)
        {
            var options = new ClaudeOptions();
            configure(options);

            services.AddSingleton(options);

            services.AddHttpClient("claude-llm", client =>
            {
                client.DefaultRequestHeaders.Add("x-api-key", options.ApiKey);
                client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            });

            services.AddKeyedSingleton<ILLMClient>("claude", (sp, _) =>
                new ClaudeLLMClient(
                    sp.GetRequiredService<IHttpClientFactory>().CreateClient("claude-llm"),
                    options));

            services.AddSingleton<ILLMClient>(sp =>
                sp.GetRequiredKeyedService<ILLMClient>("claude"));

            return services;
        }
    }
}
