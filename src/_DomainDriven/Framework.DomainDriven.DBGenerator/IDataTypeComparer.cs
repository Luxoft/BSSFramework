using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator;

public interface IDataTypeComparer
{
    bool Equals(DataType x, DataType y);
}
