using Framework.DomainDriven.Attributes;
using Framework.Persistent.Mapping;

namespace SampleSystem.Domain.TestForceAbstract
{
    [Table(Name = nameof(ClassA))]
    [InlineBaseTypeMapping]
    [NotAuditedClass]
    public class ConcreteClassA : ClassA
    {
        private int age;

        public virtual int Age
        {
            get { return this.age; }
            set { this.age = value; }
        }

    }
}
