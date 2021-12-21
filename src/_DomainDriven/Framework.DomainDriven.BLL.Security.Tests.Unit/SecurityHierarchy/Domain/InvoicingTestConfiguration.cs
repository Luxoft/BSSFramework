namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain
{
    using Framework.DomainDriven.UnitTest.Mock;

    public class InvoicingTestConfiguration : BLLContextConfiguration<TestBLLContext, PersistentDomainObjectBase>
    {
        public InvoicingTestConfiguration() : base(new[]{typeof(HierarchyObject).Assembly})
        {

        }

        protected override void Initialize<T>(T result)
        {

        }
    }
}