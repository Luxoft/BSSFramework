using System;
using System.Collections.Generic;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;

namespace Framework.DomainDriven.DBGenerator;

abstract class DatabaseScriptGeneratorContainer
{
    private readonly IList<IDatabaseScriptGenerator> _generators;
    private bool _isComplete;
    protected readonly MigrationDBScriptGeneratorBuilder _migrationDbScriptGeneratorBuilder;

    protected DatabaseScriptGeneratorContainer()
    {
        this._generators = new List<IDatabaseScriptGenerator>();
        this._migrationDbScriptGeneratorBuilder = new MigrationDBScriptGeneratorBuilder();
    }

    public MigrationDBScriptGeneratorBuilder MigrationBuilder => this._migrationDbScriptGeneratorBuilder;

    public IDatabaseScriptGenerator GetCombined()
    {
        return this._generators.Combine();
    }


    protected void Register(IDatabaseScriptGenerator generator)
    {
        this.ValidateAddOperation();

        this._generators.Add(generator);
    }

    public void Freeze()
    {
        this._isComplete = true;
    }

    public abstract IDatabaseScriptGenerator Build(DBGenerateScriptMode mode);

    private void ValidateAddOperation()
    {
        if (this._isComplete)
        {
            throw new ArgumentException("Also builded result script");
        }
    }
}
