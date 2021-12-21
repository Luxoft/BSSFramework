using System.Dynamic;

namespace Framework.DomainDriven.SerializeMetadata.Tests.Unit
{
    public class TestDomainObj : PersistentDomainObjectBase
    {
        public int Prop1 { get; set; }

        public string Prop2 { get; set; }

        public string Prop3 { get; set; }

        public TestDomainObjProperty1 Property1 { get; set; }

        public TestDomainObjProperty2 Property2 => this.Property1.Property;
    }
}
