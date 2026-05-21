using ChatOllama.Api.Infrastructure.Data;
using ChatOllama.Shared.Domain.Interfaces;
using ChatOllama.Shared.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatOllama.Api.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly EfCoreDbContext _dbContext;
        public MessageRepository(EfCoreDbContext cont, EfCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Message message)
        {
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesBySessionAsync(int sessionId)
        {
            return await _dbContext.Messages
                .AsNoTracking().Where(m => m.ChatSessionId == sessionId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}
