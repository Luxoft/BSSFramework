using Framework.Core;

namespace Framework.HangfireCore;

public class JobNameExtractPolicy : IJobNameExtractPolicy
{
    public string GetName(Type jobType)
    {
        return jobType.Name.Pipe(jobType.IsInterface, v => v.Skip("I", true)).SkipLast("Job");
    }

    public string GetDisplayName(Type jobType) => jobType.Name;
}
