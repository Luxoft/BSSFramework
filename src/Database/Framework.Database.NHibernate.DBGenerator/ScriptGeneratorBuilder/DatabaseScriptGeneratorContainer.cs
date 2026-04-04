using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Impl;
using Framework.Database.NHibernate.DBGenerator.Team;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder;

abstract class DatabaseScriptGeneratorContainer
{
    private readonly List<IDatabaseScriptGenerator> generators = [];
    private bool isComplete;
    protected readonly MigrationDBScriptGeneratorBuilder MigrationDbScriptGeneratorBuilder = new();

    public MigrationDBScriptGeneratorBuilder MigrationBuilder => this.MigrationDbScriptGeneratorBuilder;

    public IDatabaseScriptGenerator GetCombined() => this.generators.Combine();

    protected void Register(IDatabaseScriptGenerator generator)
    {
        this.ValidateAddOperation();

        this.generators.Add(generator);
    }

    public void Freeze() => this.isComplete = true;

    public abstract IDatabaseScriptGenerator Build(DBGenerateScriptMode mode);

    private void ValidateAddOperation()
    {
        if (this.isComplete)
        {
            throw new ArgumentException("Also builded result script");
        }
    }
}
