using System.Linq;
using Framework.HangfireCore;
using Framework.NotificationCore.Jobs;
using Hangfire;
using WorkflowSampleSystem.BLL.Core.Jobs;

namespace WorkflowSampleSystem.WebApiCore
{
    public class JobList
    {
        public static void RunAll(JobTiming[] timings)
        {
            RecurringJob.AddOrUpdate<ISampleJob>(
                x => x.LogExecution(),
                timings.First(x => x.Name == "DemoHangfireJob").Schedule);

            RecurringJob.AddOrUpdate<ISendNotificationsJob>(x => x.Send(), Cron.Hourly);
        }
    }
}
