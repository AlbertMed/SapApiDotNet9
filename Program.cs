using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Acceso a la configuración
var configuration = builder.Configuration;

var app = builder.Build();

// Endpoint GET /hello
app.MapGet("/hello", () => "Hello World from SAP API");

// Endpoint POST /auth
app.MapPost("/auth", async (HttpContext context) =>
{
    var data = await context.Request.ReadFromJsonAsync<AuthRequest>();
    if (data?.Id == "usuario" && data?.Password == "clave")
        return Results.Ok(new { message = "Conexión a SAP exitosa" });
    else
        return Results.BadRequest(new { message = "Error de autenticación" });
});

// Endpoint GET entrada_material_inspeccion con parámetro en la URL
app.MapGet("/entrada_material_inspeccion/{id:int}", async (int id) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
        return Results.Problem("No se encontró la cadena de conexión a la base de datos.");

    using var connection = new SqlConnection(connectionString);
    using var command = new SqlCommand("GETENTRADAMATERIAL_CALIDAD", connection)
    {
        CommandType = CommandType.StoredProcedure
    };
    // Agregar parámetro @Id recibido en la URL
    command.Parameters.AddWithValue("@NumeroEntrada", id);

    await connection.OpenAsync();
    using var reader = await command.ExecuteReaderAsync();

    var result = new List<Dictionary<string, object>>();
    while (await reader.ReadAsync())
    {
        var row = new Dictionary<string, object>();
        for (int i = 0; i < reader.FieldCount; i++)
            row[reader.GetName(i)] = reader.GetValue(i);
        result.Add(row);
    }

    return Results.Ok(result);
});

app.Run();

record AuthRequest(string Id, string Password);
