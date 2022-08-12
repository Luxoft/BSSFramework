using NHibernate;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;

namespace Framework.DomainDriven.NHibernate;

/// <summary>
/// Represents extended dialect derived from MsSql2012Dialect. Defines nHibernate extensions that works with dates
/// </summary>
public class EnhancedMsSql2012Dialect : MsSql2012Dialect
{
    /// <summary>
    /// Registers all our custom functions and defines corresponding MS SQL functions
    /// </summary>
    protected override void RegisterFunctions()
    {
        base.RegisterFunctions();

        this.RegisterFunction("AddDays", new SQLFunctionTemplate(NHibernateUtil.DateTime, "dateadd(day,?2,?1)"));
        this.RegisterFunction("AddHours", new SQLFunctionTemplate(NHibernateUtil.DateTime, "dateadd(hour,?2,?1)"));
        this.RegisterFunction("AddMonths", new SQLFunctionTemplate(NHibernateUtil.DateTime, "dateadd(month,?2,?1)"));
        this.RegisterFunction("AddYears", new SQLFunctionTemplate(NHibernateUtil.DateTime, "dateadd(year,?2,?1)"));
        this.RegisterFunction("DiffDays", new SQLFunctionTemplate(NHibernateUtil.Int32, "DATEDIFF(day,?1,?2)"));
    }
}
