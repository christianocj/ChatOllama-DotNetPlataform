namespace ChatOllama.Web.Models
{
    public class ChatSession
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public List<ChatMessageModel> Messages { get; set; } = new();
        public DateTime LastMessageDate { get; set; }
    }
}
