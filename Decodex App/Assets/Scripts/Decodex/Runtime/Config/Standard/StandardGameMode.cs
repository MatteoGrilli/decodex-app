using Decodex.Zones;
using Grim;
using Grim.Players;
using Grim.Rules;
using Grim.Zones;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Decodex
{
    public class StandardGameMode
    {
        private void RegisterPlayers()
        {
            List<Player> players = new();
            players.Add(CreatePlayer("player_1"));
            players.Add(CreatePlayer("player_2"));

            var playersRootObj = new GameObject("Players");
            players.ForEach(player =>
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
                var playerObj = GameObject.Instantiate(prefab);
                playerObj.transform.SetParent(playersRootObj.transform);
                playerObj.transform.localPosition = Vector3.zero;
                playerObj.transform.localRotation = Quaternion.identity;
                playerObj.name = player.Id;
                playerObj.GetComponent<PlayerController>().Init(player);
            });
        }

        private Player CreatePlayer(string id)
        {
            Dictionary<string, string> zoneIds = new();
            zoneIds.Add("hand", $"hand_{id}");
            zoneIds.Add("deck", $"deck_{id}");
            zoneIds.Add("memory", $"memory_{id}");
            return new(id, zoneIds, null, null, null);
        }

        private void InitRuleEngine()
        {
            RuleEngine.Instance.Reset();
            StandardGameModeRules.RegisterPaths();
            StandardGameModeRules.RegisterRulesStartOfGame();
            StandardGameModeRules.RegisterRulesPlayerActions();
            StandardGameModeRules.RegisterRulesTurnStructure();
        }

        private void CreateZones()
        {
            var zonesRootObj = new GameObject("Zones");
            var cardsRootObk = new GameObject("Cards");
            StandardGameModeZones.CreateBoard("board", zonesRootObj.transform, Vector3.zero, Quaternion.identity);

            var hand1Position = new Vector3(0, 1, -4.5f);
            var hand1Rotation = Quaternion.Euler(new Vector3(-67, -180, 0));
            StandardGameModeZones.CreateHand("hand_player_1", zonesRootObj.transform, hand1Position, hand1Rotation);

            var hand2Position = new Vector3(0, 1, 4.5f);
            var hand2Rotation = Quaternion.Euler(new Vector3(-67, 0, 0));
            StandardGameModeZones.CreateHand("hand_player_2", zonesRootObj.transform, hand2Position, hand2Rotation);

            var memory1Position = new Vector3(0, 0, -3.75f);
            var memory1Rotation = Quaternion.Euler(new Vector3(270, 180, 0));
            StandardGameModeZones.CreateMemory("memory_player_1", zonesRootObj.transform, memory1Position, memory1Rotation);

            var memory2Position = new Vector3(0, 0, 3.75f);
            var memory2Rotation = Quaternion.Euler(new Vector3(270, 0, 0));
            StandardGameModeZones.CreateMemory("memory_player_2", zonesRootObj.transform, memory2Position, memory2Rotation);


            var deck1Position = new Vector3(5, 0, -3.75f);
            var deck1Rotation = Quaternion.Euler(new Vector3(270, 180, 0));
            StandardGameModeZones.CreateDeck("deck_player_1", zonesRootObj.transform, deck1Position, deck1Rotation);

            var deck2Position = new Vector3(-5, 0, 3.75f);
            var deck2Rotation = Quaternion.Euler(new Vector3(270, 0, 0));
            StandardGameModeZones.CreateDeck("deck_player_2", zonesRootObj.transform, deck2Position, deck2Rotation);

            Array.ForEach(zonesRootObj.GetComponentsInChildren<IRenderable>(), renderable => renderable.Render());
        }

        public void StartGame()
        {
            CreateZones();
            RegisterPlayers();
            InitRuleEngine();
            RuleEngine.Instance.Process(
                new GameEventData(GameEventTypes.StartGame)
                    .Put<List<string>>("PLAYERS", new List<string>(new[] { "player_1", "player_2" })
               )
            );
        }
    }
}
