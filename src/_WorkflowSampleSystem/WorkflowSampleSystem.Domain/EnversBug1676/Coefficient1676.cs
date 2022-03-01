using Framework.Persistent;

namespace WorkflowSampleSystem.Domain.EnversBug1676
{
    public class Coefficient1676 : AuditPersistentDomainObjectBase
    {
        private Location1676 location;

        private decimal normCoefficient;

        public virtual Location1676 Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        public virtual decimal NormCoefficient
        {
            get { return this.normCoefficient; }
            set { this.normCoefficient = value; }
        }
    }
}
