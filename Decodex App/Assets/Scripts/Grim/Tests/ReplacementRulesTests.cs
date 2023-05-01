using Grim.Rules;
using Grim;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System;

public class ReplacementRulesTests
{
    private class TestArguments
    {
        public int anInteger;
        public string aString;
        public float aFloat;
        public TestArguments anObject;
    }

    private TestArguments GenerateTestArguments()
    {
        TestArguments result = new();
        result.anInteger = 1;
        result.aString = "hello";
        result.aFloat = 1.4f;
        result.anObject = new();
        result.anObject.anInteger = 3;
        result.anObject.aString = "bbye";
        result.anObject.aFloat = 4.1f;
        return result;
    }

    [Test]
    public void New()
    {
        // TODO: test null args
    }

    [Test]
    public void ReplaceWithNewAction()
    {
        var triggers = new List<GameTrigger>();
        triggers.Add(new GameTrigger("ctx", "Pre_action"));
        var ruleCallback = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var ruleAction = new GameAction<TestArguments>("ctx", "ruleAction", ruleCallback);
        var rule = new ReplacementGameRule<TestArguments>("rule", triggers, ruleAction);

        var actionCallback = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var action = new GameAction<TestArguments>("ctx", "action", actionCallback);

        rule.Register();
        var args = GenerateTestArguments();
        action.Execute(ref args);

        ruleCallback.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
        actionCallback.DidNotReceive().Invoke(Arg.Any<GameEventArgs<TestArguments>>());
    }

    [Test]
    public void ReplaceWithSameAction()
    {
        var actionCallback = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var action = new GameAction<TestArguments>("ctx", "action", actionCallback);

        var triggers = new List<GameTrigger>();
        triggers.Add(new GameTrigger("ctx", "Pre_action"));
        var ruleAction = new GameAction<TestArguments>("ctx", "action", payload => { payload.Args.anInteger = 3; action.Execute(ref payload); });
        var rule = new ReplacementGameRule<TestArguments>("rule", triggers, ruleAction);

        rule.Register();
        var args = GenerateTestArguments();
        action.Execute(ref args);

        actionCallback.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
        Assert.AreEqual(3, args.anInteger);
    }

    [Test]
    public void ReplaceCyclical()
    {
        var actionCallback = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var action = new GameAction<TestArguments>("ctx", "action", actionCallback);

        var triggers1 = new List<GameTrigger>();
        triggers1.Add(new GameTrigger("ctx", "Pre_action"));
        var rule1Callback = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var rule1Action = new GameAction<TestArguments>("ctx", "rule1Action", rule1Callback);
        var rule1 = new ReplacementGameRule<TestArguments>("rule1", triggers1, rule1Action);
        rule1.Register();

        var triggers2 = new List<GameTrigger>();
        triggers2.Add(new GameTrigger("ctx", "Pre_rule1Action"));
        var rule2Action = new GameAction<TestArguments>("ctx", "rule2Action", payload => { payload.Args.anInteger = 3; action.Execute(ref payload); });
        var rule2 = new ReplacementGameRule<TestArguments>("rule2", triggers2, rule2Action);
        rule2.Register();

        var args = GenerateTestArguments();
        action.Execute(ref args);

        actionCallback.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
        rule1Callback.DidNotReceive().Invoke(Arg.Any<GameEventArgs<TestArguments>>());
        Assert.AreEqual(3, args.anInteger);
    }
}