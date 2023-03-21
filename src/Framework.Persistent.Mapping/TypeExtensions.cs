using System;
using System.Linq;
using Framework.Core;

namespace Framework.Persistent.Mapping;

public static class TypeExtensions
{
    public static (string tableName, string schemaName) GetTableName(this Type type, string defaultSchema)
    {
        var tableAttribute = type.GetTableAttribute();
        if (null == tableAttribute)
        {
            return (tableName: type.Name, schemaName: defaultSchema);
        }
        return (tableName: tableAttribute.Name, schemaName: tableAttribute.Schema ?? defaultSchema);
    }

    public static TableAttribute GetTableAttribute(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        if (type.IsAbstract)
        {
            //    throw new Exception(string.Format("Type {0} must be not abstract", type.Name));
        }

        return (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
    }
}
