using Anch.Core;

using Framework.Core;

namespace Framework.Infrastructure.Hangfire;

public class JobNameExtractPolicy : IJobNameExtractPolicy
{
    public string GetName(Type jobType) => jobType.Name.Pipe(jobType.IsInterface, v => v.Skip("I", true)).SkipLast("Job");

    public string GetDisplayName(Type jobType) => jobType.Name;
}
