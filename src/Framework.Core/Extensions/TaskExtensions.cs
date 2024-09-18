namespace Framework.Core;

public static class TaskExtensions
{
    public static async Task<TResult[]> SyncWhenAll<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> getTask)
    {
        var res = new List<TResult>();

        foreach (var value in source)
        {
            res.AddRange(await getTask(value));
        }

        return res.ToArray();
    }
}
