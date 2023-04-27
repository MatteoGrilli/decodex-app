using Grim.Events;
using NSubstitute;
using NUnit.Framework;
using System;

public class GameEventsTest
{
    [Test]
    public void Istantiate()
    {
        var instance = GameEvents.Current;
        Assert.IsNotNull(instance);
    }

    [Test]
    public void GlobalUntyped()
    {
        var callback = Substitute.For<Action>();
        
        GameEvents.Current.Subscribe("test", callback);
        GameEvents.Current.Trigger("test");
        callback.Received(1).Invoke();

        callback.ClearReceivedCalls();

        GameEvents.Current.Unsubscribe("test", callback);
        GameEvents.Current.Trigger("test");
        callback.DidNotReceive().Invoke();
    }

    [Test]
    public void GlobalTyped()
    {
        var callback = Substitute.For<Action<int>>();

        GameEvents.Current.Subscribe("test", callback);
        GameEvents.Current.Trigger("test", 1);
        callback.Received(1).Invoke(1);

        callback.ClearReceivedCalls();

        GameEvents.Current.Unsubscribe("test", callback);
        GameEvents.Current.Trigger("test", 1);
        callback.DidNotReceive().Invoke(1);
    }

    [Test]
    public void GlobalTypedRef()
    {
        var number = 1;
        RefAction<int> callback = Increment;

        GameEvents.Current.Subscribe("test", callback);
        GameEvents.Current.Trigger("test", ref number);
        Assert.AreEqual(2, number);

        GameEvents.Current.Unsubscribe("test", callback);
        GameEvents.Current.Trigger("test", ref number);
        Assert.AreEqual(2, number);
    }

    [Test]
    public void BoardUntyped()
    {
        var callback = Substitute.For<Action>();

        GameEvents.Current.Subscribe("board", "test", callback);
        GameEvents.Current.Trigger("board", "test");
        callback.Received(1).Invoke();

        callback.ClearReceivedCalls();

        GameEvents.Current.Unsubscribe("board", "test", callback);
        GameEvents.Current.Trigger("board", "test");
        callback.DidNotReceive().Invoke();
    }

    [Test]
    public void BoardTyped()
    {
        var callback = Substitute.For<Action<int>>();

        GameEvents.Current.Subscribe("board", "test", callback);
        GameEvents.Current.Trigger("board", "test", 1);
        callback.Received(1).Invoke(1);

        callback.ClearReceivedCalls();

        GameEvents.Current.Unsubscribe("board", "test", callback);
        GameEvents.Current.Trigger("board", "test", 1);
        callback.DidNotReceive().Invoke(1);
    }

    [Test]
    public void BoardTypedRef()
    {
        var number = 1;
        RefAction<int> callback = Increment;

        GameEvents.Current.Subscribe("board", "test", callback);
        GameEvents.Current.Trigger("board", "test", ref number);
        Assert.AreEqual(2, number);

        GameEvents.Current.Unsubscribe("board", "test", callback);
        GameEvents.Current.Trigger("board", "test", ref number);
        Assert.AreEqual(2, number);
    }

    private void Increment(ref int number) => number++;
}