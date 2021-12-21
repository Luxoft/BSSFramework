using System.Collections.Generic;

namespace Framework.Core
{
    public interface IDictionaryCache<in TKey, out TValue>
    {
        IEnumerable<TValue> Values { get; }

        TValue this[TKey key] { get; }
    }
}
