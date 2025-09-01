using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Jugador> Jugadores => Set<Jugador>();
    public DbSet<Sala> Salas => Set<Sala>();
    public DbSet<SalaJugador> SalaJugadores => Set<SalaJugador>();
    public DbSet<Partida> Partidas => Set<Partida>();
    public DbSet<PartidaJugador> PartidaJugadores => Set<PartidaJugador>();
    public DbSet<CartaTablero> Tablero => Set<CartaTablero>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
