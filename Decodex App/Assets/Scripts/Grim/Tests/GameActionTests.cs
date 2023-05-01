using Grim;
using Grim.Events;
using Grim.Rules;
using NSubstitute;
using NUnit.Framework;
using System;

public class GameActionTests 
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
        Assert.Throws<ArgumentNullException>(() => new GameAction<object>(null, null, null));
        Assert.Throws<ArgumentNullException>(() => new GameAction<object>("ctx", null, null));
        Assert.Throws<ArgumentNullException>(() => new GameAction<object>("ctx", "id", null));
        Assert.Throws<ArgumentNullException>(() => new GameAction<object>(null, "id", null));
        Assert.Throws<ArgumentNullException>(() => new GameAction<object>("ctx", null, x => { }));
        Assert.Throws<ArgumentNullException>(() => new GameAction<object>(null, "id", x => { }));
        Assert.Throws<ArgumentNullException>(() => new GameAction<object>(null, null, x => { }));
        Assert.DoesNotThrow(() => new GameAction<object>("ctx", "id", x => { }));
    }

    [Test]
    public void ExecuteSet()
    {
        TestArguments arg = GenerateTestArguments();
        var action = new GameAction<TestArguments>("ctx", "id", x => x.Args.anInteger = 3);
        action.Execute(ref arg );
        Assert.AreEqual(3, arg.anInteger);
    }

    [Test]
    public void ExecuteGet()
    {
        TestArguments arg = GenerateTestArguments();
        string value = "";
        var action = new GameAction<TestArguments>("ctx", "id", x => value = arg.aString);
        action.Execute(ref arg);
        Assert.AreEqual("hello", value);
    }

    [Test]
    public void ExecuteEvents()
    {
        TestArguments arg = GenerateTestArguments();
        var action = new GameAction<TestArguments>("ctx", "id", x => { });
        var callbackPre = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var callbackPost = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        GameEvents.Current.Subscribe("ctx", "Pre_id", callbackPre);
        GameEvents.Current.Subscribe("ctx", "Post_id", callbackPost);
        action.Execute(ref arg);
        callbackPre.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
        callbackPost.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
    }

    [Test]
    public void InterruptExecution()
    {
        TestArguments arg = GenerateTestArguments();
        var action = new GameAction<TestArguments>("ctx", "id", x => { });
        Action<GameEventArgs<TestArguments>> callbackInterrupt = x => x.Continue = false;
        var callbackPre1 = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var callbackPre2 = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        var callbackPost = Substitute.For<Action<GameEventArgs<TestArguments>>>();
        GameEvents.Current.Subscribe("ctx", "Pre_id", callbackPre1);
        GameEvents.Current.Subscribe("ctx", "Pre_id", callbackInterrupt);
        GameEvents.Current.Subscribe("ctx", "Pre_id", callbackPre2);
        GameEvents.Current.Subscribe("ctx", "Post_id", callbackPost);
        action.Execute(ref arg);
        callbackPre1.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
        callbackPre2.Received(1).Invoke(Arg.Any<GameEventArgs<TestArguments>>());
        callbackPost.DidNotReceive().Invoke(Arg.Any<GameEventArgs<TestArguments>>());
    }

    //[Test]
    //public void ReplacementDifferentAction()
    //{
    //    TestArguments arg = GenerateTestArguments();
    //    var result = 0;
    //    var baseAction = new GameAction<TestArguments>("ctx", "base1", x => result = x.Args.anInteger);
    //    baseAction.Execute(ref arg);
    //    Assert.AreEqual(1, result);

    //    var replacementAction = new GameAction<TestArguments>("ctx", "replacement1", x => { result = x.Args.anObject.anInteger; x.Continue = false; });
    //    GameEvents.Current.Subscribe<GameEventArgs<TestArguments>>("ctx", "Pre_base1", x => replacementAction.Execute(ref x));
    //    arg = GenerateTestArguments();
    //    baseAction.Execute(ref arg);
    //    Assert.AreEqual(3, result);
    //}

    //[Test]
    //public void ReplacementSameAction()
    //{
    //    TestArguments arg = GenerateTestArguments();
    //    var result = 0;
    //    var baseAction = new GameAction<TestArguments>("ctx","base2", x => result = x.Args.anInteger);
    //    baseAction.Execute(ref arg);
    //    Assert.AreEqual(1, result);
        
    //    var replacementAction = new GameAction<TestArguments>("ctx", "replacement2", x => x.Args.anInteger *= 2);
    //    GameEvents.Current.Subscribe<GameEventArgs<TestArguments>>("ctx", "Pre_base2", x => replacementAction.Execute(ref x));
    //    arg = GenerateTestArguments();
    //    baseAction.Execute(ref arg);
    //    Assert.AreEqual(2, result);
    //}


    //[Test]
    //public void ReplacementCircularAction()
    //{
    //    TestArguments arg = GenerateTestArguments();
    //    var result = 0;
    //    var baseAction = new GameAction<TestArguments>("ctx", "base3", x => result = x.Args.anInteger);

    //    var replacementAction1 = new GameAction<TestArguments>("ctx", "replacement11", x => { result = x.Args.anObject.anInteger; x.Continue = false;  });
    //    GameEvents.Current.Subscribe<GameEventArgs<TestArguments>>("ctx", "Pre_base3", x => replacementAction1.Execute(ref x));
    //    var replacementAction2 = new GameAction<TestArguments>("ctx", "replacement22", x => { baseAction.Execute(ref x); x.Continue = false; });
    //    GameEvents.Current.Subscribe<GameEventArgs<TestArguments>>("ctx", "Pre_replacement11", x => replacementAction2.Execute(ref x));

    //    arg = GenerateTestArguments();
    //    baseAction.Execute(ref arg);
    //    Assert.AreEqual(1, result);
    //}
}
