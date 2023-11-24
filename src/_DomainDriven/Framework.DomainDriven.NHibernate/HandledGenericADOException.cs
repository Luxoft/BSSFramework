using System.Runtime.Serialization;
using Framework.Core;
using NHibernate;

namespace Framework.DomainDriven.NHibernate;

[Serializable]
public class HandledGenericADOException : ADOException
{
    public string EntityName { get; private set; }
    public object EntityId { get; private set; }

    public HandledGenericADOException()
    {

    }
    public HandledGenericADOException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public HandledGenericADOException(string message, Exception innerException, string sql, ValueTuple<string, object> unTypeObjectInfo) : base(message, innerException, sql)
    {
        this.EntityName = unTypeObjectInfo.Item1;
        this.EntityId = unTypeObjectInfo.Item2;
    }
    public HandledGenericADOException(string message, Exception innerException, ValueTuple<string, object> unTypeObjectInfo) : base(message, innerException)
    {
        this.EntityName = unTypeObjectInfo.Item1;
        this.EntityId = unTypeObjectInfo.Item2;
    }

    public Microsoft.Data.SqlClient.SqlException SqlException
    {
        get { return (Microsoft.Data.SqlClient.SqlException) this.InnerException; }
    }
}
