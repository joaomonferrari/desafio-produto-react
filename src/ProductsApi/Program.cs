using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using ProductsApi.Repositories;
using ProductsApi.Services;
using ProductsApi.Validators;
using ProductsApi.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Nesse arquivo Usamos a injeção de dependência nativa do .NET 8 (o container de Microsoft.Extensions.DependencyInjection)

//Controllers (Endpoints HTTP)
builder.Services.AddControllers();

//Validação: habilita o FluentValidation automático para DTOs:
builder.Services.AddFluentValidationAutoValidation();

//Registrar o validador do ProductDto:
builder.Services.AddScoped<IValidator<ProductDto>, ProductValidator>();

//Swagger (documentação interativa da API):
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });
});

// CORS libera o frontend local (Vite) em http://localhost:5173
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DI (Injeção de Dependência): diz quem implementa cada interface:
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

//Middleware do Swagger (UI):
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API v1");
});

//HTTPS (se estiver habilitado):
app.UseHttpsRedirection();

//Aplica a política de CORS:
app.UseCors("AllowVite");

//Conecta os Controllers nas rotas:
app.MapControllers();

//Starta o servidor (fica escutando):
app.Run();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite", p => p
        .WithOrigins(
            "http://localhost:5173",
            "https://joaomonferrari.github.io" // domínio do GitHub Pages
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});