using ChatOllama.Api.Infrastructure.IA;
using ChatOllama.Shared.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OllamaSharp;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEfCore();

// 1. Configura o HttpClient específico para o Ollama com o Timeout de 3 minutos
builder.Services.AddHttpClient("OllamaClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:11434");
    client.Timeout = TimeSpan.FromMinutes(3);
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/chat", async ([FromServices] IAiChatService aiChat) =>
{
    string text = await aiChat.SendMessageAsync(null, "ola, meu nome é cristiano", "gpt-oss:20b-cloud");
    string text1 = await aiChat.SendMessageAsync(null, "Qual o meu nome?", "gpt-oss:20b-cloud");
    Console.WriteLine(text);
    return Results.Ok(text + text1);
});

app.UseHttpsRedirection();

app.Run();