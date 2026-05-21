using ChatOllama.Api.Infrastructure.Data;
using ChatOllama.Shared.Domain.Interfaces;
using ChatOllama.Shared.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatOllama.Api.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly EfCoreDbContext _dbContext;

        public SessionRepository(EfCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ChatSession session)
        {
            _dbContext.ChatSessions.Add(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid publicId)
        {
            var session = _dbContext.ChatSessions.FirstOrDefault(
                x => x.PublicId == publicId);
           _dbContext.ChatSessions.Remove(session);
        }

        public async Task<IEnumerable<ChatSession>> GetAllRecentAsync()
        {
            return await _dbContext.ChatSessions
                .AsNoTracking()
                .OrderByDescending(m => m.UpdatedAt)
                .ToListAsync();
        }

        public async Task<ChatSession?> GetByPublicIdAsync(Guid publicId)
        {
            return _dbContext.ChatSessions.FirstOrDefault(
                x => x.PublicId == publicId);
        }

        public Task<ChatSession?> GetSessionWithMessagesAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ChatSession session)
        {
            _dbContext.ChatSessions.Update(session);
            _dbContext.SaveChanges();
        }
    }
}
