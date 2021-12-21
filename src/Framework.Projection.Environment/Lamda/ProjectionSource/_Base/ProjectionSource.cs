using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.Projection.Lambda
{
    /// <summary>
    /// Источник проекций, проекции набираются по Projection полям рефлексией
    /// </summary>
    public abstract class ProjectionSource : IProjectionSource
    {
        /// <inheritdoc />
        public IEnumerable<IProjection> GetProjections()
        {
            var projections = this.GetInternalProjections().ToArray();

            var nameDuplicate = projections.GetDuplicates(new EqualityComparerImpl<IProjection>((p1, p2) => p1.Name == p2.Name)).ToArray();

            if (nameDuplicate.Any())
            {
                throw new InvalidOperationException($"Duplicate projection names: {nameDuplicate.Select(p => p.Name).Distinct().Join(", ")}");
            }

            return projections;
        }

        protected IEnumerable<IProjection> GetInternalProjections()
        {
            foreach (var property in this.GetType().GetProperties())
            {
                if (property.PropertyType.IsGenericTypeImplementation(typeof(Projection<>)))
                {
                    yield return (IProjection)property.GetValue(this);
                }
            }
        }
    }
}
