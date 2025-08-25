using Domain.Entities;
using Domain.Enums;


namespace Domain.States.Turns
{
    public class OneRevealedState : ITurnState
    {
        public Task FlipAsync(Game game, int playerId, int index)
        {
            EnsurePlayerTurn(game, playerId);

            if (game.FirstFlipIndex is null)
                throw new InvalidOperationException("Estado inconsistente: no hay primer volteo.");

            if (game.FirstFlipIndex.Value == index)
                throw new InvalidOperationException("No puedes seleccionar la misma carta.");

            var first = GetPlayableCard(game, game.FirstFlipIndex.Value);
            var second = GetPlayableCard(game, index);

            var isMatch = first.PairKey == second.PairKey;

            // Registrar movimiento
            game.Moves.Add(new Move
            {
                GameId = game.GameId,
                PlayerId = playerId,
                FirstIndex = first.Index,
                SecondIndex = second.Index,
                IsMatch = isMatch,
                CreatedAtUtc = DateTime.UtcNow
            });

            if (isMatch)
            {
                first.IsMatched = true;
                second.IsMatched = true;

                var gp = game.GamePlayers.Single(x => x.PlayerId == playerId);
                gp.Score += 1;

                // ¿Partida terminada?
                if (game.Board.All(c => c.IsMatched))
                {
                    game.Status = GameStatus.Finished;
                    game.EndedAtUtc = DateTime.UtcNow;
                }

                // Mismo jugador continúa
                game.FirstFlipIndex = null;
            }
            else
            {
                // Rotar turno
                game.FirstFlipIndex = null;
                game.CurrentPlayerId = NextPlayerId(game);
                game.TurnNumber++;
            }

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

        private static int NextPlayerId(Game g)
        {
            var ordered = g.GamePlayers
                           .OrderBy(x => x.TurnOrder)
                           .Select(x => x.PlayerId)
                           .ToList();
            var pos = ordered.IndexOf(g.CurrentPlayerId!.Value);
            return ordered[(pos + 1) % ordered.Count];
        }
    }
}
