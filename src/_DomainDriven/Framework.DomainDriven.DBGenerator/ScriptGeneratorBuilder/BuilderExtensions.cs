using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.ScriptGenerators;

namespace Framework.DomainDriven.DBGenerator;

static class BuilderExtensions
{
    internal static IDatabaseScriptGenerator ToTryCreateDatabase(this IDatabaseScriptGenerator source)
    {
        return new TryCreateDatabaseScriptGenerator(source);
    }
    internal static IDatabaseScriptGenerator Unsafe(
            this IDatabaseScriptGenerator scriptGenerator,
            bool copyData,
            bool removeSchemaDatabase,
            string[] copyDataForTables = null)
    {
        return new UnsafeApplyChangedDatabaseScriptGenerator(scriptGenerator, copyData, removeSchemaDatabase, copyDataForTables);
    }

    internal static void ValidateOneSet<T>(this T currentValue, T nextValue, string propertyName) where T : class
    {
        if (null != currentValue && !EqualityComparer<T>.Default.Equals(currentValue, nextValue))
        {
            throw new ArgumentException($"Value:{propertyName} also setted");
        }
    }

    public static void ValidateConfigurate(this IConfigurable source)
    {
        if (source.IsFreezed)
        {
            throw new ArgumentException("Can't changed values. Object in freeze status");
        }
    }
}
