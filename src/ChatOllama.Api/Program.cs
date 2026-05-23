using ChatOllama.Api.Infrastructure.IA;
using ChatOllama.Api.Infrastructure.Repositories;
using ChatOllama.Shared.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OllamaSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEfCore();

// 1. Configura o HttpClient específico para o Ollama com o Timeout de 3 minutos
builder.Services.AddHttpClient("OllamaClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:11434");
    client.Timeout = TimeSpan.FromMinutes(10);
});

// 2. Regista o OllamaApiClient usando o HttpClient configurado acima
builder.Services.AddScoped(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("OllamaClient");

    return new OllamaApiClient(httpClient)
    {
        SelectedModel = "llama3.1:latest" // Modelo padrão inicial
    };
});

// 3. Regista a abstração IChatClient do Microsoft.Extensions.AI
builder.Services.AddScoped<IChatClient>(sp =>
    sp.GetRequiredService<OllamaApiClient>());

// 4. Regista o serviço de chat customizado para ficar desacoplado
builder.Services.AddScoped<IAiChatService, OllamaChatProvider>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/chat", async ([FromServices] IAiChatService aiChat, string prompt) =>
{
    string text = await aiChat.SendMessageAsync(Guid.NewGuid(), prompt, "gpt-oss:20b-cloud");
    Console.WriteLine(text);
    return Results.Text(text, contentEncoding: System.Text.Encoding.UTF8);
});

app.MapGet("/chat-temporary", async ([FromServices] IAiChatService aiChat, string prompt) =>
{
    string text = await aiChat.SendMessageAsync(prompt, "llama3.1");
    Console.WriteLine(text);
    return Results.Text(text, contentEncoding: System.Text.Encoding.UTF8);
});

app.UseHttpsRedirection();

app.Run();