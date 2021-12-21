namespace Framework.HangfireCore
{
    public class JobTiming
    {
        public string Name { get; set; }

        /// <summary>
        /// Cron. See https://en.wikipedia.org/wiki/Cron for details
        /// </summary>
        public string Schedule { get; set; }
    }
}
