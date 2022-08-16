using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;

namespace Automation.Utils.DatabaseUtils.Interfaces
{
    public interface IDatabaseContext
    {
        public DatabaseItem MainDatabase { get; }

        public Dictionary<string, DatabaseItem> SecondaryDatabases { get; }

        public Server Server { get; }

        public void Dispose();
    }
}