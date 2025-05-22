using greenfil_backend.Models;
using Greenfil.Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });builder.Services.AddEndpointsApiExplorer();  // Necesario para Swagger

builder.Services.AddDbContext<GreenfilContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Greenfil API", Version = "v1" });
});  // Configuración completa de Swagger

// Registra tu servicio Python
builder.Services.AddScoped<PythonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

// Mapea los controladores (incluyendo tu StlController)
app.MapControllers();

app.Run();