using ChatOllama.Api.Infrastructure.Data;
using ChatOllama.Shared.Domain.Interfaces;
using ChatOllama.Shared.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatOllama.Api.Infrastructure.Repositories
{
    public class UsuarioRepository : IRepository<Usuario>
    {
        private readonly EfCoreDbContext _dbContext;

        public UsuarioRepository(EfCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Usuario entity)
        {
            _dbContext.Usuario.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = _dbContext.Usuario.FirstOrDefault(u => u.Id == id);
            _dbContext.Usuario.Remove(usuario);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Usuario?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Usuario entity)
        {
            throw new NotImplementedException();
        }
    }
}
