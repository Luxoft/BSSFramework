using CommonFramework;

namespace Framework.Projection.Lambda.Extensions;

internal static class ProjectionExtensions
{
    public static T? TryGetSecurityProjection<T>(this IEnumerable<T> source, Type arg)
        where T : IProjection =>
        source.TryGetProjectionByRole(arg, ProjectionRole.SecurityNode);

    public static T? TryGetProjectionByRole<T>(this IEnumerable<T> source, Type sourceType, ProjectionRole role)
        where T : IProjection =>
        source.SingleOrDefault(projection => projection.SourceType == sourceType && projection.Role == role);

    public static IEnumerable<IProjection> GetAllProjections(this IEnumerable<IProjection> projections)
    {
        var allProjections = new HashSet<IProjection>();

        projections.Foreach(p => p.FillProjectionList(allProjections));

        return allProjections;
    }

    private static void FillProjectionList(this IProjection projection, HashSet<IProjection> allProjections)
    {
        if (allProjections.Add(projection))
        {
            foreach (var prop in projection.Properties)
            {
                prop.Type.ElementProjection?.FillProjectionList(allProjections);
            }
        }
    }
}
