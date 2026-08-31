using System.Text;

using Anch.Testing.Xunit;

using Framework.Core;
using Framework.Database;
using Framework.Database.ConnectionStringSource;
using Framework.Database.Domain;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.ServiceEnvironment.DependencyInjection;

[assembly: AnchTestFramework]

namespace SampleSystem.DbGenerate.EntityFramework;

public class DbGeneratorTest
{
    private const string ServerName = ".";

    private const string EfDatabaseName = "SampleSystem_ef_empty";

    private const string NhDatabaseName = "SampleSystem_nh_empty";

    private static readonly HashSet<string> DateTypeFamily = new(StringComparer.OrdinalIgnoreCase)
    {
        "date", "datetime", "datetime2", "datetimeoffset", "smalldatetime", "time",
    };

    private static readonly HashSet<string> PrimitiveTypeFamily = new(StringComparer.OrdinalIgnoreCase)
    {
        "bit", "tinyint", "smallint", "int", "bigint", "decimal", "numeric", "float", "real", "money", "smallmoney", "uniqueidentifier",
    };

    [AnchFact]
    public Task GenerateLocal(CancellationToken ct) => this.GenerateAllDb(@".", "SampleSystem_ef_empty", null, ct);

    [AnchFact]
    public async Task RegenerateEfAndDiffWithNHibernate(CancellationToken ct)
    {
        this.DropDatabaseIfExists(ServerName, EfDatabaseName);

        await this.GenerateAllDb(ServerName, EfDatabaseName, null, ct);

        var efColumns = GetColumns(ServerName, EfDatabaseName);
        var nhColumns = GetColumns(ServerName, NhDatabaseName);

        var report = BuildDiffReport(efColumns, nhColumns);

        Console.WriteLine(report);

        var reportPath = Path.Combine(Path.GetTempPath(), "ef_vs_nh_schema_diff.txt");
        await File.WriteAllTextAsync(reportPath, report, ct);
        Console.WriteLine($"Diff report written to {reportPath}");
    }

