using System;
using System.IO;
using System.Linq;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.Data.SqlClient;

namespace Automation.Utils.DatabaseUtils;

public class DatabaseItem : IDatabaseItem
{
    private SqlConnectionStringBuilder builder;
    private ConfigUtil configUtil;

    public DatabaseItem(
        ConfigUtil configUtil,
        string connectionString, string initialCatalog = null)
    {
        this.configUtil = configUtil;
        this.builder = new SqlConnectionStringBuilder(connectionString);
        initialCatalog ??= this.builder.InitialCatalog;
        this.DatabaseName = this.configUtil.RandomizeDatabaseName
            ? $"{initialCatalog}{TextRandomizer.RandomString(5)}"
            : initialCatalog;

        var fileName = $"{this.InstanceName}_{this.DatabaseName}_{TextRandomizer.RandomString(5)}";

        this.CopyDataPath = ToCopyDataPath(initialCatalog);
        this.CopyLogPath = ToCopyLogPath(initialCatalog);
        this.SourceDataPath = ToSourceDataPath(fileName);
        this.SourceLogPath = ToSourceLogPath(fileName);
        this.builder.InitialCatalog = this.DatabaseName;
    }

    public string DataSource => this.builder.DataSource;

    public string InitialCatalog => this.builder.InitialCatalog;

    public string UserId => this.builder.UserID;

    public string Password => this.builder.Password;

    public bool IntegratedSecurity => this.builder.IntegratedSecurity;

    public string ConnectionString => this.builder.ConnectionString;

    public string InstanceName => this.builder.DataSource.Split('\\').LastOrDefault();

    public string DatabaseName { get; }
    public string CopyDataPath { get; }
    public string CopyLogPath { get; }
    public string SourceDataPath { get; }
    public string SourceLogPath { get; }

    private string ToSourceDataPath(string fileName) => ToWorkPath(SourceDataFile(fileName));

    private string ToSourceLogPath(string fileName) => ToWorkPath(SourceLogFile(fileName));

    private string ToCopyDataPath(string initialCatalog) => ToWorkPath(CopyDataFile(initialCatalog));

    private string ToCopyLogPath(string initialCatalog) => ToWorkPath(CopyLogFile(initialCatalog));

    private string CopyDataFile(string initialCatalog) =>$"{this.configUtil.SystemName}_{Environment.UserName}_{initialCatalog}.mdf";

    private string CopyLogFile(string initialCatalog) => $"{this.configUtil.SystemName}_{Environment.UserName}_{initialCatalog}_log.ldf";

    private static string SourceDataFile(string fileName) => $"{fileName}.mdf";

    private static string SourceLogFile(string fileName) => $"{fileName}_log.ldf";

    private string ToWorkPath(string fileName) => Path.Combine(this.configUtil.DbDataDirectory, fileName);
}