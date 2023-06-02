using Grim.Rules;
using Grim.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grim.Tests
{
    public class RulesEngineTests
    {
        [Test]
        public void Istantiate()
        {
            var instance = RuleEngine.Instance;
            Assert.IsNotNull(instance);
        }

        [Test]
        public void RegisterRuleWrongStep()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            instance.RegisterPath(new[] { "CONTINUOUS", "ON_PLAYER" });
            var validRule = Rule.New()
                .WithId("valid")
                .WithAction(async x => { })
                .WithPath(new[] { "CONTINUOUS", "ON_PLAYER" })
                .Build();
            var invalidRule = Rule.New()
                .WithId("invalid")
                .WithAction(async x => { })
                .WithPath(new[] { "CONTINUOUS", "INVALID" })
                .Build();
            Assert.DoesNotThrow(() => instance.Register(validRule));
            Assert.Throws<Exception>(() => instance.Register(invalidRule));
        }

        [Test]
        public void RegisterRulePathTooShort()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            instance.RegisterPath(new[] { "CONTINUOUS", "ON_PLAYER" });
            var invalidRule = Rule.New()
                .WithId("valid")
                .WithAction(async x => { })
                .WithPath(new[] { "CONTINUOUS" })
                .Build();
            Assert.Throws<Exception>(() => instance.Register(invalidRule));
        }

        [Test]
        public void UnregisterRule()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            instance.RegisterPath(new[] { "CONTINUOUS", "ON_PLAYER" });
            var rule = Rule.New()
                .WithId("valid")
                .WithAction(async x => { })
                .WithPath(new[] { "CONTINUOUS", "ON_PLAYER" })
                .Build();
            instance.Register(rule);
        }

        [Test]
        public void ExecuteRule()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            instance.RegisterPath(new[] { "ACTUATORS", "SELF" });
            var log = new List<string>();
            var activatingRule = Rule.New()
                .WithId("activating")
                .WithCondition(x => x.Event == "DRAW_CARD")
                .WithAction(async x => { log.Add($"Drawn {x.Get<int>("AMOUNT")} cards"); })
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .Build();
            var nonActivatingRule = Rule.New()
                .WithId("non_activating")
                .WithCondition(x => x.Event == "NOT_DRAW_CARD") // any other id
                .WithAction(async x => { log.Add($"This should NOT trigger"); })
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .Build();
            instance.Register(activatingRule);
            instance.Register(nonActivatingRule);

            var eventPayload = new GameEventData("DRAW_CARD");
            eventPayload.Put<int>("AMOUNT", 1);

            instance.Process(eventPayload);

            Assert.Contains("Drawn 1 cards", log);
            Assert.That(log, Has.No.Member("This should NOT trigger"));
            Assert.AreEqual(1, log.Count);
        }

        [Test]
        public void ExecuteRuleEmptyPathBeforeExecution()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            instance.RegisterPath(new[] { "CONTINUOUS", "ON_PLAYERS" });
            instance.RegisterPath(new[] { "ACTUATORS", "SELF" });
            var log = new List<string>();
            var activatingRule = Rule.New()
                .WithId("activating")
                .WithCondition(x => x.Event == "DRAW_CARD")
                .WithAction(async x => { log.Add($"Drawn {x.Get<int>("AMOUNT")} cards"); })
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .Build();
            instance.Register(activatingRule);

            var eventPayload = new GameEventData("DRAW_CARD").Put<int>("AMOUNT", 1);

            instance.Process(eventPayload);

            Assert.Contains("Drawn 1 cards", log);
            Assert.That(log, Has.No.Member("This should NOT trigger"));
            Assert.AreEqual(1, log.Count);
        }

        [Test]
        public void ExecuteRuleWithTriggers()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            instance.RegisterPath(new[] { "ACTUATORS", "SELF" });
            instance.RegisterPath(new[] { "ACTUATORS", "TRIGGERS" });

            var log = new List<string>();
            var selfRule = Rule.New()
                .WithId("action_draw")
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithAction(async x => { log.Add($"Drawn {x.Get<int>("AMOUNT")} cards."); })
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .Build();
            var triggerRule1 = Rule.New()
                .WithId("draw_trigger_lifegain")
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithAction(async x => { log.Add($"Gained 1 life."); })
                .WithPath(new[] { "ACTUATORS", "TRIGGERS" })
                .Build();
            var triggerRule2 = Rule.New()
                .WithId("draw_trigger_discard")
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithAction(async x => { log.Add($"Discarded a card."); })
                .WithPath(new[] { "ACTUATORS", "TRIGGERS" })
                .Build();
            instance.Register(selfRule);
            instance.Register(triggerRule1);
            instance.Register(triggerRule2);

            var eventPayload = new GameEventData("ACTION_DRAW_N");
            eventPayload.Put<int>("AMOUNT", 2);

            instance.Process(eventPayload);

            Assert.Contains("Drawn 2 cards.", log);
            Assert.Contains("Gained 1 life.", log);
            Assert.Contains("Discarded a card.", log);
            Assert.AreEqual(3, log.Count);
        }

        [Test]
        public void ExecuteRuleWithReplacementStopExecution()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            // Order matters!
            instance.RegisterPath(new[] { "CONTINUOUS", "ON_RULES" });
            instance.RegisterPath(new[] { "ACTUATORS", "SELF" });
            
            var log = new List<string>();
            var selfRule = Rule.New()
                .WithId("action_draw")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithAction(async x => { log.Add($"Drawn {x.Get<int>("AMOUNT")} cards."); })
                .Build();
            var stopRule = Rule.New()
                .WithId("block_draw")
                .WithPath(new[] { "CONTINUOUS", "ON_RULES" })
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithAction(async x => System.Threading.Tasks.Task.FromResult(false))
                .Build();
            instance.Register(selfRule);
            instance.Register(stopRule);

            var eventPayload = new GameEventData("ACTION_DRAW_N");
            eventPayload.Put<int>("AMOUNT", 2);

            instance.Process(eventPayload);

            Assert.AreEqual(0, log.Count);
        }

        [Test]
        public void ExecuteRuleWithReplacementModifyPayload()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            // Order matters!
            instance.RegisterPath(new[] { "REPLACEMENT", "OTHER" });
            instance.RegisterPath(new[] { "ACTUATORS", "SELF" });

            var log = new List<string>();
            var selfRule = Rule.New()
                .WithId("action_draw")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithAction(async x => { log.Add($"Drawn {x.Get<int>("AMOUNT")} cards."); })
                .Build();
            var stopRule = Rule.New()
                .WithId("double_draw")
                .WithPath(new[] { "REPLACEMENT", "OTHER" })
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithMaxExecutions(1)
                .WithAction(async x => {
                    instance.Process(new GameEventData("ACTION_DRAW_N").Put<int>("AMOUNT", 2 * x.Get<int>("AMOUNT")));
                    return false;
                })
                .Build();
            instance.Register(selfRule);
            instance.Register(stopRule);

            var eventPayload = new GameEventData("ACTION_DRAW_N");
            eventPayload.Put<int>("AMOUNT", 2);

            instance.Process(eventPayload);

            Assert.Contains("Drawn 4 cards.", log);
            Assert.AreEqual(1, log.Count);
        }

        [Test]
        public void ExecuteRuleWithReplacementReplaceAction()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            // Order matters!
            instance.RegisterPath(new[] { "REPLACEMENT", "OTHER" });
            instance.RegisterPath(new[] { "ACTUATORS", "SELF" });

            var log = new List<string>();
            var ruleToReplace = Rule.New()
                .WithId("action_draw")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithAction(async x => { log.Add($"Drawn {x.Get<int>("AMOUNT")} cards."); })
                .Build();
            var ruleToExecute = Rule.New()
                .WithId("action_gain_life")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(x => x.Event == "GAIN_LIFE")
                .WithAction(async x => { log.Add($"Gained {x.Get<int>("AMOUNT")} life."); })
                .Build();
            var replacementRule = Rule.New()
                .WithId("replace_draw_with_lifegain")
                .WithPath(new[] { "REPLACEMENT", "OTHER" })
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithMaxExecutions(1)
                .WithAction(async x => {
                    instance.Process(new GameEventData("GAIN_LIFE").Put<int>("AMOUNT", x.Get<int>("AMOUNT")));
                    return false;
                })
                .Build();
            // Order doesn't matter
            instance.Register(replacementRule);
            instance.Register(ruleToReplace);
            instance.Register(ruleToExecute);

            var eventPayload = new GameEventData("ACTION_DRAW_N");
            eventPayload.Put<int>("AMOUNT", 2);

            instance.Process(eventPayload);

            Assert.Contains("Gained 2 life.", log);
            Assert.AreEqual(1, log.Count);
        }

        [Test]
        public void ExecuteRuleWithReplacementCyclical()
        {
            var instance = RuleEngine.Instance;
            instance.Reset();
            // Order matters!
            instance.RegisterPath(new[] { "REPLACEMENT", "OTHER" });
            instance.RegisterPath(new[] { "ACTUATORS", "SELF" });

            var log = new List<string>();
            var originalRule = Rule.New()
                .WithId("action_draw")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithAction(async x => { log.Add($"Drawn {x.Get<int>("AMOUNT")} cards."); })
                .Build();
            var replacementRule1 = Rule.New()
                .WithId("replace_lifegain_with_draw")
                .WithPath(new[] { "ACTUATORS", "SELF" })
                .WithCondition(x => x.Event == "GAIN_LIFE")
                .WithAction(async x => {
                    instance.Process(new GameEventData("ACTION_DRAW_N").Put<int>("AMOUNT", x.Get<int>("AMOUNT")));
                    return false;
                })
                .WithMaxExecutions(1)
                .Build();
            var replacementRule2 = Rule.New()
                .WithId("replace_draw_with_lifegain")
                .WithPath(new[] { "REPLACEMENT", "OTHER" })
                .WithCondition(x => x.Event == "ACTION_DRAW_N")
                .WithAction(async x => {
                    instance.Process(new GameEventData("GAIN_LIFE").Put<int>("AMOUNT", x.Get<int>("AMOUNT")));
                    return false;
                })
                .WithMaxExecutions(1)
                .Build();
            // Order doesn't matter
            instance.Register(replacementRule2);
            instance.Register(replacementRule1);
            instance.Register(originalRule);

            var eventPayload = new GameEventData("ACTION_DRAW_N");
            eventPayload.Put<int>("AMOUNT", 3);

            instance.Process(eventPayload);

            Assert.Contains("Drawn 3 cards.", log);
            Assert.AreEqual(1, log.Count);
        }
    }
}
