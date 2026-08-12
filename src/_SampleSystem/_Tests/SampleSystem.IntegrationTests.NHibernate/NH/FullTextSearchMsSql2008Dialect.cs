using Framework.Database.NHibernate.Dialect;

using NHibernate.Dialect.Function;

namespace SampleSystem.IntegrationTests.NH;

[Obsolete("Use FullTextSearchMsSql2012Dialect")] // 17.1
public class FullTextSearchMsSql2008Dialect : EnhancedMsSql2008Dialect
{
    protected override void RegisterFunctions()
    {
        base.RegisterFunctions();
        this.RegisterFunction("contains", new StandardSQLFunction("contains", null));
    }
}
