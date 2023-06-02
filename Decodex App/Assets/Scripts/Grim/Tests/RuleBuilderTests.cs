using Grim.Rules;
using Grim.Utils;
using NUnit.Framework;
using System;
using System.Diagnostics.Contracts;

namespace Grim.Tests
{
    public class RuleBuilderTests
    {
        [Test]
        public void BuildDefaultMaxExecution()
        {
            var result = 0;
            var rule = Rule.New()
                .WithId("id")
                .WithPath(new[] { "A", "B", "C" })
                .WithAction(async x => { result = 3; })
                .Build();
            Assert.AreEqual("id", rule.Id);
            Assert.AreEqual(new[] { "A", "B", "C" }, rule.Path);
            Assert.IsNull(rule.MaxExecutions);
            Assert.IsTrue(rule.Condition(new GameEventData("")));
            rule.Action(new GameEventData(""));
            Assert.AreEqual(3, result);
        }

        [Test]
        public void BuildWithMaxExecution()
        {
            var result = 0;
            var rule = Rule.New()
                .WithId("id")
                .WithPath(new[] { "A", "B", "C" })
                .WithCondition(x => true)
                .WithAction(async x => { result = 3; })
                .WithMaxExecutions(1)
                .Build();
            Assert.AreEqual("id", rule.Id);
            Assert.AreEqual(new[] { "A", "B", "C" }, rule.Path);
            Assert.AreEqual(1, rule.MaxExecutions);
            Assert.IsTrue(rule.Condition(new GameEventData("")));
            rule.Action(new GameEventData(""));
            Assert.AreEqual(3, result);
        }

        [Test]
        public void MissingId()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Rule.New()
                    .WithPath(new[] { "A", "B", "C" })
                    .WithCondition(x => true)
                    .WithAction(async x => { })
                    .Build()
            );
        }

        [Test]
        public void MissingAction()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Rule.New()
                    .WithId("id")
                    .WithPath(new[] { "A", "B", "C" })
                    .WithCondition(x => true)
                    .Build()
            );
        }
    }
}
