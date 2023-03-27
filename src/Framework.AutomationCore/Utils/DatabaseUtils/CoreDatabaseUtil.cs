using System.Collections.Specialized;
using System.Data;
using System.Text.RegularExpressions;
using Automation.Utils.DatabaseUtils.Interfaces;
using MartinCostello.SqlLocalDb;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using MoreLinq;

namespace Automation.Utils.DatabaseUtils;

public static partial class CoreDatabaseUtil
{
    public static void ExecuteSqlFromFolder(string connectionString, string folder, string initialCatalog = null)
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

        var builder = new SqlConnectionStringBuilder(connectionString);

        if (initialCatalog != null)
        {
            builder.InitialCatalog = initialCatalog;
        }

        ExecuteSqlScripts(filePaths, builder);
    }

    private static void ExecuteSqlScripts(string[] sqlPaths, SqlConnectionStringBuilder connectionBuilder)
    {
        using (var connection = new SqlConnection(connectionBuilder.ConnectionString))
        {
            connection.Open();

            ExecuteSql(connection, "EXEC sp_msforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"");

            foreach (var filePath in sqlPaths)
            {
                try
                {
                    ExecuteSql(connection, filePath);
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

            ExecuteSql(connection, "exec sp_msforeachtable \"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\"");
            connection.Close();
        }
    }

    public static void ExecuteSql(SqlConnection connection, string sqlFileOrText)
    {
        string sql;

        if (sqlFileOrText.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase))
        {
            using (var stream = File.OpenRead(sqlFileOrText))
            {
                var reader = new StreamReader(stream);
                sql = reader.ReadToEnd();
            }
        }
        else
        {
            sql = sqlFileOrText;
        }

        var regex = new Regex("^GO(\r\n|\n|\r)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        string[] lines = regex.Split(sql).Select(z => z.Replace("$Database", connection.Database)).ToArray();

        using (var cmd = connection.CreateCommand())
        {
            cmd.Connection = connection;

            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                    cmd.CommandText = line;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public static void ExecuteSql(string connectionString, string sqlFileOrText, string initialCatalog = null)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);

        if (initialCatalog != null)
        {
            builder.InitialCatalog = initialCatalog;
        }

        using (var connection = new SqlConnection(builder.ConnectionString))
        {
            connection.Open();

            ExecuteSql(connection, sqlFileOrText);
            connection.Close();
        }
    }

    public static void Drop(this IDatabaseContext databaseContext)
    {
        Drop(databaseContext.Server, databaseContext.Main.DatabaseName);
        databaseContext.Secondary?.ForEach(x => Drop(databaseContext.Server, x.Value.DatabaseName));
    }

    private static void Drop(Server server, string databaseName)
    {
        var database = server.GetDatabase(databaseName);

        if (database == null)
        {
            return;
        }

        server.Drop(database);
    }


    public static void Drop(this Server server, Database database)
    {
        try
        {
            server.SetModeRestrictedUser(database.Name);
            database.Drop();
        }
        catch (FailedOperationException)
        {
            try
            {
                server.KillDatabase(database.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        catch (Exception ex)
        {
            SetModeMultiUser(server, database);
            Console.WriteLine(ex.Message);
        }
    }

    public static void ReCreate(this IDatabaseContext databaseContext)
    {
        databaseContext.Drop();
        databaseContext.Create();
    }

    public static void Create(this IDatabaseContext databaseContext)
    {
        Create(databaseContext.Server, databaseContext.Main);
        databaseContext.Secondary?.ForEach(x => Create(databaseContext.Server, x.Value));
    }

    private static void Create(Server server, IDatabaseItem database)
    {
        var db = new Database(server, database.DatabaseName);
        if (!string.IsNullOrEmpty(database.DatabaseCollation))
        {
            db.Collation = database.DatabaseCollation;
        }

        var fileGroup = new FileGroup(db, "PRIMARY");

        var dataFile = new DataFile(
            fileGroup,
            database.DatabaseName,
            database.SourceDataPath)
        {
            Size = 5.5 * 1024.0, GrowthType = FileGrowthType.KB, Growth = 1.0 * 1024.0
        };

        fileGroup.Files.Add(dataFile);

        db.FileGroups.Add(fileGroup);

        var logFile = new LogFile(
            db,
            database.DatabaseName + "_log",
            database.SourceLogPath)
        {
            Size = 1.0 * 1024.0, GrowthType = FileGrowthType.Percent, Growth = 10.0
        };

        db.LogFiles.Add(logFile);

        db.Create();

        server.Refresh();
    }

    public static void CopyDetachedFiles(this IDatabaseContext databaseContext)
    {
        CopyDetachedFiles(databaseContext.Server, databaseContext.Main);
        databaseContext.Secondary?.ForEach(x => CopyDetachedFiles(databaseContext.Server, x.Value));
    }

    private static void CopyDetachedFiles(Server server, DatabaseItem database)
    {
        server.DetachDatabase(database.DatabaseName);

        new FileInfo(database.SourceDataPath).MoveTo(database.CopyDataPath, true);
        new FileInfo(database.SourceLogPath).MoveTo(database.CopyLogPath, true);
    }

    public static void AttachDatabase(this IDatabaseContext databaseContext)
    {
        AttachDatabase(databaseContext.Server, databaseContext.Main);
        databaseContext.Secondary?.ForEach(x => AttachDatabase(databaseContext.Server, x.Value));
    }

    private static void AttachDatabase(Server server, IDatabaseItem database)
    {
        new FileInfo(database.CopyDataPath).CopyTo(database.SourceDataPath, true);
        new FileInfo(database.CopyLogPath).CopyTo(database.SourceLogPath, true);

        server.AttachDatabase(database.DatabaseName, new StringCollection { database.SourceDataPath, database.SourceLogPath });
    }

    public static void DetachDatabase(this Server server, string databaseName)
    {
        server.SetModeRestrictedUser(databaseName);

        server.DetachDatabase(databaseName, false);
    }

    public static void CreateLocalDb(string instanceName)
    {
        using (var localDb = new SqlLocalDbApi())
        {
            var instanceInfo = localDb.GetInstanceInfo(instanceName);
            localDb.AutomaticallyDeleteInstanceFiles = true;

            if (!instanceInfo.Exists)
            {
                localDb.CreateInstance(instanceInfo.Name);
            }

            if (!instanceInfo.IsRunning)
            {
                localDb.StartInstance(instanceInfo.Name);
            }
        }
    }

    public static void DeleteLocalDb(string instanceName)
    {
        using (var localDb = new SqlLocalDbApi())
        {
            var instanceInfo = localDb.GetInstanceInfo(instanceName);

            if (instanceInfo.IsRunning)
            {
                localDb.StopInstance(instanceInfo.Name);
            }

            localDb.DeleteInstance(instanceName);
        }
    }

    public static bool LocalDbInstanceExists(string instanceName)
    {
        using (var localDbApi = new SqlLocalDbApi())
        {
            var instanceInfo = localDbApi.GetInstanceInfo(instanceName);

            return instanceInfo.Exists;
        }
    }
}
