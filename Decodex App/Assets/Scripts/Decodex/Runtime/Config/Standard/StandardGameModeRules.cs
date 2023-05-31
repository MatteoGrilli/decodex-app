using Decodex.Cards;
using Grim;
using Grim.Rules;
using Grim.Zones;
using Grim.Zones.Coordinates;
using System.Collections.Generic;
using UnityEngine;
using Grim.Utils;
using Decodex.Utils;
using System.Linq;
using Grim.Players;

namespace Decodex
{
    public class StandardGameModeRules
    {
        public static void RegisterPaths()
        {
            RuleEngine.Instance.RegisterPath(new[] { "CONTINUOUS", "ON_PLAYERS" });
            RuleEngine.Instance.RegisterPath(new[] { "CONTINUOUS", "ON_RULES" });
            RuleEngine.Instance.RegisterPath(new[] { "CONTINUOUS", "ON_OBJECTS", "LAYER_1" }); // To define layers here
            RuleEngine.Instance.RegisterPath(new[] { "REPLACEMENT", "SELF" });
            RuleEngine.Instance.RegisterPath(new[] { "REPLACEMENT", "CONTROL" });
            RuleEngine.Instance.RegisterPath(new[] { "REPLACEMENT", "COPY" });
            RuleEngine.Instance.RegisterPath(new[] { "REPLACEMENT", "OTHERS" });
            RuleEngine.Instance.RegisterPath(new[] { "ACTUATORS", "SELF" });
            RuleEngine.Instance.RegisterPath(new[] { "ACTUATORS", "TRIGGERS" });
            RuleEngine.Instance.RegisterPath(new[] { "ACTUATORS", "POST" });
        }

        public static void RegisterRulesStartOfGame()
        {
            // Determine starting player
            RuleEngine.Instance.Register(
                Rule.New()
                .WithId("DETERMINE_PLAYER_ORDER")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(data => data.Event == GameEventTypes.StartGame)
                .WithAction(data =>
                {
                    var players = GameObject.FindGameObjectsWithTag("PLAYER");
                    ArrayUtils.Shuffle(players);
                    GameState.Instance.PlayerOrder = new List<string>(players.Select(player => player.GetComponent<PlayerController>().Model.Id));
                    Debug.Log("DETERMINE_PLAYER_ORDER: Player order is: " + string.Join(", ", GameState.Instance.PlayerOrder));
                })
                .Build()
            );
            // Set up daemon
            // Set up memory
            // Draw a starting hand
            RuleEngine.Instance.Register(
                Rule.New()
                .WithId("DRAW_STARTING_HAND")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(data => data.Event == GameEventTypes.StartGame)
                .WithAction(data =>
                {
                    // TODO: Consider creating a utility for this
                    var playerModels = new List<Player>(GameObject.FindGameObjectsWithTag("PLAYER").Select(player => player.GetComponent<PlayerController>().Model));
                    playerModels.ForEach(playerModel =>
                    {
                        Debug.Log("DRAW_STARTING_HAND: Drawing hand for player " + playerModel.Id);
                        RuleEngine.Instance.Process(
                        new GameEventData(GameEventTypes.DrawN)
                            .Put<int>("AMOUNT", 7)
                            .Put<string>("ZONE_FROM", playerModel.ZoneIds["deck"])
                            .Put<string>("ZONE_TO", playerModel.ZoneIds["hand"])
                        );
                    });
                })
                .Build()
            );
            // Start first turn after round start
            RuleEngine.Instance.Register(
                Rule.New()
                .WithId("START_FIRST_ROUND")
                .WithPath(new[] { "ACTUATORS", "POST" })
                .WithCondition(data => data.Event == GameEventTypes.StartGame)
                .WithAction(data =>
                {
                    var firstPlayerId = GameState.Instance.PlayerOrder[0];
                    Debug.Log("START_ROUND: Starting round. First player is: " + firstPlayerId);
                    RuleEngine.Instance.Process(
                        new GameEventData(GameEventTypes.StartTurn)
                            .Put<string>("PLAYER", firstPlayerId)
                    );
                })
                .Build()
            );
        }

