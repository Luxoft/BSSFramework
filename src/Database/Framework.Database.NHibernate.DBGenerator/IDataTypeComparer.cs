using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.NHibernate.DBGenerator;

public interface IDataTypeComparer
{
    bool Equals(DataType x, DataType y);
}
