using System;

namespace Framework.Core
{
    [Obsolete("v10 not used")]
    public interface IContainer<out TValue>
    {
        TValue Value { get; }
    }
}
