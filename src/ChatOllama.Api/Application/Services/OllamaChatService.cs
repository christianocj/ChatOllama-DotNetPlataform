using ChatOllama.Shared.Domain.Interfaces;
using OllamaSharp;

namespace ChatOllama.Api.Application.Services
{
    public class OllamaChatService
    {
        private readonly IAiChatService _chatService;
        private readonly IMessageRepository _messageRepository;
        private readonly ISessionRepository _sessionRepository;

        public OllamaChatService(
            IAiChatService chatService, 
            IMessageRepository messageRepository, 
            ISessionRepository sessionRepository)
        {
            _chatService = chatService;
            _messageRepository = messageRepository;
            _sessionRepository = sessionRepository;
        }
    }
}
