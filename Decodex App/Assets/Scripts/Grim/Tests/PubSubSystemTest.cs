using Grim.Events;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Grim.Tests
{
    public class PubSubSystemTest
    {
        [Test]
        public void Istantiate()
        {
            var instance = PubSubSystem.Current;
            Assert.IsNotNull(instance);
        }

        [Test]
        public void GlobalUntyped()
        {
            var callback = Substitute.For<Action>();

            PubSubSystem.Current.Subscribe("test", callback);
            PubSubSystem.Current.Trigger("test");
            callback.Received(1).Invoke();

            callback.ClearReceivedCalls();

            PubSubSystem.Current.Unsubscribe("test", callback);
            PubSubSystem.Current.Trigger("test");
            callback.DidNotReceive().Invoke();
        }

        [Test]
        public void GlobalTyped()
        {
            var callback = Substitute.For<Action<int>>();

            PubSubSystem.Current.Subscribe("test", callback);
            PubSubSystem.Current.Trigger("test", 1);
            callback.Received(1).Invoke(1);

            callback.ClearReceivedCalls();

            PubSubSystem.Current.Unsubscribe("test", callback);
            PubSubSystem.Current.Trigger("test", 1);
            callback.DidNotReceive().Invoke(1);
        }

        [Test]
        public void GlobalTypedRef()
        {
            var number = 1;
            RefAction<int> callback = Increment;

            PubSubSystem.Current.Subscribe("test", callback);
            PubSubSystem.Current.Trigger("test", ref number);
            Assert.AreEqual(2, number);

            PubSubSystem.Current.Unsubscribe("test", callback);
            PubSubSystem.Current.Trigger("test", ref number);
            Assert.AreEqual(2, number);
        }

        [Test]
        public void BoardUntyped()
        {
            var callback = Substitute.For<Action>();

            PubSubSystem.Current.Subscribe("board", "test", callback);
            PubSubSystem.Current.Trigger("board", "test");
            callback.Received(1).Invoke();

            callback.ClearReceivedCalls();

            PubSubSystem.Current.Unsubscribe("board", "test", callback);
            PubSubSystem.Current.Trigger("board", "test");
            callback.DidNotReceive().Invoke();
        }

        [Test]
        public void BoardTyped()
        {
            var callback = Substitute.For<Action<int>>();

            PubSubSystem.Current.Subscribe("board", "test", callback);
            PubSubSystem.Current.Trigger("board", "test", 1);
            callback.Received(1).Invoke(1);

            callback.ClearReceivedCalls();

            PubSubSystem.Current.Unsubscribe("board", "test", callback);
            PubSubSystem.Current.Trigger("board", "test", 1);
            callback.DidNotReceive().Invoke(1);
        }

        [Test]
        public void BoardTypedRef()
        {
            var number = 1;
            RefAction<int> callback = Increment;

            PubSubSystem.Current.Subscribe("board", "test", callback);
            PubSubSystem.Current.Trigger("board", "test", ref number);
            Assert.AreEqual(2, number);

            PubSubSystem.Current.Unsubscribe("board", "test", callback);
            PubSubSystem.Current.Trigger("board", "test", ref number);
            Assert.AreEqual(2, number);
        }

        private void Increment(ref int number) => number++;
    }
}