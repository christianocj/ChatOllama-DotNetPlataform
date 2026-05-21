using ChatOllama.Shared.Domain.Models;

namespace ChatOllama.Shared.Domain.Interfaces
{
    public interface IMessageRepository
    {
        // Adiciona uma nova mensagem ao banco
        Task AddAsync(Message message);

        // Traz todas as mensagens de uma sessão específica
        Task<IEnumerable<Message>> GetMessagesBySessionAsync(int sessionId);
    }
}
