namespace Framework.Database.NHibernate.DALGenerator._Internal;

public class EscapeWordService : IEscapeWordService
{
    private readonly Lazy<HashSet<string>> lazyEscapeWordsHasSet;

    public EscapeWordService() => this.lazyEscapeWordsHasSet = new Lazy<HashSet<string>>(() => this.GetEscapeWords().ToHashSet(StringComparer.CurrentCultureIgnoreCase));

    public bool IsEscapeWord(string word) => this.lazyEscapeWordsHasSet.Value.Contains(word);

    protected virtual IEnumerable<string> GetEscapeWords()
    {
        yield return "order";
    }
}

public class EmptyEscapeWordService : IEscapeWordService
{
    public bool IsEscapeWord(string word) => false;
}
