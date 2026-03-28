using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;
using Framework.Database.NHibernate.DBGenerator.Team;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Impl;

class MainDBScriptGeneratorBuilder : DatabaseScriptGeneratorContainer, IMainDBScriptGeneratorBuilder
{
    private bool removeSchemaDatabase = true;

    public IMainDBScriptGeneratorBuilder WithMain(
        DatabaseScriptGeneratorMode mode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
        string previousColumnPostfix = "_previousVersion",
        ICollection<string> ignoredIndexes = null,
        IDataTypeComparer dataTypeComparer = null)
    {
        this.ValidateConfigurate();

        this.Register(new DatabaseScriptGenerator(mode, previousColumnPostfix, ignoredIndexes, dataTypeComparer));

        return this;
    }

    public IMainDBScriptGeneratorBuilder WithUniqueGroup(params IgnoreLink[] ignore)
    {
        this.ValidateConfigurate();

        this.Register(new UniqueGroupDatabaseScriptGenerator(ignore));

        return this;
    }

    public IMainDBScriptGeneratorBuilder WithCustom(IDatabaseScriptGenerator service)
    {
        this.Register(service);

        return this;
    }

    public IMainDBScriptGeneratorBuilder WithRequireRef(params IgnoreLink[] ignoreLinks)
    {
        this.ValidateConfigurate();

        this.Register(new RequiredRefDatabaseScriptGenerator(ignoreLinks));

        return this;
    }

    public IMigrationScriptGeneratorBuilder MigrationBuilder => base.MigrationBuilder;

    public IMainDBScriptGeneratorBuilder WithPreserveSchemaDatabase()
    {
        this.removeSchemaDatabase = false;
        return this;
    }

    public override IDatabaseScriptGenerator Build(DBGenerateScriptMode mode)
    {
        var combined = new[] { this.GetCombined(), this.MigrationDbScriptGeneratorBuilder.Build(mode) }.Combine();
        switch (mode)
        {
            case DBGenerateScriptMode.AppliedOnCopySchemeDatabase:
            {
                return combined.Unsafe(false, this.removeSchemaDatabase, [this.MigrationDbScriptGeneratorBuilder.TableName]);
            }
            case DBGenerateScriptMode.AppliedOnCopySchemeAndDataDatabase:
            {
                return combined.Unsafe(true, this.removeSchemaDatabase, [this.MigrationDbScriptGeneratorBuilder.TableName]);
            }

            default:
            {
                return combined;
            }
        }

    }

    public bool IsFreezed { get; internal set; }
}
