using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

internal static class ProjectionBuilderExtensions
{
    public static IReadOnlyCollection<ProjectionBuilder> ToBuilders([NotNull] this IEnumerable<IProjection> projections)
    {
        if (projections == null) throw new ArgumentNullException(nameof(projections));

        var builderDict = projections.ToDictionary(proj => proj, proj => new ProjectionBuilder(proj));

        foreach (var projectionPair in builderDict)
        {
            foreach (var projectionProperty in projectionPair.Key.Properties)
            {
                projectionPair.Value.Properties.Add(new ProjectionPropertyBuilder(projectionProperty)
                                                    {
                                                            ElementProjection = projectionProperty.Type.ElementProjection.Maybe(proj => builderDict[proj])
                                                    });
            }

            foreach (var projectionProperty in projectionPair.Key.CustomProperties)
            {
                projectionPair.Value.CustomProperties.Add(new ProjectionCustomPropertyBuilder(projectionProperty)
                                                          {
                                                                  Type = projectionProperty.Type.TryOverrideElementProjection(proj => builderDict[proj])
                                                          });
            }
        }

        return builderDict.Values;
    }
}
