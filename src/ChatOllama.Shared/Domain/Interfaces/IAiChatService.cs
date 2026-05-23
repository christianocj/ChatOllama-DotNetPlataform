using ChatOllama.Shared.Domain.Models;

namespace ChatOllama.Shared.Domain.Interfaces;

public interface IAiChatService
{
    /// <summary>
    /// Envia um prompt e recebe a resposta completa de uma só vez
    /// </summary>
    Task<string> SendMessageAsync(Guid sessionPublicId, string prompt, string modelName, CancellationToken cancellationToken = default);
    /// <summary>
    /// Envia um prompt e recebe a resposta completa de uma só vez.
    /// </summary>
    Task<string> SendMessageAsync(string prompt, string modelName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Envia um prompt e recebe a resposta token por token
    /// </summary>
    IAsyncEnumerable<string> StreamMessageAsync(ChatSession session, string prompt, string modelName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista os modelos disponíveis instalados no servidor do Ollama (ex: llama3, phi3).
    /// </summary>
    Task<IEnumerable<string>> GetAvailableModelsAsync();
}