using greenfil_backend.Models;
using Greenfil.Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Cadena de conexión directamente aquí (puedes cambiarla según tu MySQL)
var connectionString = "server=localhost;port=3306;user=root;password=1234;database=greenfil";

// ✅ Registro de servicios (antes de Build)

// Controladores y configuración JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

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

// 🔨 Construcción de la app
var app = builder.Build();

// Middleware y configuración del pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greenfil API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();