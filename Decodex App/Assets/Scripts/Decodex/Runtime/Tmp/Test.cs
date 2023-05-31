using Grim.Rules;
using System.Collections;
using UnityEngine;

namespace Decodex
{
    public class Test : MonoBehaviour
    {
        void Start()
        {
            var gameMode = new StandardGameMode();
            gameMode.StartGame();
            StartCoroutine(EndTurnTest(3, GameState.Instance.PlayerOrder[0]));
            StartCoroutine(EndTurnTest(6, GameState.Instance.PlayerOrder[1]));
        }

        private IEnumerator EndTurnTest(int delay, string playerId)
        {
            yield return new WaitForSeconds(delay);
            RuleEngine.Instance.Process(
                new GameEventData(GameEventTypes.EndTurn)
                    .Put<string>("PLAYER", playerId)
                );
            yield break;
        }
    }
}
