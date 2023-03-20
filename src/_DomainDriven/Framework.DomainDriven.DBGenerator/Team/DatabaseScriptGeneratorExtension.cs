using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.DBGenerator.Contracts;


namespace Framework.DomainDriven.DBGenerator.Team;

public static class DatabaseScriptGeneratorExtension
{
    public static IDatabaseScriptGenerator Combine(this IDatabaseScriptGenerator scriptGenerator, params IDatabaseScriptGenerator[] generators)
    {
        return generators.Concat(new[] { scriptGenerator }).Combine();
    }

    public static IDatabaseScriptGenerator Combine(this IEnumerable<IDatabaseScriptGenerator> scriptGenerators)
    {
        return new CompositeDatabaseScriptGenerator(scriptGenerators.ToList());
    }

    public static IMigrationScriptReader Combine(this IMigrationScriptReader reader,
                                                 params IMigrationScriptReader[] readers)
    {
        return new[] { reader }.Concat(readers).Combine();
    }

    public static IMigrationScriptReader Combine(this IEnumerable<IMigrationScriptReader> readers)
    {
        return new CompositeMigrationScriptReader(readers);
    }


    public class CompositeMigrationScriptReader : IMigrationScriptReader
    {
        private readonly IList<IMigrationScriptReader> _readers;

        public CompositeMigrationScriptReader(IEnumerable<IMigrationScriptReader> readers)
        {
            this._readers = readers.ToList();
        }

        public IEnumerable<MigrationDbScript> Read()
        {
            return this._readers.SelectMany(z => z.Read());
        }
    }


    class CompositeDatabaseScriptGenerator : IDatabaseScriptGenerator
    {
        private readonly IList<IDatabaseScriptGenerator> _services;

        public CompositeDatabaseScriptGenerator(IList<IDatabaseScriptGenerator> services)
        {
            this._services = services;
        }

        public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
        {
            var results = this._services.Select(z => z.GenerateScript(context)).ToList();

            return results.Combine();
        }
    }
}
