namespace Framework.HangfireCore.JobServices;

public interface IScopedJobExecutor
{
    Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> executedTask);
}
