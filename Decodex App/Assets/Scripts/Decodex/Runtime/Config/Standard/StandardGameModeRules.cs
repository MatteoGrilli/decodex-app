using Decodex.Cards;
using Grim.Rules;
using Grim.Zones;
using Grim.Zones.Coordinates;
using UnityEngine;

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
        }

        public static void RegisterRulesStartOfGame()
        {
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
                    Debug.Log("DRAWING_STARTING_HAND FOR PLAYER 1");
                    RuleEngine.Instance.Process(
                    new GameEventData(GameEventTypes.DrawN)
                        // TODO: determine this from the player object!
                        .Put<int>("AMOUNT", 7)
                        .Put<string>("ZONE_FROM", "deck_1")
                        .Put<string>("ZONE_TO", "hand_1")
                    );
                })
                .Build()
            );
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
                    Debug.Log($"DRAWING {data.Get<int>("AMOUNT")} CARDS!");
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
                })
                .Build()
            );
        }
    }
}
