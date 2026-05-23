using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace ChatOllama.Api.Infrastructure.IA
{
    public class OllamaChatTemporaryHistoryProvider : ChatHistoryProvider
    {
        readonly ProviderSessionState<State> _sessionState;
        readonly string _filePath;

        public OllamaChatTemporaryHistoryProvider()
        {
            _sessionState = new ProviderSessionState<State>(
                stateInitializer: _ => LoadFromFile(), stateKey: GetType().Name); ;
            _filePath = "temporarychat.json";
        }

        protected override ValueTask<IEnumerable<ChatMessage>> ProvideChatHistoryAsync(InvokingContext context, CancellationToken cancellationToken = default)
        {
            return new(_sessionState.GetOrInitializeState(context.Session).Messages);
        }

        protected override ValueTask StoreChatHistoryAsync(InvokedContext context, CancellationToken cancellationToken = default)
        {
            var state = _sessionState.GetOrInitializeState(context.Session);

            var allNewMassages = context.RequestMessages.Concat(context.ResponseMessages ?? []);

            state.Messages.AddRange(allNewMassages);
            _sessionState.SaveState(context.Session, state);

            SaveToFile(state);
            return default;
        }
        private State LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return new State();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<State>(json) ?? new State();
        }

        private void SaveToFile(State state)
        {
            var json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
