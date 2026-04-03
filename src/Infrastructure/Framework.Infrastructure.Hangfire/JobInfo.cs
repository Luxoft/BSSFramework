namespace Framework.Infrastructure.Hangfire;

public record JobInfo<TJob, TArg>(Func<TJob, TArg, Task> ExecuteActon);
