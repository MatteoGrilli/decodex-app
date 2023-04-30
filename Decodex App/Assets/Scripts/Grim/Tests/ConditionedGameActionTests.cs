using Grim;
using Grim.Events;
using Grim.Rules;
using NSubstitute;
using NUnit.Framework;
using System;

public class ConditionedGameActionTests
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
        result.anInteger = 3;
        result.aString = "hello";
        result.aFloat = 1.4f;
        result.anObject = new();
        result.anObject.anInteger = 2;
        result.anObject.aString = "bbye";
        result.anObject.aFloat = 4.1f;
        return result;
    }

    [Test]
    public void New()
    {
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>(null, null, (Condition<TestArguments>)null, null));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>("ctx", null, (Condition<TestArguments>)null, null));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>(null, "id", (Condition<TestArguments>)null, null));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>(null, null, x => true, null));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>(null, null, (Condition<TestArguments>)null, x => { }));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>("ctx", "id", (Condition<TestArguments>)null, null));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>("ctx", null, x => true, null));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>("ctx", null, (Condition<TestArguments>)null, x => { }));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>(null, "id", x => true, null));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>(null, "id", (Condition<TestArguments>)null, x => { }));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>(null, null, x => true, x => { }));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>(null, "id", x => true, x => { }));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>("ctx", null, x => true, x => { }));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>("ctx", "id", (Condition<TestArguments>)null, x => { }));
        Assert.Throws<ArgumentNullException>(() => new ConditionedGameAction<TestArguments>("ctx", "id", x => true, null));
        Assert.DoesNotThrow(() => new ConditionedGameAction<TestArguments>("ctx", "id", x => true, x => { }));
    }

    [Test]
    public void ExecuteTrue()
    {
        var args = GenerateTestArguments();
        var action = new ConditionedGameAction<TestArguments>("ctx", "id", x => true, x => { });
        var callbackPre = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var callbackPost = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        GameEvents.Current.Subscribe("ctx", "Pre_id", callbackPre);
        GameEvents.Current.Subscribe("ctx", "Post_id", callbackPost);
        action.Execute(ref args);
        callbackPre.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
        callbackPost.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
    }

    [Test]
    public void ExecuteFalse()
    {
        var args = GenerateTestArguments();
        var action = new ConditionedGameAction<TestArguments>("ctx", "id", x => false, x => { });
        var callbackPre = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var callbackPost = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        GameEvents.Current.Subscribe("ctx", "Pre_id", callbackPre);
        GameEvents.Current.Subscribe("ctx", "Post_id", callbackPost);
        action.Execute(ref args);
        callbackPre.DidNotReceive().Invoke(Arg.Any<GameEventArgs<TestArguments>>());
        callbackPost.DidNotReceive().Invoke(Arg.Any<GameEventArgs<TestArguments>>());
    }
}
