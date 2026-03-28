using Framework.Database.NHibernate.DBGenerator.Contracts;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Contracts;

public interface IConfigurable
{
    bool IsFreezed { get; }
}

public interface IMigrationScriptGeneratorBuilder : IConfigurable
{
    IMigrationScriptGeneratorBuilder WithDatabase(string databaseName);
    IMigrationScriptGeneratorBuilder WithTable(string tableName);
    IMigrationScriptGeneratorBuilder WithCustom(IMigrationScriptReader source);
    IMigrationScriptGeneratorBuilder WithFolder(string directoryPath);
}
