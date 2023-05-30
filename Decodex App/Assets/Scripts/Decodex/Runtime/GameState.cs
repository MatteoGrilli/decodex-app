using System.Collections.Generic;

namespace Decodex
{
    public class GameState
    {
        private static GameState _instance;
        public static GameState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                }
                return _instance;
            }
        }

        public GameState()
        {
            PlayerOrder = new();
        }

        public List<string> PlayerOrder { get; set; }

        public object Properties { get; private set; }

        public int NumPlayers => PlayerOrder.Count;
    }
}
