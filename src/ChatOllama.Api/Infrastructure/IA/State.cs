using Microsoft.Extensions.AI;
using System.Text.Json.Serialization;

namespace ChatOllama.Api.Infrastructure.IA
{
    public sealed class State
    {
        [JsonPropertyName("messages")]
        public List<ChatMessage> Messages { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
