using Application.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using ProyecParadigBack.Hubs;
using ProyecParadigBack.Middlewares;
using ProyecParadigBack.Notificadores;
using Serilog;
using ServicesApp;




var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();


builder.Host.UseSerilog();


builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

builder.Services.AddScoped<ISvSalas, SvSalas>();
builder.Services.AddScoped<ISvPartidas, SvPartidas>();
builder.Services.AddScoped<ISvTurnos, SvTurnos>();
builder.Services.AddScoped<INotificadorJuego, NotificadorJuegoSignalR>();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); 
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapHub<GameHub>("/hubs/game");
app.MapControllers();

/*using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var limite = DateTime.UtcNow.AddHours(-1);

    var salasInactivas = db.Salas
        .Include(s => s.Partidas)
        .Include(s => s.Jugadores)
        .Where(s =>
            s.Estado == EstadoSala.Lobby &&
            !s.Jugadores.Any() &&
            (
                !s.Partidas.Any() ||
                s.Partidas.All(p => p.FinalizadaUtc != null && p.FinalizadaUtc < limite)
            )
        ).ToList();

    db.Salas.RemoveRange(salasInactivas);
    db.SaveChanges();
    Console.WriteLine($"Se eliminaron {salasInactivas.Count} salas inactivas.");
}*/
app.Run();
