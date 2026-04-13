namespace RAGSharp.Providers.Claude
{
    public class ClaudeOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ChatModel { get; set; } = "claude-3-5-sonnet-20241022";
        public int MaxTokens { get; set; } = 1024;
    }
}