        public static void RegisterRulesTurnStructure()
        {
            // At the start of their turn, the turn player draws a card.
            RuleEngine.Instance.Register(
                Rule.New()
                .WithId("START_TURN_DRAW")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(data => data.Event == GameEventTypes.StartTurn)
                .WithAction(data =>
                {
                    var turnPlayerId = data.Get<string>("PLAYER");
                    var turnPlayerModel = GameObject.Find(turnPlayerId).GetComponent<PlayerController>().Model;
                    Debug.Log("START_TURN_DRAW: Drawing a card at start of turn for player " + turnPlayerId);
                    RuleEngine.Instance.Process(
                        new GameEventData(GameEventTypes.Draw)
                            .Put<string>("ZONE_FROM", turnPlayerModel.ZoneIds["deck"])
                            .Put<string>("ZONE_TO", turnPlayerModel.ZoneIds["hand"])
                    );
                })
                .Build()
            );
            // After a player's turn ends, either start the next player's turn or start the simulation
            RuleEngine.Instance.Register(
                Rule.New()
                .WithId("START_FOLLOWING_TURN")
                .WithPath("ACTUATORS", "POST")
                .WithCondition(data => {
                    var turnPlayer = data.Get<string>("PLAYER");
                    var orderInTurn = GameState.Instance.PlayerOrder.IndexOf(turnPlayer);
                    var isThereFollowingPlayer = orderInTurn < GameState.Instance.NumPlayers - 1;
                    return data.Event == GameEventTypes.EndTurn && isThereFollowingPlayer;
                })
                .WithAction(data => {
                    var turnPlayer = data.Get<string>("PLAYER");
                    var orderInTurn = GameState.Instance.PlayerOrder.IndexOf(turnPlayer);
                    var nextPlayer = GameState.Instance.PlayerOrder[orderInTurn + 1];
                    Debug.Log("START_FOLLOWING_TURN: Starting turn for player " + nextPlayer);
                    RuleEngine.Instance.Process(
                        new GameEventData(GameEventTypes.StartTurn)
                            .Put<string>("PLAYER", nextPlayer)
                    );
                })
                .Build()
            );
            RuleEngine.Instance.Register(
                Rule.New()
                .WithId("START_SIMULATION")
                .WithPath("ACTUATORS", "POST")
                .WithCondition(data => {
                    var turnPlayer = data.Get<string>("PLAYER");
                    var orderInTurn = GameState.Instance.PlayerOrder.IndexOf(turnPlayer);
                    var isThereFollowingPlayer = orderInTurn < GameState.Instance.NumPlayers - 1;
                    return data.Event == GameEventTypes.EndTurn && !isThereFollowingPlayer;
                })
                .WithAction(data => {
                    Debug.Log("START_SIMULATION: Starting simulation");
                    RuleEngine.Instance.Process(
                        new GameEventData(GameEventTypes.StartSimulation)
                    );
                })
                .Build()
            );
            // TODO: Describe all simulation flow!
        }

        public static void RegisterRulesPlayerActions()
        {
            // Draw multiple cards
            RuleEngine.Instance.Register(
                Rule.New()
                .WithId("DRAW_N")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(data => data.Event == GameEventTypes.DrawN)
                .WithAction(data =>
                {
                    for (int i = 0; i < data.Get<int>("AMOUNT"); i++)
                        RuleEngine.Instance.Process(
                            new GameEventData(GameEventTypes.Draw)
                                .Put<string>("ZONE_FROM", data.Get<string>("ZONE_FROM"))
                                .Put<string>("ZONE_TO", data.Get<string>("ZONE_TO"))
                        );
                })
                .Build()
            );
            // Draw one card
            RuleEngine.Instance.Register(
                Rule.New()
                .WithId("DRAW")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(data => data.Event == GameEventTypes.Draw)
                .WithAction(data =>
                {
                    var modelFrom = GameObject.Find(data.Get<string>("ZONE_FROM")).GetComponent<ZoneController<LinearCoordinate, CardInstance>>().Model;
                    var modelTo = GameObject.Find(data.Get<string>("ZONE_TO")).GetComponent<ZoneController<LinearCoordinate, CardInstance>>().Model;
                    var cardToDraw = modelFrom.Get(new LinearCoordinate(0));
                    modelFrom.Remove(new LinearCoordinate(0)); // todo: might be useful to have a remove with item as well
                    modelTo.Put(cardToDraw);
                    // Possibly delegate to a more generic "MOVE_CARD" rule.
                    // To further split into "REMOVE_CARD_FROM_ZONE" and "ADD_CARD_TO_ZONE"
                })
                .Build()
            );
        }
    }
}
