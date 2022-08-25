using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;

namespace Automation.Utils.DatabaseUtils.Interfaces;

public interface IDatabaseContext
{
    public DatabaseItem Main { get; }

    public Dictionary<string, DatabaseItem> Secondary { get; }

    public Server Server { get; }
}