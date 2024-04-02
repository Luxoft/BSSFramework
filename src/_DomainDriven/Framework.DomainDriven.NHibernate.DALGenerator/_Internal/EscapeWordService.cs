namespace Framework.DomainDriven.NHibernate.DALGenerator;

public class EscapeWordService : IEscapeWordService
{
    private readonly Lazy<HashSet<string>> lazyEscapeWordsHasSet;

    public EscapeWordService()
    {
        this.lazyEscapeWordsHasSet = new Lazy<HashSet<string>>(() => this.GetEscapeWords().ToHashSet(StringComparer.CurrentCultureIgnoreCase));
    }

    public bool IsEscapeWord(string word)
    {
        return this.lazyEscapeWordsHasSet.Value.Contains(word);
    }

    protected virtual IEnumerable<string> GetEscapeWords()
    {
        yield return "order";
    }
}
