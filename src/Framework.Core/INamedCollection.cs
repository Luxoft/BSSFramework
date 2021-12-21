using System.Collections.Generic;

namespace Framework.Core
{
    public interface INamedCollection<out T> : IEnumerable<T>
    {
        T this[string name] { get; }
    }
}