namespace Anch.Testing.Database.Mssql;

public record MssqlDatabaseSettings
{
    public string BackupPath { get; init; } = "";

    public string DatabaseCollation { get; init; } = "";

    public string[] SecondaryDatabases { get; init; } = [];
}
