using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PII_2025_II_P2_GRUPO1.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar el contexto de base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar servicios necesarios para Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Verificar conexión a la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureCreated(); // Asegura que la base de datos y las tablas existan

        // Probar conexión directa
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("Conexión a la base de datos exitosa."); // Mensaje de éxito
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
    }
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();