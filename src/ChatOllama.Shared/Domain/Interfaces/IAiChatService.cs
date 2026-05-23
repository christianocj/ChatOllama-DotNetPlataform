namespace ChatOllama.Shared.Domain.Interfaces;

public interface IAiChatService
{
    /// <summary>
    /// Envia um prompt e recebe a resposta completa de uma só vez
    /// </summary>
    Task<string> SendMessageAsync(Guid sessionPublicId, string prompt, string modelName, CancellationToken cancellationToken = default);
    /// <summary>
    /// Envia um prompt e recebe a resposta completa de uma só vez. 
    /// Sem o parametro Guid sessionPublicId para conversas temporárias
    /// </summary>
    Task<string> SendMessageAsync(string prompt, string modelName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Envia um prompt e recebe a resposta Token por token. 
    /// </summary>
    IAsyncEnumerable<string> StreamMessageAsync(Guid sessionPublicId, string prompt, string modelName, CancellationToken cancellationToken = default);
    /// <summary>
    /// Envia um prompt e recebe a resposta token por token.
    /// Sem o parametro Guid sessionPublicId para conversas temporárias
    /// </summary>
    IAsyncEnumerable<string> StreamMessageAsync(string prompt, string modelName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista os modelos disponíveis instalados no servidor do Ollama (ex: llama3, phi3).
    /// </summary>
    Task<IEnumerable<string>> GetAvailableModelsAsync();
}