
using ChatOllama.Shared.Domain.Enums;

namespace ChatOllama.Shared.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();
        public int ChatSessionId { get; set; }
        public MessageRole Role { get; set; }
        public string? ModelName { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
