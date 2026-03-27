using System.Runtime.Serialization;

using NHibernate;

namespace Framework.Database.NHibernate;

[Serializable]
public class HandledGenericAdoException : ADOException
{
    public string EntityName { get; private set; }
    public object EntityId { get; private set; }

    public HandledGenericAdoException()
    {

    }
    public HandledGenericAdoException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public HandledGenericAdoException(string message, Exception innerException, string sql, ValueTuple<string, object> unTypeObjectInfo) : base(message, innerException, sql)
    {
        this.EntityName = unTypeObjectInfo.Item1;
        this.EntityId = unTypeObjectInfo.Item2;
    }
    public HandledGenericAdoException(string message, Exception innerException, ValueTuple<string, object> unTypeObjectInfo) : base(message, innerException)
    {
        this.EntityName = unTypeObjectInfo.Item1;
        this.EntityId = unTypeObjectInfo.Item2;
    }

    public Microsoft.Data.SqlClient.SqlException SqlException
    {
        get { return (Microsoft.Data.SqlClient.SqlException) this.InnerException; }
    }
}
