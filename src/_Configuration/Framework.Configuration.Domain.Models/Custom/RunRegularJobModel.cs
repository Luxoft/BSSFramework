namespace Framework.Configuration.Domain
{
    public class RunRegularJobModel : DomainObjectBase
    {
        public RunRegularJobModel()
        {
        }

        public RunRegularJobModel(RegularJob regularJob, RunRegularJobMode mode, string instanceServerName)
        {
            this.RegularJob = regularJob;
            this.Mode = mode;
            this.InstanceServerName = instanceServerName;
        }

        public RegularJob RegularJob { get; set; }

        public RunRegularJobMode Mode { get; set; }

        public string InstanceServerName { get; set; }
    }
}
