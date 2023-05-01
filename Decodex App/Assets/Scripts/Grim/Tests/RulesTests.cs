using Grim;
using Grim.Rules;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

public class RulesTests
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
        // TODO: test null arguments
    }

    [Test]
    public void Register()
    {
        var triggers = new List<GameTrigger>();
        triggers.Add(new GameTrigger("ctx", "Pre_action"));
        var callback = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var ruleAction = new GameAction<TestArguments>("ctx", "ruleAction", callback);
        var rule = new GameRule<TestArguments>("rule", triggers, ruleAction);
        var action = new GameAction<TestArguments>("ctx", "action", payload => { });

        rule.Register();
        var args = GenerateTestArguments();
        action.Execute(ref args);

        callback.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
    }
}