using UnityEngine;

namespace Decodex
{
    public class Test : MonoBehaviour
    {
        void Start()
        {
            var gameMode = new StandardGameMode();
            gameMode.CreateZones();
            gameMode.InitRuleEngine();
            gameMode.StartGame();
        }
    }
}
