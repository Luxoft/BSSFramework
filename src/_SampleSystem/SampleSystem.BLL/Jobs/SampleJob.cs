using Microsoft.Extensions.Logging;

using SampleSystem.BLL.Core.Jobs;

namespace SampleSystem.BLL.Jobs;

public class SampleJob(ILogger<SampleJob> logger) : ISampleJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Job executed");
    }
}
