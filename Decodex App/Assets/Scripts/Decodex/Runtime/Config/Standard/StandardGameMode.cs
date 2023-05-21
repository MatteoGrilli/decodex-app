using Grim.Rules;
using Grim.Zones;
using System;
using UnityEngine;

namespace Decodex
{
    public class StandardGameMode
    {
        public void InitRuleEngine()
        {
            RuleEngine.Instance.Reset();
            StandardGameModeRules.RegisterPaths();
            StandardGameModeRules.RegisterRulesStartOfGame();
            StandardGameModeRules.RegisterRulesPlayerActions();
        }

        public void CreateZones()
        {
            var zonesRootObj = new GameObject("Zones");
            var cardsRootObk = new GameObject("Cards");
            StandardGameModeZones.CreateBoard("board", zonesRootObj.transform, Vector3.zero, Quaternion.identity);

            var hand1Position = new Vector3(0, 1, -4.5f);
            var hand1Rotation = Quaternion.Euler(new Vector3(-67,-180,0));
            StandardGameModeZones.CreateHand("hand_1", zonesRootObj.transform, hand1Position, hand1Rotation);

            var hand2Position = new Vector3(0, 1, 4.5f);
            var hand2Rotation = Quaternion.Euler(new Vector3(-67,0,0));
            StandardGameModeZones.CreateHand("hand_2", zonesRootObj.transform, hand2Position, hand2Rotation);

            var memory1Position = new Vector3(0, 0, -3.75f);
            var memory1Rotation = Quaternion.Euler(new Vector3(270, 180, 0));
            StandardGameModeZones.CreateMemory("memory_1", zonesRootObj.transform, memory1Position, memory1Rotation);

            var memory2Position = new Vector3(0, 0, 3.75f);
            var memory2Rotation = Quaternion.Euler(new Vector3(270, 0, 0));
            StandardGameModeZones.CreateMemory("memory_2", zonesRootObj.transform, memory2Position, memory2Rotation);


            var deck1Position = new Vector3(5, 0, -3.75f);
            var deck1Rotation = Quaternion.Euler(new Vector3(270, 180, 0));
            StandardGameModeZones.CreateDeck("deck_1", zonesRootObj.transform, deck1Position, deck1Rotation);

            var deck2Position = new Vector3(-5, 0, 3.75f);
            var deck2Rotation = Quaternion.Euler(new Vector3(270, 0, 0));
            StandardGameModeZones.CreateDeck("deck_2", zonesRootObj.transform, deck2Position, deck2Rotation);

            Array.ForEach(zonesRootObj.GetComponentsInChildren<IRenderable>(), renderable => renderable.Render());
        }

        public void StartGame()
        {
            RuleEngine.Instance.Process(new GameEventData(GameEventTypes.StartGame));
        }
    }
}
