using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;

public interface IDatabaseContext
{
    public DatabaseItem Main { get; }

    public Dictionary<string, DatabaseItem> Secondary { get; }

    public Server Server { get; }
}
