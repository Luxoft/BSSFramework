using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Projection.Lambda
{
    public static class ProjectionSourceExtensions
    {
        public static IProjectionSource Add(this IProjectionSource baseSource, IProjectionSource otherSource)
        {
            if (baseSource == null) throw new ArgumentNullException(nameof(baseSource));
            if (otherSource == null) throw new ArgumentNullException(nameof(otherSource));

            return new[] { baseSource, otherSource }.Composite();
        }

        public static IProjectionSource Composite(this IEnumerable<IProjectionSource> sources)
        {
            if (sources == null) throw new ArgumentNullException(nameof(sources));

            return new CompositeProjectionSource(sources);
        }

        private class CompositeProjectionSource : IProjectionSource
        {
            private readonly IReadOnlyCollection<IProjectionSource> sources;

            public CompositeProjectionSource(IEnumerable<IProjectionSource> sources)
            {
                this.sources = (sources ?? throw new ArgumentNullException(nameof(sources))).ToArray();
            }

            public IEnumerable<IProjection> GetProjections()
            {
                return this.sources.SelectMany(p => p.GetProjections());
            }
        }
    }
}
