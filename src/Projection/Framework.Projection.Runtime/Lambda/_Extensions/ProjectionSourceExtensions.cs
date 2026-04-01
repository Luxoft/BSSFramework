using Framework.Projection.Lambda.ProjectionSource._Base;

namespace Framework.Projection.Lambda._Extensions;

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

    private class CompositeProjectionSource(IEnumerable<IProjectionSource> sources) : IProjectionSource
    {
        private readonly IReadOnlyCollection<IProjectionSource> sources = (sources ?? throw new ArgumentNullException(nameof(sources))).ToArray();

        public IEnumerable<IProjection> GetProjections() => this.sources.SelectMany(p => p.GetProjections());
    }
}
