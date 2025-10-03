namespace Framework.Core;

public static class DictionaryHelper
{
    public static Dictionary<TKey, TValue> Create<TKey, TValue>(params KeyValuePair<TKey, TValue>[] pairs)
        where TKey : notnull
    {
        return pairs.ToDictionary();
    }
}
