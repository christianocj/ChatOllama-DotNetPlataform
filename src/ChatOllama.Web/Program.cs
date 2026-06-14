using ChatOllama.Web.Components;
using ChatOllama.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ChatApiClient>();

builder.Services.AddHttpClient<ChatApiClient>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(3);
    client.BaseAddress = new Uri("https://localhost:7258/");
});
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
