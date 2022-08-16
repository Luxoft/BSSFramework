using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Automation.Utils;

using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;

namespace Automation.Helpers
{
    public static class DatabaseHelper
    {
        public static List<T> GetAll<T>(string connectionString)
            where T : class
        {
            var tableName = GetTableName(typeof(T));

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<T>($"SELECT * FROM {tableName}").ToList();
            }
        }

        private static string GetTableName(Type type)
        {
            var tableAttrName =
                type.GetCustomAttribute<TableAttribute>(false)?.Name
                ?? (type.GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic)?.Name;

            if (tableAttrName != null)
            {
                return tableAttrName;
            }

            throw new Exception($"Table name for '{type.Name}' not found");
        }
    }
}
