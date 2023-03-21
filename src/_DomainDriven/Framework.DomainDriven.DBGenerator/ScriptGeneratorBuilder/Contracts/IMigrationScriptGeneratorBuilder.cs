using Framework.DomainDriven.DBGenerator.Contracts;

namespace Framework.DomainDriven.DBGenerator;

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
