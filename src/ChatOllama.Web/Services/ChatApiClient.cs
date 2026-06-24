<<<<<<< Updated upstream
﻿namespace ChatOllama.Web.Services
{
    public class ChatApiClient
    {
        private readonly HttpClient _http;

        public ChatApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> SendAsync(string prompt)
        {
            return await _http.GetStringAsync(
                $"chat-temporary?prompt={Uri.EscapeDataString(prompt)}");
        }
=======
﻿namespace ChatOllama.Web.Services;

using ChatOllama.Api.Models;

/// <summary>
/// Cliente para comunicação com a API de chat
/// </summary>
public class ChatApiClient
{
    private readonly HttpClient _http;

    public ChatApiClient(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Envia uma mensagem para a API e retorna a resposta
    /// </summary>
    /// <param name="prompt">A mensagem do usuário</param>
    /// <returns>A resposta do assistente</returns>
    /// <exception cref="HttpRequestException">Quando ocorre um erro na comunicação com a API</exception>
    /// <exception cref="InvalidOperationException">Quando a resposta da API não contém o conteúdo esperado</exception>
    public async Task<string> SendAsync(string prompt)
    {
        // Cria o objeto de requisição
        var request = new ChatRequest { Prompt = prompt ?? throw new ArgumentNullException(nameof(prompt)) };

        // Envia a requisição POST com JSON
        var response = await _http.PostAsJsonAsync("chat-temporary", request);

        // Garante que o código de status seja bem-sucedido (lança exceção se não for)
        response.EnsureSuccessStatusCode();

        // Tenta ler a resposta como ChatResponse
        var chatResponse = await response.Content.ReadFromJsonAsync<ChatResponse>();

        // Valida que recebemos uma resposta válida
        if (chatResponse == null)
        {
            throw new InvalidOperationException("A resposta da API está vazia ou mal formatada.");
        }

        if (string.IsNullOrWhiteSpace(chatResponse.Response))
        {
            throw new InvalidOperationException("A API retornou uma resposta vazia.");
        }

        // Retorna apenas o texto da resposta
        return chatResponse.Response;
>>>>>>> Stashed changes
    }
}
