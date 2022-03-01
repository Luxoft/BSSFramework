using Framework.DomainDriven.NHibernate;

using NHibernate.Dialect.Function;

namespace WorkflowSampleSystem.IntegrationTests.NH
{
    public class FullTextSearchMsSql2012Dialect : EnhancedMsSql2012Dialect
    {
        protected override void RegisterFunctions()
        {
            base.RegisterFunctions();
            this.RegisterFunction("contains", new StandardSQLFunction("contains", null));
        }
    }
}
