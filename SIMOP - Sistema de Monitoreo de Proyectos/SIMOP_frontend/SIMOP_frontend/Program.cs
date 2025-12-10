using SIMOP_frontend.Components;
using SIMOP_frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddHttpClient("SIMOP_API", client =>
{
    client.BaseAddress = new Uri("https://localhost:44329/");  //link api
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient("SIMOP_API");
});


builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<IHitoService, HitoService>();
builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddScoped<IPresupuestoService, PresupuestoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
