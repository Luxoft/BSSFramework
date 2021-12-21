using System.Collections.Generic;
using System.Linq;
using Framework.Core;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;

namespace Framework.DomainDriven.DBGenerator
{
    class MainDBScriptGeneratorBuilder : DatabaseScriptGeneratorContainer, IMainDBScriptGeneratorBuilder
    {
        private bool removeSchemaDatabase = true;

        public IMainDBScriptGeneratorBuilder WithMain(
            DatabaseScriptGeneratorMode mode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
            string previusColumnPostfix = "_previusVersion",
            ICollection<string> ignoredIndexes = null)
        {
            this.ValidateConfigurate();

            this.Register(new DatabaseScriptGenerator(mode, previusColumnPostfix, ignoredIndexes));

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
            var combined = new[] { this.GetCombined(), this._migrationDbScriptGeneratorBuilder.Build(mode) }.Combine();
            switch (mode)
            {
                case DBGenerateScriptMode.AppliedOnCopySchemeDatabase:
                    {
                        return combined.Unsafe(false, this.removeSchemaDatabase, new[] { this._migrationDbScriptGeneratorBuilder.TableName });
                    }
                case DBGenerateScriptMode.AppliedOnCopySchemeAndDataDatabase:
                    {
                        return combined.Unsafe(true, this.removeSchemaDatabase, new[] { this._migrationDbScriptGeneratorBuilder.TableName });
                    }

                default:
                    {
                        return combined;
                    }
            }

        }

        public bool IsFreezed { get; internal set; }
    }
}
