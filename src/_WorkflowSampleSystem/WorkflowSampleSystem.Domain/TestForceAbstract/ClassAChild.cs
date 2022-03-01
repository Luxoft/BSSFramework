namespace WorkflowSampleSystem.Domain.TestForceAbstract
{
    public class ClassAChild : PersistentDomainObjectBase
    {
        private ClassA parent;

        private bool isFake;

        public virtual ClassA Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        public virtual bool IsFake
        {
            get { return this.isFake; }
            set { this.isFake = value; }
        }
    }
}