    private void DropDatabaseIfExists(string serverName, string databaseName)
    {
        var masterConnectionString = $"Data Source={serverName};Initial Catalog=master;Integrated Security=True;Application Name=SampleSystem;TrustServerCertificate=true";

        using var connection = new SqlConnection(masterConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = $"""
                                IF DB_ID('{databaseName}') IS NOT NULL
                                BEGIN
                                    ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                    DROP DATABASE [{databaseName}];
                                END
                                """;
        command.ExecuteNonQuery();
    }

    private static List<ColumnInfo> GetColumns(string serverName, string databaseName)
    {
        var connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True;Application Name=SampleSystem;TrustServerCertificate=true";

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
                               SELECT s.name AS SchemaName, t.name AS TableName, c.name AS ColumnName, ty.name AS TypeName, c.is_nullable AS IsNullable
                               FROM sys.columns c
                               JOIN sys.tables t ON c.object_id = t.object_id
                               JOIN sys.schemas s ON t.schema_id = s.schema_id
                               JOIN sys.types ty ON c.user_type_id = ty.user_type_id
                               ORDER BY s.name, t.name, c.name
                               """;

        var result = new List<ColumnInfo>();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new ColumnInfo(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetBoolean(4)));
        }

        return result;
    }

    private static string BuildDiffReport(List<ColumnInfo> efColumns, List<ColumnInfo> nhColumns)
    {
        var sb = new StringBuilder();

        var efTables = efColumns.GroupBy(c => $"{c.Schema}.{c.Table}", StringComparer.OrdinalIgnoreCase)
                                 .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var nhTables = nhColumns.GroupBy(c => $"{c.Schema}.{c.Table}", StringComparer.OrdinalIgnoreCase)
                                 .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var onlyInEf = efTables.Keys.Except(nhTables.Keys, StringComparer.OrdinalIgnoreCase).OrderBy(k => k).ToList();
        var onlyInNh = nhTables.Keys.Except(efTables.Keys, StringComparer.OrdinalIgnoreCase).OrderBy(k => k).ToList();
        var commonTables = efTables.Keys.Intersect(nhTables.Keys, StringComparer.OrdinalIgnoreCase).OrderBy(k => k).ToList();

        sb.AppendLine($"=== Tables only in EF ({onlyInEf.Count}) ===");
        foreach (var table in onlyInEf)
        {
            sb.AppendLine($"  - {table}");
        }

        sb.AppendLine();
        sb.AppendLine($"=== Tables only in NH ({onlyInNh.Count}) ===");
        foreach (var table in onlyInNh)
        {
            sb.AppendLine($"  - {table}");
        }

        sb.AppendLine();
        sb.AppendLine("=== Common tables with differences (dates and nullability of primitive types are ignored) ===");

        foreach (var table in commonTables)
        {
            var efCols = efTables[table].ToDictionary(c => c.Column, StringComparer.OrdinalIgnoreCase);
            var nhCols = nhTables[table].ToDictionary(c => c.Column, StringComparer.OrdinalIgnoreCase);

            var columnsOnlyInEf = efCols.Keys.Except(nhCols.Keys, StringComparer.OrdinalIgnoreCase).OrderBy(k => k).ToList();
            var columnsOnlyInNh = nhCols.Keys.Except(efCols.Keys, StringComparer.OrdinalIgnoreCase).OrderBy(k => k).ToList();
            var commonColumns = efCols.Keys.Intersect(nhCols.Keys, StringComparer.OrdinalIgnoreCase).OrderBy(k => k).ToList();

            var typeMismatches = new List<string>();
            var nullableMismatches = new List<string>();

            foreach (var column in commonColumns)
            {
                var ef = efCols[column];
                var nh = nhCols[column];

                var bothDates = DateTypeFamily.Contains(ef.TypeName) && DateTypeFamily.Contains(nh.TypeName);
                var bothPrimitives = PrimitiveTypeFamily.Contains(ef.TypeName) && PrimitiveTypeFamily.Contains(nh.TypeName);

                if (!bothDates && !string.Equals(ef.TypeName, nh.TypeName, StringComparison.OrdinalIgnoreCase))
                {
                    typeMismatches.Add($"{column}: ef={ef.TypeName} nh={nh.TypeName}");
                }

                if (!bothPrimitives && ef.IsNullable != nh.IsNullable)
                {
                    nullableMismatches.Add($"{column}: ef.nullable={ef.IsNullable} nh.nullable={nh.IsNullable}");
                }
            }

            if (columnsOnlyInEf.Count == 0 && columnsOnlyInNh.Count == 0 && typeMismatches.Count == 0 && nullableMismatches.Count == 0)
            {
                continue;
            }

            sb.AppendLine();
            sb.AppendLine($"--- {table} ---");

            if (columnsOnlyInEf.Count > 0)
            {
                sb.AppendLine($"  Columns only in EF ({columnsOnlyInEf.Count}): {string.Join(", ", columnsOnlyInEf)}");
            }

            if (columnsOnlyInNh.Count > 0)
            {
                sb.AppendLine($"  Columns only in NH ({columnsOnlyInNh.Count}): {string.Join(", ", columnsOnlyInNh)}");
            }

            foreach (var mismatch in typeMismatches)
            {
                sb.AppendLine($"  Type mismatch: {mismatch}");
            }

            foreach (var mismatch in nullableMismatches)
            {
                sb.AppendLine($"  Nullable mismatch: {mismatch}");
            }
        }

        return sb.ToString();
    }

    private sealed record ColumnInfo(string Schema, string Table, string Column, string TypeName, bool IsNullable);

    public async Task GenerateDatabase(DbGenerationOptions options)
    {
        var ct = TestContext.Current.CancellationToken;

        if (string.IsNullOrWhiteSpace(options.Server))
        {
            throw new ArgumentException("Server name is empty");
        }

        if (string.IsNullOrWhiteSpace(options.DataBase))
        {
            throw new ArgumentException("DataBase name is empty");
        }

        Console.WriteLine($"Generate database:'{options.DataBase}' on {options.Server}");

        await this.GenerateAllDb(options.Server, options.DataBase, null, ct);
    }

    public async Task GenerateAllDb(string serverName, string mainDatabaseName, DbUserCredential? credentials, CancellationToken ct)
    {
        var credStr = credentials == null ? "Integrated Security=True" : $"User ID={credentials.UserName};Password={credentials.Password}";

        var connectionString =
            $"Data Source={serverName};Initial Catalog={mainDatabaseName};{credStr};Application Name=SampleSystem;TrustServerCertificate=true";

        var rootServiceProvider = new ServiceCollection()
                                  .AddSingleton<IDefaultConnectionStringSource>(new ManualDefaultConnectionStringSource(connectionString))
                                  .AddSingleton(DBSessionSettings.Default)
                                  .Self(new SampleSystemEntityFrameworkExtension().AddServices)
                                  .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        await using var scope = rootServiceProvider.CreateAsyncScope();

        await using var dbContext = scope.ServiceProvider.GetRequiredService<SampleSystemDbContext>();
        await dbContext.Database.EnsureCreatedAsync(ct);

        new BssFluentMigrator(connectionString).Migrate();
    }
}
