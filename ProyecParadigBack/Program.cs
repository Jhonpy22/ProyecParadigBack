using Application.Interfaces;
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

app.Run();
