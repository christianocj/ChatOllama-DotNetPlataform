using ChatOllama.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

public static class EfCore
{
    public static void AddEfCore(
        this IServiceCollection services,
        [CallerFilePath] string caminhoFicheiroChamador = "")
    {
        // Pega o diretório de quem chamou o método .AddEfCore()
        string diretorioChamador = Path.GetDirectoryName(caminhoFicheiroChamador);

        string connectionString = Path.Combine(diretorioChamador, "banco.db");

        services.AddDbContext<EfCoreDbContext>(options =>
            options.UseSqlite($"Data Source={connectionString}"));
    }
}
