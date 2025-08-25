using Domain.Entities;


namespace Domain.States.Turns
{
    public interface ITurnState
    {
        Task FlipAsync(Game game, int playerId, int index);
    }
}
