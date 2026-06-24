using ChatOllama.Web.Models;
using ChatOllama.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChatOllama.Web.Components.Pages.Chat
{
    public partial class Chat
    {
        private List<ChatMessageModel> Messages = [];
        private List<ChatSession> Chats = new();
        private string? currentChatId;
        private ElementReference messagesContainer;
        private bool isLoading = false;

        [Inject]
        public ChatApiClient Api { get; set; } = default!;

        [Inject]
        public IJSRuntime JS { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadChatsFromStorage();
            if (!Chats.Any())
            {
                CreateNewChat();
            }
            else
            {
                await LoadChat(Chats.First().Id);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Messages.Any() && messagesContainer.Id != null)
            {
                await JS.InvokeVoidAsync("scrollToBottom", messagesContainer);
            }
        }

        private async Task LoadChatsFromStorage()
        {
            try
            {
                var saved = await JS.InvokeAsync<string>("localStorage.getItem", "chats");
                if (!string.IsNullOrEmpty(saved))
                {
                    Chats = System.Text.Json.JsonSerializer.Deserialize<List<ChatSession>>(saved) ?? new();
                }
            }
            catch { }
        }

        private void SaveChats()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(Chats);
            JS.InvokeVoidAsync("localStorage.setItem", "chats", json);
        }

        private void CreateNewChat()
        {
            var newChat = new ChatSession
            {
                Id = Guid.NewGuid().ToString(),
                Title = $"Conversa {Chats.Count + 1}",
                Messages = new List<ChatMessageModel>(),
                LastMessageDate = DateTime.Now
            };

            Chats.Insert(0, newChat);
            SaveChats();
            currentChatId = newChat.Id;
            Messages = newChat.Messages;
            StateHasChanged();
        }

        private async Task LoadChat(string chatId)
        {
            var chat = Chats.FirstOrDefault(c => c.Id == chatId);
            if (chat != null)
            {
                currentChatId = chat.Id;
                Messages = chat.Messages;
                StateHasChanged();
            }
        }

        private async Task DeleteChat(string chatId)
        {
            var chatToDelete = Chats.FirstOrDefault(c => c.Id == chatId);
            if (chatToDelete != null)
            {
                Chats.Remove(chatToDelete);
                if (currentChatId == chatId)
                {
                    if (Chats.Any())
                    {
                        await LoadChat(Chats.First().Id);
                    }
                    else
                    {
                        CreateNewChat();
                    }
                }
                SaveChats();
                StateHasChanged();
            }
        }

        private async Task SendMessage(string prompt)
        {
<<<<<<< Updated upstream
            Messages.Add(new()
            {
                Role = "user",
                Content = prompt
            });

=======
            // Adiciona a mensagem do usuário ao chat
            var userMessage = new ChatMessageModel
            {
                Role = "user",
                Content = prompt
            };
            Messages.Add(userMessage);

            // Atualiza o chat atual se existir
>>>>>>> Stashed changes
            var currentChat = Chats.FirstOrDefault(c => c.Id == currentChatId);
            if (currentChat != null)
            {
                currentChat.Messages = Messages;
                currentChat.LastMessageDate = DateTime.Now;
                if (Messages.Count == 1)
                {
                    currentChat.Title = prompt.Length > 30 ? prompt.Substring(0, 30) + "..." : prompt;
                }
                SaveChats();
            }

            StateHasChanged();

<<<<<<< Updated upstream
            isLoading = true;
            StateHasChanged();

            string response = await Api.SendAsync(prompt);

            isLoading = false;

            Messages.Add(new()
            {
                Role = "assistant",
                Content = response
            });

            if (currentChat != null)
            {
                currentChat.Messages = Messages;
                SaveChats();
            }

            StateHasChanged();
=======
            // Mostra indicador de carregamento
            isLoading = true;
            StateHasChanged();

            try
            {
                // Envia a mensagem para a API e aguarda a resposta
                string response = await Api.SendAsync(prompt);

                // Adiciona a resposta do assistente ao chat
                var assistantMessage = new ChatMessageModel
                {
                    Role = "assistant",
                    Content = response
                };
                Messages.Add(assistantMessage);

                // Atualiza o chat atual se existir
                if (currentChat != null)
                {
                    currentChat.Messages = Messages;
                    SaveChats();
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Trata erros de HTTP (conexão, timeout, etc.)
                var errorMessage = $"Erro de conexão: Não foi possível conectar-se ao serviço de chat. Verifique se o servidor está em execução.";

                Messages.Add(new ChatMessageModel
                {
                    Role = "assistant",
                    Content = errorMessage
                });

                if (currentChat != null)
                {
                    currentChat.Messages = Messages;
                    SaveChats();
                }

                // Log para diagnóstico (em produção, usar um logger adequado)
                Console.Error.WriteLine($"HTTP Error in SendMessage: {httpEx.Message}");
            }
            catch (InvalidOperationException invalidOpEx)
            {
                // Trata erros de operação inválida (resposta mal formatada, etc.)
                var errorMessage = $"Erro no processamento: {invalidOpEx.Message}";

                Messages.Add(new ChatMessageModel
                {
                    Role = "assistant",
                    Content = errorMessage
                });

                if (currentChat != null)
                {
                    currentChat.Messages = Messages;
                    SaveChats();
                }

                Console.Error.WriteLine($"Invalid Operation Error in SendMessage: {invalidOpEx.Message}");
            }
            catch (Exception ex)
            {
                // Trata qualquer outro erro inesperado
                var errorMessage = "Desculpe, ocorreu um erro inesperado. Por favor, tente novamente mais tarde.";

                Messages.Add(new ChatMessageModel
                {
                    Role = "assistant",
                    Content = errorMessage
                });

                if (currentChat != null)
                {
                    currentChat.Messages = Messages;
                    SaveChats();
                }

                // Log para diagnóstico
                Console.Error.WriteLine($"Unexpected Error in SendMessage: {ex.Message}");
            }
            finally
            {
                // Sempre esconde o indicador de carregamento
                isLoading = false;
                StateHasChanged();
            }
>>>>>>> Stashed changes
        }
    }
}
