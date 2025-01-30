using WargaGrpc.Data;
using WargaGrpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();

// Konfigurasi CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:5173") // URL frontend React
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddDbContext<WargaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

// Enable CORS
app.UseCors("AllowReactApp");

app.MapGrpcService<WargaService>();
app.MapControllers();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. HTTP API is available at /Warga");

app.Run();