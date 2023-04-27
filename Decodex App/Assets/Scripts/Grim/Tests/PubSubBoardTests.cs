using Grim.Events;
using NSubstitute;
using NUnit.Framework;
using System;

public class PubSubBoardTests
{
    [Test]
    public void Untyped()
    {
        var callback = Substitute.For<Action>();
        var board = new PubSubBoard();

        board.Subscribe("test", callback);
        board.Trigger("test");
        callback.Received(1).Invoke();

        callback.ClearReceivedCalls();

        board.Unsubscribe("test", callback);
        board.Trigger("test");
        callback.DidNotReceive().Invoke();
    }

    [Test]
    public void Typed()
    {
        var callback = Substitute.For<Action<int>>();
        var board = new PubSubBoard();

        board.Subscribe("test", callback);
        board.Trigger("test", 1);
        callback.Received(1).Invoke(1);

        callback.ClearReceivedCalls();

        board.Unsubscribe("test", callback);
        board.Trigger("test", 1);
        callback.DidNotReceive().Invoke(1);
    }

    [Test]
    public void TypedRef()
    {
        // Didn't know how to properly mock a delegate with ref param :D
        var number = 1;
        RefAction<int> callback = Increment;
        var board = new PubSubBoard();
        board.Subscribe("test", callback);
        board.Trigger("test", ref number);
        Assert.AreEqual(2, number);
        board.Trigger("test", ref number);
        Assert.AreEqual(3, number);

        board.Unsubscribe("test", callback);
        board.Trigger("test");
        Assert.AreEqual(3, number);
    }

    private void Increment(ref int number) => number++;
}