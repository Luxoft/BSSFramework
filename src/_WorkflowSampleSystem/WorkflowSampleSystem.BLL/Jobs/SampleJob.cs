using WorkflowSampleSystem.BLL.Core.Jobs;
using Serilog;

namespace WorkflowSampleSystem.BLL.Jobs
{
    public class SampleJob : ISampleJob
    {
        public void LogExecution() => Log.Information("Job executed");
    }
}
