namespace Framework.HangfireCore;

public record JobInfo<TJob, TArg>(Func<TJob, TArg, Task> ExecuteActon);
