using Framework.Core;

namespace WorkflowSampleSystem.Domain
{
    public class TestObjForNestedBase : BaseDirectory
    {
        private Period period;

        public virtual Period Period
        {
            get => this.period;
            set => this.period = value;
        }
    }
}
