using Framework.Database.NHibernate.DBGenerator.Contracts;

namespace Framework.Database.NHibernate.DBGenerator.Team;

public static class DatabaseScriptGeneratorExtension
{
    public static IDatabaseScriptGenerator Combine(this IDatabaseScriptGenerator scriptGenerator, params IDatabaseScriptGenerator[] generators)
    {
        return generators.Concat([scriptGenerator]).Combine();
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
        private readonly List<IMigrationScriptReader> readers;

        public CompositeMigrationScriptReader(IEnumerable<IMigrationScriptReader> readers)
        {
            this.readers = readers.ToList();
        }

        public IEnumerable<MigrationDbScript> Read()
        {
            return this.readers.SelectMany(z => z.Read());
        }
    }


    class CompositeDatabaseScriptGenerator : IDatabaseScriptGenerator
    {
        private readonly List<IDatabaseScriptGenerator> services;

        public CompositeDatabaseScriptGenerator(List<IDatabaseScriptGenerator> services)
        {
            this.services = services;
        }

        public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
        {
            var results = this.services.Select(z => z.GenerateScript(context)).ToList();

            return results.Combine();
        }
    }
}
