using ChatOllama.Api.Infrastructure.IA;
using ChatOllama.Api.Infrastructure.Repositories;
using ChatOllama.Api.Models;
using ChatOllama.Shared.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OllamaSharp;
using Scalar.AspNetCore;

string local_model = "llama3.1:latest";
string cloud_model = "gemma4:31b-cloud";
//string cloud_model = "gpt-oss:20b-cloud";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEfCore();
builder.Services.AddOpenApi();

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
        SelectedModel = local_model // Modelo padrão inicial
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
    // Adiciona a interface do Scalar
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("ChatOllama API");
        options.DarkMode = true;
        options.WithTheme(ScalarTheme.BluePlanet); 
    });
}

app.MapGet("/chat", async ([FromServices] IAiChatService aiChat, string prompt) =>
{
    string text = await aiChat.SendMessageAsync(Guid.NewGuid(), prompt, cloud_model);
    Console.WriteLine(text);
    return Results.Text(text, contentEncoding: System.Text.Encoding.UTF8);
});

app.MapPost("/chat-temporary", async ([FromServices] IAiChatService aiChat, [FromBody] ChatRequest request) =>
{
    try
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Prompt))
        {
            return Results.BadRequest(new { error = "O prompt não pode estar vazio" });
        }

        string text = await aiChat.SendMessageAsync(request.Prompt, cloud_model);
        Console.WriteLine(text);

        // Retorna a resposta no formato esperado pelo cliente
        var response = new ChatOllama.Api.Models.ChatResponse { Response = text };
        return Results.Json(response);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Erro no endpoint chat-temporary: {ex.Message}");
        return Results.Problem("Ocorreu um erro interno no servidor", statusCode: 500);
    }
});
app.MapGet("/chat-stream", async ([FromServices] IAiChatService aiChat, string prompt) =>
{
    string txt = string.Empty;
    await foreach (var item in aiChat.StreamMessageAsync(Guid.NewGuid(), prompt, cloud_model))
    {
        txt += item;
        Console.Write(item);
    }
    return Results.Text(txt, contentEncoding: System.Text.Encoding.UTF8);
});

app.UseHttpsRedirection();

app.Run();