namespace Framework.Projection.Lambda.ProjectionSource;

internal class LinkAllProjectionSource(IProjectionSource baseSource) : IProjectionSource
{
    public IEnumerable<IProjection> GetProjections()
    {
        var graph = new HashSet<IProjection>();

        foreach (var projection in baseSource.GetProjections())
        {
            this.FillProjectionsGraph(projection, graph);
        }

        return graph;
    }

    public void FillProjectionsGraph(IProjection projection, HashSet<IProjection> graph)
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
