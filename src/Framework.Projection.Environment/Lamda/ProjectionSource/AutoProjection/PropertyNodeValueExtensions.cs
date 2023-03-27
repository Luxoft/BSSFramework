using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

internal static class PropertyNodeValueExtensions
{
    public static IEnumerable<Node<ProjectionNodeValue>> ToNodes([NotNull] this IEnumerable<ProjectionPath> paths)
    {
        if (paths == null) throw new ArgumentNullException(nameof(paths));

        return from projectionPath in paths

               let path = projectionPath.PropertyPath

               where !path.IsEmpty

               group new { projectionPath.LastProperty, SubPath = path.Tail } by new { Property = path.Head, LastProperty = path.Tail.IsEmpty ? projectionPath.LastProperty : null }

               into g

               let currentNodeValue = g.Key.LastProperty == null ? new ProjectionNodeValue(g.Key.Property) : new ProjectionLastNodeValue(g.Key.Property, g.Key.LastProperty)

               select new Node<ProjectionNodeValue>(currentNodeValue, g.Select(pair => new ProjectionPath(pair.SubPath, pair.LastProperty)).ToNodes());
    }

    public static IEnumerable<ProjectionPath> ToPaths([NotNull] this Node<ProjectionNodeValue> node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        var property = node.Value.Property;

        switch (node.Value)
        {
            case ProjectionLastNodeValue lastValue:

                yield return new ProjectionPath(new PropertyPath(new [] { property }), lastValue.LastProperty);
                break;

            default:

                foreach (var subPath in node.Children.SelectMany(subNode => subNode.ToPaths()))
                {
                    yield return new ProjectionPath(property + subPath.PropertyPath, subPath.LastProperty);
                }
                break;
        }
    }
}
