using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WargaGrpc.Data;
using WargaGrpc.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();

// Konfigurasi CORS untuk mengizinkan semua domain
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin() // Izinkan semua domain
                .AllowAnyMethod() // Izinkan semua metode HTTP
                .AllowAnyHeader(); // Izinkan semua header
        });
});

// Configure Kestrel untuk mendengarkan di semua antarmuka jaringan
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5090, o => o.Protocols = HttpProtocols.Http1); // Dengarkan di semua antarmuka jaringan
});

// Database context
builder.Services.AddDbContext<WargaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

// Aktifkan CORS
app.UseCors("AllowAll");

// Tambahkan middleware untuk mengirim header CORS
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    await next();
});

// Tangani preflight request (OPTIONS)
app.MapMethods("/warga", new[] { "OPTIONS" }, () => Results.Ok())
    .RequireCors("AllowAll");

// Map endpoints langsung (modern approach)
app.MapGrpcService<WargaService>();
app.MapControllers();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. HTTP API is available at /Warga");

app.Run();
