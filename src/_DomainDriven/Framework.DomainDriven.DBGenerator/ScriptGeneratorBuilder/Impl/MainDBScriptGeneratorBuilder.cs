using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;

namespace Framework.DomainDriven.DBGenerator;

class MainDBScriptGeneratorBuilder : DatabaseScriptGeneratorContainer, IMainDBScriptGeneratorBuilder
{
    private bool _removeSchemaDatabase = true;

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
        this._removeSchemaDatabase = false;
        return this;
    }

    public override IDatabaseScriptGenerator Build(DBGenerateScriptMode mode)
    {
        var combined = new[] { this.GetCombined(), this._migrationDbScriptGeneratorBuilder.Build(mode) }.Combine();
        switch (mode)
        {
            case DBGenerateScriptMode.AppliedOnCopySchemeDatabase:
            {
                return combined.Unsafe(false, this._removeSchemaDatabase, new[] { this._migrationDbScriptGeneratorBuilder.TableName });
            }
            case DBGenerateScriptMode.AppliedOnCopySchemeAndDataDatabase:
            {
                return combined.Unsafe(true, this._removeSchemaDatabase, new[] { this._migrationDbScriptGeneratorBuilder.TableName });
            }

            default:
            {
                return combined;
            }
        }

    }

    public bool IsFreezed { get; internal set; }
}
