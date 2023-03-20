using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

internal class VerifyUniqueProjectionSource : IProjectionSource
{
    private readonly IProjectionSource baseSource;


    public VerifyUniqueProjectionSource([NotNull] IProjectionSource baseSource)
    {
        this.baseSource = baseSource ?? throw new ArgumentNullException(nameof(baseSource));
    }


    public IEnumerable<IProjection> GetProjections()
    {
        var projections = this.baseSource.GetProjections().ToList();

        var duplicateProjections = projections.GetDuplicates(new EqualityComparerImpl<IProjection>((proj1, proj2) => proj1.Name == proj2.Name)).ToList();

        if (duplicateProjections.Any())
        {
            throw new Exception($"Duplicate projections: {duplicateProjections.Select(proj => proj.Name).Distinct().Join(",")}");
        }

        foreach (var projection in projections)
        {
            var duplicateProperties = projection.Properties.GetDuplicates(new EqualityComparerImpl<IProjectionProperty>((prop1, prop2) => prop1.Name == prop2.Name)).ToList();

            if (duplicateProperties.Any())
            {
                throw new Exception($"Projection \"{projection.Name}\" has duplicate properties: {duplicateProperties.Select(prop => prop.Name).Distinct().Join(",")}");
            }
        }

        return projections;
    }
}
