using System;

using JetBrains.Annotations;

namespace Framework.Core;

public class CallProxy<T>
{
    private readonly Func<T> func;

    public CallProxy([NotNull] Func<T> baseInstance)
    {
        this.func = baseInstance ?? throw new ArgumentNullException(nameof(baseInstance));
    }

    public T Value => this.func();
}
