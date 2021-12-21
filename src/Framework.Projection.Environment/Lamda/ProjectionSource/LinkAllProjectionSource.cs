using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda
{
    internal class LinkAllProjectionSource : IProjectionSource
    {
        private readonly IProjectionSource baseSource;

        public LinkAllProjectionSource([NotNull] IProjectionSource baseSource)
        {
            this.baseSource = baseSource ?? throw new ArgumentNullException(nameof(baseSource));
        }

        public IEnumerable<IProjection> GetProjections()
        {
            var graph = new HashSet<IProjection>();

            foreach (var projection in this.baseSource.GetProjections())
            {
                this.FillProjectionsGraph(projection, graph);
            }

            return graph;
        }

        public void FillProjectionsGraph([NotNull] IProjection projection, [NotNull] HashSet<IProjection> graph)
        {
            if (projection == null) { throw new ArgumentNullException(nameof(projection)); }

            if (graph == null) { throw new ArgumentNullException(nameof(graph)); }

            if (graph.Add(projection))
            {
                foreach (var projectionProperty in projection.Properties)
                {
                    if (projectionProperty.Type.ElementProjection != null)
                    {
                        this.FillProjectionsGraph(projectionProperty.Type.ElementProjection, graph);
                    }
                }
            }
        }
    }
}
