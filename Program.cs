using GerenciamentoTarefas.Domain.Interfaces;
using GerenciamentoTarefasAPI.Repository;
using GerenciamentoTarefasAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar o Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/logB3-.txt",
                  rollingInterval: RollingInterval.Day,
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// Configura��o de vari�veis de ambiente e arquivos de configura��o
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); // Isso garante que as vari�veis de ambiente sejam carregadas

// Configura��o do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // URL da sua aplica��o Angular
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Adiciona os servi�os ao cont�iner.
builder.Services.AddControllers();

// Configura��o do Swagger para documenta��o da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GerenciamentoTarefasAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira o token JWT no formato 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

    c.EnableAnnotations(); // Habilita o uso de anota��es no Swagger
});

// Configura��o do DbContext com PostgreSQL
builder.Services.AddDbContext<GerenciamentoTarefasContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro do RabbitMQService
builder.Services.AddSingleton<RabbitMQService>();

// Registro do TarefasRepository
builder.Services.AddScoped<TarefasRepository>();

// Registro do UsuarioService
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<ITarefasRepository, TarefasRepository>();
builder.Services.AddScoped<IRabbitMQService,RabbitMQService>();


// Leitura da chave JWT do Configuration
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("A chave JWT n�o pode ser nula ou vazia.");
}

// Configura��es do JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configura o pipeline de requisi��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GerenciamentoTarefasAPI v1");
    });
}

// Habilitar CORS antes de autentica��o e autoriza��o
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication(); // Adiciona autentica��o JWT
app.UseAuthorization();

app.MapControllers();

app.Run();
