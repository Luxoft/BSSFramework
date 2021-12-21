using System.Collections.Generic;

using NHibernate;
using NHibernate.Dialect.Function;

namespace Framework.DomainDriven.NHibernate
{
    internal static class SQLFunctionDescriptorStore
    {
        private static readonly Dictionary<string, SQLFunctionTemplate> Store = new Dictionary<string, SQLFunctionTemplate>
        {
            { "AddDays", new SQLFunctionTemplate(NHibernateUtil.DateTime, "dateadd(day,?2,?1)") },
            { "AddHours", new SQLFunctionTemplate(NHibernateUtil.DateTime, "dateadd(hour,?2,?1)") },
            { "AddMonths", new SQLFunctionTemplate(NHibernateUtil.DateTime, "dateadd(month,?2,?1)") },
            { "AddYears", new SQLFunctionTemplate(NHibernateUtil.DateTime, "dateadd(year,?2,?1)") }
        };

        public static IReadOnlyDictionary<string, SQLFunctionTemplate> Descriptors => Store;
    }
}