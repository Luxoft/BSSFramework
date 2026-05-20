using System.Data;
using System.Text.RegularExpressions;

using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Data.SqlClient;

namespace Framework.AutomationCore.Extensions;

public static class ExecuteSqlExtensions
{
    public static async Task ExecuteSqlAsync(this TestConnectionString connectionString, string sqlFileOrText, CancellationToken ct)
    {
        await using var connection = new SqlConnection(connectionString.Value);

        await connection.OpenAsync(ct);

        await connection.ExecuteSqlAsync(sqlFileOrText, ct);

        await connection.CloseAsync();
    }

    public static async Task ExecuteSqlAsync(this SqlConnection connection, string sqlFileOrText, CancellationToken ct)
    {
        var sql = await sqlFileOrText.GetSqlTextAsync(ct);

        var regex = new Regex("^GO(\r\n|\n|\r)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        var lines = regex.Split(sql).Select(z => z.Replace("$Database", connection.Database)).ToArray();

        await using var cmd = connection.CreateCommand();
        cmd.Connection = connection;

        foreach (var line in lines)
        {
            if (line.Length > 0)
            {
                cmd.CommandText = line;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;

                await cmd.ExecuteNonQueryAsync(ct);
            }
        }
    }

    public static async Task<string> GetSqlTextAsync(this string sqlFileOrText, CancellationToken ct)
    {
        if (sqlFileOrText.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase))
        {
            await using var stream = File.OpenRead(sqlFileOrText);
            var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync(ct);
        }
        else
        {
            return sqlFileOrText;
        }
    }

    public static async Task ExecuteSqlFromFolderAsync(this TestConnectionString connectionString, string folder, CancellationToken ct = default)
    {
        string[] filePaths;

        if (!Directory.Exists(folder))
        {
            Console.WriteLine("No directory found for path: {0}", folder);
            return;
        }

        try
        {
            filePaths = Directory.GetFiles(folder);
        }
        catch (Exception)
        {
            return;
        }

        var builder = new SqlConnectionStringBuilder(connectionString.Value);

        await ExecuteSqlScriptsAsync(filePaths, builder, ct);
    }

    private static async Task ExecuteSqlScriptsAsync(string[] sqlPaths, SqlConnectionStringBuilder connectionBuilder, CancellationToken ct)
    {
        await using var connection = new SqlConnection(connectionBuilder.ConnectionString);

        await connection.OpenAsync(ct);

        await connection.ExecuteSqlAsync("EXEC sp_msforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"", ct);

        foreach (var filePath in sqlPaths)
        {
            try
            {
                await connection.ExecuteSqlAsync(filePath, ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(filePath);

                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                Console.WriteLine();
                throw new Exception($"file: {filePath}: ", ex);
            }
        }

        await connection.ExecuteSqlAsync("exec sp_msforeachtable \"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\"", ct);

        await connection.CloseAsync();
    }
}

