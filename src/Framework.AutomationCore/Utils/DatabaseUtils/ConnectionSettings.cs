using System;
using System.Linq;

using Microsoft.Data.SqlClient;

namespace Automation.Utils.Utils.DatabaseUtils
{
    public class ConnectionSettings
    {
        private readonly SqlConnectionStringBuilder builder;

        public ConnectionSettings()
        {
            this.builder = new SqlConnectionStringBuilder(AppSettings.Default["ConnectionStrings"]);

            if (ConfigUtil.UseLocalDb)
            {
                this.SetLocalDbInstance(TextRandomizer.UniqueString(ConfigUtil.SystemName));
            }

            ConfigUtil.ConnectionString = this.builder.ConnectionString;
            ConfigUtil.InstanceName = this.InstanceName;
        }

        public string DataSource
        {
            get => this.builder.DataSource;
            set => this.builder.DataSource = value;
        }

        public string InitialCatalog
        {
            get => this.builder.InitialCatalog;
            set => this.builder.InitialCatalog = value;
        }

        public string UserId => this.builder.UserID;

        public string Password => this.builder.Password;

        public bool IntegratedSecurity => this.builder.IntegratedSecurity;

        public string ConnectionString => this.builder.ConnectionString;

        public string InstanceName => this.builder.DataSource.Split('\\').LastOrDefault();

        public bool IsLocalDb => this.builder.DataSource.StartsWith("(localdb)", StringComparison.InvariantCultureIgnoreCase);

        public void SetLocalDbInstance(string instanceName) => this.builder.DataSource = $"(localdb)\\{instanceName}";
    }
}
