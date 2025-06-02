using System.Text;
using greenfil_backend.Models;
using Greenfil.Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// 🔐 Cadena de conexión directamente aquí (puedes cambiarla según tu MySQL)
var connectionString = "server=localhost;port=3306;user=root;password=1234;database=greenfil";

// ✅ Registro de servicios (antes de Build)

// Controladores y configuración JSON
=======
// Configuración de JSON para evitar ciclos de referencia
>>>>>>> f5250b035c6cb6a7ddff097203ecd55f48c62c60
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

<<<<<<< HEAD
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Greenfil API", Version = "v1" });
});

// DbContext
builder.Services.AddDbContext<GreenfilContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Servicios propios
builder.Services.AddScoped<PythonService>();
builder.Services.AddScoped<JwtService>(); // ✅ Mueve esto arriba, antes de Build()
=======
builder.Services.AddEndpointsApiExplorer();

// Configuración de DbContext
builder.Services.AddDbContext<GreenfilContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// 🔐 Configuración de Swagger con soporte para JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Greenfil API", Version = "v1" });

    // 🔑 Seguridad para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT con el prefijo 'Bearer'.\nEjemplo: Bearer eyJhbGciOi..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 🔧 Servicios y autenticación JWT
builder.Services.AddScoped<PythonService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();
>>>>>>> f5250b035c6cb6a7ddff097203ecd55f48c62c60

// 🔨 Construcción de la app
var app = builder.Build();

<<<<<<< HEAD
// Middleware y configuración del pipeline
=======
// Middleware de desarrollo + Swagger
>>>>>>> f5250b035c6cb6a7ddff097203ecd55f48c62c60
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greenfil API v1");
    });
}

// Middleware de seguridad
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
<<<<<<< HEAD
=======

// Mapea controladores
>>>>>>> f5250b035c6cb6a7ddff097203ecd55f48c62c60
app.MapControllers();

app.Run();
