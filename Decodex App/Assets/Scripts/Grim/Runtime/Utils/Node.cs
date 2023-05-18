using System;
using System.Collections.Generic;

namespace Grim.Utils
{
    public class Node<K, T>
    {
        public T Value { get; private set; }
        private OrderedDictionary<K, Node<K, T>> _children;
        public OrderedDictionary<K, Node<K, T>> Children { get { return new(_children); } }
        private OrderedDictionary<K, Node<K, T>> _parents;
        public OrderedDictionary<K, Node<K, T>> Parents { get { return new(_parents); } }

        public Node() : this(default(T)) {}

        public Node(T value)
        {
            Value = value;
            _children = new();
            _parents = new();
        }

        public bool IsLeaf() => _children.Count == 0;
        public bool IsRoot() => _parents.Count == 0;

        public void AddChild(K key, Node<K, T> node)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (node == null) throw new ArgumentNullException(nameof(node));
            _children.Add(key, node);
            node._parents.Add(key, this);
        }

        public void RemoveChild(K key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            _children[key]._parents.Remove(key);
            _children.Remove(key);
        }
    }
}
