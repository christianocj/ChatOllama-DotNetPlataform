using ChatOllama.Shared.Domain.Enums;
using ChatOllama.Shared.Domain.Interfaces;
using ChatOllama.Shared.Domain.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using System.Runtime.CompilerServices;

namespace ChatOllama.Api.Infrastructure.IA
{
    public class OllamaChatProvider : IAiChatService
    {
        private readonly IChatClient _chatClient;
        private readonly OllamaApiClient _ollamaClient;
        private readonly IMessageRepository _messages;
        static readonly int idchat = 1;
        public OllamaChatProvider(IChatClient chatClient, OllamaApiClient ollamaRawClient, IMessageRepository sessionRepository)
        {
            _chatClient = chatClient;
            _ollamaClient = ollamaRawClient;
            _messages = sessionRepository;
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

        public async Task<string> SendMessageAsync(Guid sessionPublicId, string prompt, string modelName,
            CancellationToken cancellationToken = default)
        {
            var agent = _chatClient.AsAIAgent(
                new ChatClientAgentOptions
                {
                    //ChatHistoryProvider = new OllamaChatHistoryProvider(),
                    ChatOptions = new ChatOptions
                    {
                        ModelId = modelName,
                        Instructions = ""
                    }
                });

            var memory = await agent.CreateSessionAsync(cancellationToken);
            memory.SetInMemoryChatHistory( await RecuperarHistorico(sessionPublicId));

            var msg = new ChatMessage(ChatRole.User, prompt)
            {
                CreatedAt = DateTime.UtcNow,
                MessageId = Guid.CreateVersion7().ToString()
            };
            await AtualizarHistorico(MessageRole.User, prompt, sessionPublicId, modelName);

            var resposta = await agent.RunAsync(msg, memory, cancellationToken: cancellationToken);

            await AtualizarHistorico(MessageRole.Assistant, resposta.Text, sessionPublicId, modelName);

            memory.TryGetInMemoryChatHistory(out var list);
            Console.WriteLine(list?.Count);
            return resposta.Text;
        }

        //Para conversas temporárias, não guardar no banco
        public async Task<string> SendMessageAsync(string prompt, string modelName, CancellationToken cancellationToken = default)
        {
            var agent = _chatClient.AsAIAgent(
                new ChatClientAgentOptions
                {
                    ChatHistoryProvider = new OllamaChatHistoryProvider(),
                    ChatOptions = new ChatOptions
                    {
                        ModelId = modelName,
                        Instructions = ""
                    }
                });
            var resposta = await agent.RunAsync(prompt, cancellationToken: cancellationToken);
            return resposta.Text;
        }

        public async IAsyncEnumerable<string> StreamMessageAsync(
            ChatSession session, string prompt, string modelName, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var historicoDeMensagens = RecuperarHistorico(Guid.NewGuid());
            var opcoes = new ChatOptions { ModelId = modelName, };

            var fluxoDeResposta = _chatClient.GetStreamingResponseAsync(null, opcoes, cancellationToken);

            await foreach (var pedaco in fluxoDeResposta.WithCancellation(cancellationToken))
            {
                if (!string.IsNullOrEmpty(pedaco.Text))
                {
                    yield return pedaco.Text;
                }
            }
        }
        private async Task<List<ChatMessage>> RecuperarHistorico(Guid sessionPublicId)
        {
            var mensagens = new List<ChatMessage>();
            var sms = await _messages.GetMessagesBySessionAsync(idchat);
            ChatMessage msg;
            foreach (var item in sms)
            {
                var role = item.Role == MessageRole.User ? ChatRole.User : ChatRole.Assistant;
                msg = new ChatMessage(role, item.Content)
                {
                    MessageId = item.PublicId.ToString(),
                };
                mensagens.Add(msg);
            }

            return mensagens;
        }

        private async Task AtualizarHistorico(MessageRole chatRole, string texto, Guid publicSessionId, string model)
        {
            var msg = new Message
            {
                Content = texto,
                PublicId = Guid.CreateVersion7(),
                CreatedAt = DateTime.UtcNow,
                Role = chatRole,
                ModelName = model,
                ChatSessionId = idchat
            };
            await _messages.AddAsync(msg);
        }
    }
}
