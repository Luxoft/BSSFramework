using Framework.DomainDriven.NHibernate;

using NHibernate.Dialect.Function;

namespace AttachmentsSampleSystem.IntegrationTests.NH
{
    public class FullTextSearchMsSql2008Dialect : EnhancedMsSql2008Dialect
    {
        protected override void RegisterFunctions()
        {
            base.RegisterFunctions();
            this.RegisterFunction("contains", new StandardSQLFunction("contains", null));
        }
    }
}
