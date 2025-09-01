using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Salas;

public class LobbyState: IEstadoSala 
{
    public Task UnirseAsync(Sala sala, Jugador jugador)
    {
        if (sala.Estado != EstadoSala.Lobby)
            throw new InvalidOperationException("No se puede unir: la sala no está en Lobby.");

        if (sala.Jugadores.Any(sj => sj.JugadorId == jugador.JugadorId))
            throw new InvalidOperationException("El jugador ya está en la sala.");

        if (sala.Jugadores.Count >= sala.MaxJugadores)
            throw new InvalidOperationException("La sala está llena.");
        return Task.CompletedTask;
    }

    public Task IniciarAsync(Sala sala, int filas, int columnas)
    {
        if (sala.Estado != EstadoSala.Lobby)
            throw new InvalidOperationException("No se puede iniciar: la sala no está en Lobby.");

        if (filas <= 0 || columnas <= 0 || (filas * columnas) % 2 != 0)
            throw new InvalidOperationException("Dimensiones inválidas (rows*cols debe ser par).");

        if (sala.Jugadores is null || sala.Jugadores.Count < 2)
            throw new InvalidOperationException("Se requieren al menos 2 jugadores.");

        
        sala.Estado = EstadoSala.EnJuego;
        return Task.CompletedTask;
    }

    public Task FinalizarAsync(Sala sala)
        => throw new InvalidOperationException("No se puede finalizar una sala que no ha iniciado.");
}
