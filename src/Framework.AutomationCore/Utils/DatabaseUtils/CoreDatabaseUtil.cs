using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

using MartinCostello.SqlLocalDb;

using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace Automation.Utils
{
    public static partial class CoreDatabaseUtil
    {
        public static void ExecuteSqlFromFolder(string folder, string initialCatalog = null)
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

            var builder = new SqlConnectionStringBuilder(ConfigUtil.ConnectionString);

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
            var lines = regex.Split(sql);

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

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }

        public static void ExecuteSql(string sqlFileOrText, string initialCatalog = null)
        {
            var builder = new SqlConnectionStringBuilder(ConfigUtil.ConnectionString);

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

        public static void Drop()
        {
            var database = GetDatabase(DatabaseName);

            if (database == null)
            {
                return;
            }

            Drop(database);
        }

        public static void Drop(Database database)
        {
            try
            {
                SetModeRestrictedUser(database.Name);
                database.Drop();
            }
            catch (FailedOperationException)
            {
                try
                {
                    ConfigUtil.Server.KillDatabase(database.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                SetModeMultiUser(database);
                Console.WriteLine(ex.Message);
            }
        }

        public static void ReCreate()
        {
            Drop();
            Create();
        }

        public static void Create()
        {
            var srv = ConfigUtil.Server;

            var db = new Database(srv, DatabaseName);
            var fileGroup = new FileGroup(db, "PRIMARY");

            var dataFile = new DataFile(
                               fileGroup,
                               DatabaseName,
                               Path.Combine(ConfigUtil.DbDataDirectory, FileName + ".mdf"))
                           {
                               Size = 5.5 * 1024.0, GrowthType = FileGrowthType.KB, Growth = 1.0 * 1024.0
                           };

            fileGroup.Files.Add(dataFile);

            db.FileGroups.Add(fileGroup);

            var logFile = new LogFile(
                              db,
                              DatabaseName + "_log",
                              Path.Combine(ConfigUtil.DbDataDirectory, FileName + "_log.ldf"))
                          {
                              Size = 1.0 * 1024.0, GrowthType = FileGrowthType.Percent, Growth = 10.0
                          };

            db.LogFiles.Add(logFile);

            db.Create();

            srv.Refresh();
        }

        public static void CopyDetachedFiles()
        {
            DetachDatabase();

            new FileInfo(SourceDataPath).CopyTo(CopyDataPath, true);
            new FileInfo(SourceLogPath).CopyTo(CopyLogPath, true);
        }

        public static void AttachDatabase()
        {
            new FileInfo(CopyDataPath).CopyTo(SourceDataPath, true);
            new FileInfo(CopyLogPath).CopyTo(SourceLogPath, true);

            ConfigUtil.Server.AttachDatabase(DatabaseName, SourceFiles);
            if (!ConfigUtil.UseLocalDb)
            {
                SqlConnection.ClearAllPools();
            }
        }

        public static void DetachDatabase()
        {
            SetModeRestrictedUser(DatabaseName);

            ConfigUtil.Server.DetachDatabase(DatabaseName, false);
        }

        public static void CreateLocalDb()
        {
            if (!ConfigUtil.UseLocalDb)
            {
                return;
            }

            using (var localDB = new SqlLocalDbApi())
            {
                var instanceInfo = localDB.GetInstanceInfo(ConfigUtil.InstanceName);
                localDB.AutomaticallyDeleteInstanceFiles = true;

                if (!instanceInfo.Exists)
                {
                    localDB.CreateInstance(instanceInfo.Name);
                }

                if (!instanceInfo.IsRunning)
                {
                    localDB.StartInstance(instanceInfo.Name);
                }
            }
        }

        public static void DeleteLocalDb()
        {
            if (!ConfigUtil.UseLocalDb)
            {
                return;
            }

            using (var localDB = new SqlLocalDbApi())
            {
                var instanceInfo = localDB.GetInstanceInfo(ConfigUtil.InstanceName);

                if (instanceInfo.IsRunning)
                {
                    localDB.StopInstance(instanceInfo.Name);
                }

                localDB.DeleteInstance(ConfigUtil.InstanceName);
            }
        }
    }
}
