using Greenfil.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();  // Necesario para los controladores API
builder.Services.AddEndpointsApiExplorer();  // Necesario para Swagger
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