using Grim.Utils;
using NUnit.Framework;

namespace Grim.Tests
{
    public class NodeTests
    {
        [Test]
        public void New()
        {
            Assert.DoesNotThrow(() => new Node<string, int>());
            Assert.DoesNotThrow(() => new Node<string, int>(3));
            var node = new Node<string, int>();
            Assert.AreEqual(0, node.Value);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0, node.Parents.Count);
            Assert.IsTrue(node.IsLeaf());
            Assert.IsTrue(node.IsRoot());
        }

        [Test]
        public void AddChild()
        {
            var node = new Node<string, int>();
            var child = new Node<string, int>(1);
            node.AddChild("A", child);
            Assert.AreEqual(1, node.Children.Count);
            Assert.AreEqual(1, child.Parents.Count);
            Assert.IsTrue(node.IsRoot());
            Assert.IsFalse(node.IsLeaf());
            Assert.IsFalse(child.IsRoot());
            Assert.IsTrue(child.IsLeaf());
            Assert.IsTrue(node.Children.ContainsKey("A"));
            Assert.IsTrue(child.Parents.ContainsKey("A"));
            Assert.AreEqual(1, node.Children["A"].Value);
        }

        [Test]
        public void RemoveChild()
        {
            var node = new Node<string, int>();
            var child = new Node<string, int>(1);
            node.AddChild("A", child);
            Assert.AreEqual(1, node.Children.Count);
            Assert.AreEqual(1, child.Parents.Count);
            node.RemoveChild("A");
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0, child.Parents.Count);
            Assert.IsTrue(node.IsRoot());
            Assert.IsTrue(node.IsLeaf());
            Assert.IsTrue(child.IsRoot());
            Assert.IsTrue(child.IsLeaf());
        }

        [Test]
        public void TamperWithReturnedArray()
        {
            var node = new Node<string, int>();
            node.Children.Add("A", new Node<string, int>(0));
            node.Parents.Add("A", new Node<string, int>(0));
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0, node.Parents.Count);
        }
    }
}