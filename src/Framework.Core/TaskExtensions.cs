namespace Framework.Core;

public static class TaskExtensions
{
    public static Func<Task<object?>> ToDefaultTask(this Func<Task> func) =>
        async () =>
        {
            await func();
            return null;
        };

    public static Func<T1, Task<object?>> ToDefaultTask<T1>(this Func<T1, Task> func) =>
        async arg =>
        {
            await func(arg);
            return null;
        };

    public static Func<T1, T2, Task<object?>> ToDefaultTask<T1, T2>(this Func<T1, T2, Task> func) =>
        async (arg1, arg2) =>
        {
            await func(arg1, arg2);
            return null;
        };
}
