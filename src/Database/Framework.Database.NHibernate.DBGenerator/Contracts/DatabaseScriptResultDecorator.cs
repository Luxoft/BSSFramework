using Framework.Database.NHibernate.DBGenerator.Team;

namespace Framework.Database.NHibernate.DBGenerator.Contracts;

public class DatabaseScriptResultDecorator : IDatabaseScriptResult
{
    private readonly IDatabaseScriptResult source;

    private readonly Func<string, string> resultSelector;

    public DatabaseScriptResultDecorator(IDatabaseScriptResult source, Func<string, string> resultSelector)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        if (resultSelector == null)
        {
            throw new ArgumentNullException(nameof(resultSelector));
        }

        this.source = source;
        this.resultSelector = resultSelector;
    }

    public IEnumerable<string> this[ApplyMigrationDbScriptMode mode] => this.source[mode].Select(z => this.resultSelector(z));

    public IEnumerable<IEnumerable<string>> GetResults()
    {
        foreach (var result in this.source.GetResults())
        {
            yield return result.Select(this.resultSelector);
        }
    }

    public string ToNewLinesCombined() => this.resultSelector(this.source.ToNewLinesCombined());

    public IDatabaseScriptResult Evaluate() => this.source.Evaluate();
}
