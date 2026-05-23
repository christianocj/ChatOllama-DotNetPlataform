using ChatOllama.Shared.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatOllama.Api.Infrastructure.Data
{
    public class EfCoreDbContext : DbContext
    {
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        public EfCoreDbContext(DbContextOptions<EfCoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do ChatSession
            modelBuilder.Entity<ChatSession>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.HasIndex(s => s.PublicIdd)
                      .IsUnique();

                entity.Property(s => s.Titulo)
                      .HasMaxLength(75)
                      .IsRequired();
            });

            // Configuração da Message
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.HasIndex(m => m.PublicId).IsUnique();

                entity.Property(m => m.Content)
                      .IsRequired();

                // Configura o Enum como String no Banco
                entity.Property(m => m.Role)
                      .HasConversion<string>();

                // Relacionamento 1 para Muitos
                entity.HasOne<ChatSession>()
                      .WithMany(s => s.Mensagens)
                      .HasForeignKey(m => m.ChatSessionId)
                      .OnDelete(DeleteBehavior.Cascade); // Apagar sessão apaga as mensagens
            });
        }
    }
}
