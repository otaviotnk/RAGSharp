namespace RAGSharp.WebAPI.Models
{
    public class IndexRequest
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Source { get; set; }
        public string Content { get; set; } = "";
    }
}