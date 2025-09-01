using Domain.Entities;



namespace Domain.States.Turnos
{
    public static class FabricaEstadoTurno
    {
        public static IEstadoTurno From(Partida p)
            => p.IndicePrimerVolteo is null ? new DesocupadoState() : new UnaReveladaState();
    }
}
