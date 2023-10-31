﻿using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.Data.SqlClient;

namespace Automation.Utils.DatabaseUtils;

public class DatabaseItem : IDatabaseItem
{
    private readonly SqlConnectionStringBuilder builder;

    private readonly string dbDataDirectory;

    public DatabaseItem(
        string connectionString,
        string databaseCollation,
        string dbDataDirectory,
        string initialCatalog = null,
        bool randomizeDatabaseName = false)
    {
        this.dbDataDirectory = dbDataDirectory;
        this.builder = new SqlConnectionStringBuilder(connectionString);
        initialCatalog ??= this.builder.InitialCatalog;
        this.DatabaseName = randomizeDatabaseName
            ? $"{initialCatalog}{TextRandomizer.RandomString(5)}"
            : initialCatalog;

        var fileName = $"{this.InstanceName}_{this.DatabaseName}_{TextRandomizer.RandomString(5)}";

        this.CopyDataPath = this.ToCopyDataPath(initialCatalog);
        this.CopyLogPath = this.ToCopyLogPath(initialCatalog);
        this.SourceDataPath = this.ToSourceDataPath(fileName);
        this.SourceLogPath = this.ToSourceLogPath(fileName);
        this.DatabaseCollation = databaseCollation;
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
    public string DatabaseCollation { get; }

    private string ToSourceDataPath(string fileName) => this.ToWorkPath(SourceDataFile(fileName));

    private string ToSourceLogPath(string fileName) => this.ToWorkPath(SourceLogFile(fileName));

    private string ToCopyDataPath(string initialCatalog) => this.ToWorkPath(this.CopyDataFile(initialCatalog));

    private string ToCopyLogPath(string initialCatalog) => this.ToWorkPath(this.CopyLogFile(initialCatalog));

    private string CopyDataFile(string initialCatalog) =>$"{Environment.UserName}_{initialCatalog}.mdf";

    private string CopyLogFile(string initialCatalog) => $"{Environment.UserName}_{initialCatalog}_log.ldf";

    private static string SourceDataFile(string fileName) => $"{fileName}.mdf";

    private static string SourceLogFile(string fileName) => $"{fileName}_log.ldf";

    private string ToWorkPath(string fileName) => Path.Combine(this.dbDataDirectory, fileName);
}
