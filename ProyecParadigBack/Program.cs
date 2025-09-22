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
    .MinimumLevel.Debug()  // captura todo desde Debug hacia arriba
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // menos ruido de EF y ASP.NET
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
    options.AddPolicy("AllowFrontend", policy =>
    {

       policy.AllowAnyOrigin()         
              .AllowAnyHeader()          
              .AllowAnyMethod();
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

app.UseCors("AllowFrontend");
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapHub<GameHub>("/hubs/game");
app.MapControllers();


app.Run();
