using static System.Net.WebRequestMethods;

namespace ChatOllama.Shared.Domain.Models
{
    public class ChatSession
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public ICollection<Message> Mensagens { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public void UpdateTitle(string novoTitulo)
        {
            Titulo = novoTitulo;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
