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


    public class CompositeMigrationScriptReader(IEnumerable<IMigrationScriptReader> readers) : IMigrationScriptReader
    {
        private readonly List<IMigrationScriptReader> readers = readers.ToList();

        public IEnumerable<MigrationDbScript> Read()
        {
            return this.readers.SelectMany(z => z.Read());
        }
    }


    class CompositeDatabaseScriptGenerator(List<IDatabaseScriptGenerator> services) : IDatabaseScriptGenerator
    {
        public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
        {
            var results = services.Select(z => z.GenerateScript(context)).ToList();

            return results.Combine();
        }
    }
}
