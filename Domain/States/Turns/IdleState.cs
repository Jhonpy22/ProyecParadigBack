using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Turns
{
    public class IdleState : ITurnState
    {
        public Task FlipAsync(Game game, int playerId, int index)
        {
            EnsurePlayerTurn(game, playerId);
            var card = GetPlayableCard(game, index);

            // Primer volteo del turno
            game.FirstFlipIndex = card.Index;
            return Task.CompletedTask;
        }

        private static void EnsurePlayerTurn(Game g, int pid)
        {
            if (g.Status != GameStatus.InProgress)
                throw new InvalidOperationException("El juego no está en progreso.");
            if (g.CurrentPlayerId != pid)
                throw new InvalidOperationException("No es tu turno.");
        }

        private static BoardCard GetPlayableCard(Game g, int idx)
        {
            var card = g.Board.SingleOrDefault(c => c.Index == idx)
                       ?? throw new InvalidOperationException("Índice de carta inválido.");
            if (card.IsMatched) throw new InvalidOperationException("La carta ya está tomada.");
            return card;
        }
    }
}
