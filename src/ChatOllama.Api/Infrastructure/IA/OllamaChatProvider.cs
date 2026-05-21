using ChatOllama.Shared.Domain.Interfaces;
using ChatOllama.Shared.Domain.Models;
using Microsoft.Extensions.AI;
using OllamaSharp;
using System.Runtime.CompilerServices;

namespace ChatOllama.Api.Infrastructure.IA
{
    public class OllamaChatProvider : IAiChatService
    {
        private readonly IChatClient _chatClient;
        private readonly OllamaApiClient _ollamaClient;

        public OllamaChatProvider(IChatClient chatClient, OllamaApiClient ollamaRawClient)
        {
            _chatClient = chatClient;
            _ollamaClient = ollamaRawClient;
        }

        public async Task<IEnumerable<string>> GetAvailableModelsAsync()
        {
            List<string> models = [];
            foreach (var model in await _ollamaClient.ListLocalModelsAsync())
            {
                models.Add(model.Name);
            }
            return models;
        }

        public async Task<string> SendMessageAsync(ChatSession session, string prompt, string modelName,
            CancellationToken cancellationToken = default)
        {
            var historicoDeMensagens = ConstruirHistorico(session, prompt);

            var opcoes = new ChatOptions { ModelId = modelName };

            var resposta = await _chatClient.GetResponseAsync(historicoDeMensagens, opcoes, cancellationToken);

            return resposta.Text;
        }

        public Task<string> SendMessageAsync(string prompt, string modelName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<string> StreamMessageAsync(
            ChatSession session, string prompt, string modelName, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var historicoDeMensagens = ConstruirHistorico(session, prompt);
            var opcoes = new ChatOptions { ModelId = modelName,  };

            var fluxoDeResposta = _chatClient.GetStreamingResponseAsync(historicoDeMensagens, opcoes, cancellationToken);
              
            await foreach (var pedaco in fluxoDeResposta.WithCancellation(cancellationToken))
            {
                if (!string.IsNullOrEmpty(pedaco.Text))
                {
                    yield return pedaco.Text;
                }
            }
        }
        private List<ChatMessage> ConstruirHistorico(ChatSession session, string prompt)
        {
            var mensagens = new List<ChatMessage>();
            if (session?.Mensagens != null)
            {
                foreach (var msg in session.Mensagens)
                {
                    var role = msg.Role.ToString() == "User" ? ChatRole.User : ChatRole.Assistant;
                    mensagens.Add(new ChatMessage(role, msg.Content));
                }
            }

            mensagens.Add(new ChatMessage(ChatRole.User, prompt));
            return mensagens;
        }
    }
}
