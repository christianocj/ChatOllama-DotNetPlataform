using ChatOllama.Shared.Domain.Models;

namespace ChatOllama.Shared.Domain.Interfaces
{
    public interface ISessionRepository
    {
        Task<ChatSession?> GetByPublicIdAsync(Guid publicId);

        // Buscar o histórico todo de uma sessão
        Task<ChatSession?> GetSessionWithMessagesAsync(Guid publicId);

        // Lista para a barra lateral do teu Chat
        Task<IEnumerable<ChatSession>> GetAllRecentAsync();

        // Criar e Atualizar recebem a entidade pura
        Task AddAsync(ChatSession session);
        Task UpdateAsync(ChatSession session);

        // Apagar também usa o Guid
        Task DeleteAsync(Guid publicId);
    }
}
