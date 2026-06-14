namespace ChatOllama.Web.Services
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
    }
}
