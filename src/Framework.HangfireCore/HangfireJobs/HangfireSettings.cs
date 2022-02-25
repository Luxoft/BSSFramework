namespace Framework.HangfireCore
{
    public class HangfireSettings
    {
        public string ConnectionString { get; set; }

        public JobTiming[] JobTimings { get; set; }
    }
}
