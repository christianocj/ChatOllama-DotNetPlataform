namespace ChatOllama.Api.Models;

/// <summary>
/// Representa uma requisição de chat enviada para a API
/// </summary>
public class ChatRequest
{
    /// <summary>
    /// O prompt/mensagem do usuário
    /// </summary>
    public string Prompt { get; set; } = string.Empty;
}

/// <summary>
/// Representa uma resposta de chat retornada pela API
/// </summary>
public class ChatResponse
{
    /// <summary>
    /// A resposta gerada pelo modelo
    /// </summary>
    public string Response { get; set; } = string.Empty;
}