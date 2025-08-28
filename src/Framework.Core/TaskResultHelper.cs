namespace Framework.Core;

public static class TaskResultHelper<TResult>
{
    public static readonly bool IsTask = typeof(Task).IsAssignableFrom(typeof(TResult));

    public static void TypeIsNotTaskValidate(string errorMessage = $"For Task result use EvaluateAsync method")
    {
        if (IsTask)
        {
            throw new Exception(errorMessage);
        }
    }
}
