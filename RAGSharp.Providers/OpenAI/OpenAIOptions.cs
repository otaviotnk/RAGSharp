namespace RAGSharp.Providers.OpenAI
{
    public class OpenAIOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ChatModel { get; set; } = "gpt-4o-mini";
        public string EmbeddingModel { get; set; } = "text-embedding-3-small";
    }

}
