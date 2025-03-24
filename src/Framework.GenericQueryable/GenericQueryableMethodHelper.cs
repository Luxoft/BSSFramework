using System.Reflection;

namespace Framework.GenericQueryable;

public static class GenericQueryableMethodHelper
{
    public static readonly MethodInfo WithFetchMethod =
        new Func<string, IQueryable<object>>(Array.Empty<object>().AsQueryable().WithFetch).Method.GetGenericMethodDefinition();
}
