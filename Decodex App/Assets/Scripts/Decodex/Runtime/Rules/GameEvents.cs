using Grim.Rules;

namespace Decodex
{
    public static class GameEvents
    {
        public static GameEventData DrawN(int amount)
        {
                var payload = new GameEventData(GameEventTypes.DrawN);
                payload.Put<int>("AMOUNT", amount);
                return payload;
        }

        public static GameEventData Draw() => new GameEventData(GameEventTypes.Draw);
    }
}
