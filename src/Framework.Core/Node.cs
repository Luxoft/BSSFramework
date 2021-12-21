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

    public static class Node
    {
        public static IEnumerable<NodeP<TSource>> Create<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> getCurrentKey, Func<TSource, TKey> getParentKey)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (getCurrentKey == null) throw new ArgumentNullException(nameof(getCurrentKey));
            if (getParentKey == null) throw new ArgumentNullException(nameof(getParentKey));

            var groupedByParentNodes = source.GroupBy(getParentKey).ToArray();

            return groupedByParentNodes.Partial(g => g.Key.IsDefault(), (rootObjects, otherObject) =>

                otherObject.ToDictionary(g => g.Key, g => g.ToArray())
                           .Pipe(childrenDict => rootObjects.SelectMany().Select(rootObj => Create(rootObj, getCurrentKey, childrenDict, null))));
        }

        private static NodeP<TSource> Create<TSource, TKey>(TSource source, Func<TSource, TKey> getCurrentKey, Dictionary<TKey, TSource[]> childrenDict, NodeP<TSource> parent)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (getCurrentKey == null) throw new ArgumentNullException(nameof(getCurrentKey));
            if (childrenDict == null) throw new ArgumentNullException(nameof(childrenDict));

            var currentNode = new NodeP<TSource>(source) { Parent = parent };

            currentNode.Children = childrenDict.GetValueOrDefault(getCurrentKey(source), () => new TSource[0]).ToReadOnlyCollection(subObj => Create(subObj, getCurrentKey, childrenDict, currentNode));

            return currentNode;
        }
    }
}
