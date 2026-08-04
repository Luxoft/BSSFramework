using Microsoft.SqlServer.Management.Smo;

namespace Anch.Testing.Database.Mssql;

public interface ISqlServerFactory
{
    Server Create();
}
