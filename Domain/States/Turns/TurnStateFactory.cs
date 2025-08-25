using Domain.Entities;


namespace Domain.States.Turns
{
    public static class TurnStateFactory
    {
        public static ITurnState From(Game g)
            => g.FirstFlipIndex is null ? new IdleState() : new OneRevealedState();
    }
}
