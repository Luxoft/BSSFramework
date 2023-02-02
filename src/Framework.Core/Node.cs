using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core
{
    public class Node<T> : IEnumerable<T>
    {
        public Node(T value, IEnumerable<Node<T>> children)
        {
            if (children == null) throw new ArgumentNullException(nameof(children));

            this.Value = value;
            this.Children = children.CheckNotNull().ToReadOnlyCollection();
        }


        public T Value { get; }

        public IEnumerable<Node<T>> Children { get; internal set; }


        public override string ToString()
        {
            return this.Value == null ? "{null}" : this.Value.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.GetAllElements(v => v.Children).Select(node => node.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class NodeP<T> : Node<T>
    {
        internal NodeP(T value)
            : base(value, new Node<T>[0])
        {

        }

        public new IEnumerable<NodeP<T>> Children
        {
            get { return (IEnumerable<NodeP<T>>) base.Children; }
            internal set { base.Children = value; }
        }

        public NodeP<T> Parent { get; internal set; }
    }

    public static class NodeExtensions
    {
        public static Node<TResult> Select<TSource, TResult>(this Node<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new Node<TResult>(selector(source.Value), source.Children.Select(child => child.Select(selector)));
        }

        public static Node<T> Select<T>(this Node<T> source, Func<T, IEnumerable<T>, T> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var children = source.Children.Select(child => child.Select(selector)).ToReadOnlyCollection();

            return new Node<T>(selector(source.Value, children.Select(v => v.Value)), children);
        }
    }
}
