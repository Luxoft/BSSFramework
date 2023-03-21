using SampleSystem.BLL.Core.Jobs;
using Serilog;

namespace SampleSystem.BLL.Jobs;

public class SampleJob : ISampleJob
{
    public void LogExecution() => Log.Information("Job executed");
}
