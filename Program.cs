using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WargaGrpc.Data;
using WargaGrpc.Services;
using Npgsql;

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
                .WithOrigins("http://localhost:8081", "http://localhost:3000", "exp://10.200.6.14:8081")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5090, o => o.Protocols =
        HttpProtocols.Http1AndHttp2);
});

// Database context
builder.Services.AddDbContext<WargaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseCors("AllowReactApp");

// Map endpoints langsung (modern approach)
app.MapGrpcService<WargaService>();
app.MapControllers();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. HTTP API is available at /Warga");

app.Run();