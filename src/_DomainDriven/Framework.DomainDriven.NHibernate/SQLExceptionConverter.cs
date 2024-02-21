using NHibernate;
using NHibernate.Exceptions;

namespace Framework.DomainDriven.NHibernate;

public class SQLExceptionConverter : ISQLExceptionConverter
{
    public SQLExceptionConverter(IViolatedConstraintNameExtracter extracter)
    {
    }

    #region ISQLExceptionConverter Members

    public Exception Convert(AdoExceptionContextInfo exceptionInfo)
    {
        /*
         * So far I know we don't have something similar to "X/Open-compliant SQLState" in .NET
         * This mean that each Dialect must have its own ISQLExceptionConverter, overriding BuildSQLExceptionConverter method,
         * and its own IViolatedConstraintNameExtracter if needed.
         * The System.Data.Common.DbException, of .NET2.0, don't give us something applicable to all dialects.
         */
        return HandledNonSpecificException(exceptionInfo.SqlException, exceptionInfo.Message, exceptionInfo.Sql,
                                           ValueTuple.Create(exceptionInfo.EntityName, exceptionInfo.EntityId));
    }

    #endregion

    /// <summary> Handle an exception not converted to a specific type based on the SQLState. </summary>
    /// <param name="sqlException">The exception to be handled. </param>
    /// <param name="message">An optional message </param>
    /// <param name="sql">Optionally, the sql being performed when the exception occurred. </param>
    /// <param name="create"> </param>
    /// <returns> The converted exception; should <b>never</b> be null. </returns>
    public static ADOException HandledNonSpecificException(Exception sqlException, string message, string sql,
                                                           ValueTuple<string, object> unTypedObjectInfo)
    {
        return new HandledGenericADOException(message, sqlException, sql, unTypedObjectInfo);
    }
}
