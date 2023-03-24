using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Framework.Core;

public class PropertyEqualityComparer<T, TProperty> : EqualityComparer<T>
{
    private readonly Func<T, TProperty> getPropertyFunc;

    public PropertyEqualityComparer(Func<T, TProperty> getPropertyFunc)
    {
        this.getPropertyFunc = getPropertyFunc;
    }

    public override bool Equals(T x, T y)
    {
        var xValue = this.getPropertyFunc(x);
        var yValue = this.getPropertyFunc(y);

        return object.Equals(xValue, yValue);
    }

    public override int GetHashCode(T obj)
    {
        return 0;
    }
}

public class EqualityComparerImpl<T> : EqualityComparer<T>
{
    private readonly Func<T, T, bool> _equalsFunc;
    private readonly Func<T, int> _getHashFunc;

    public EqualityComparerImpl(Func<T, T, bool> equalsFunc, Func<T, int> getHashFunc = null)
    {
        if (equalsFunc == null) throw new ArgumentNullException(nameof(equalsFunc));

        this._equalsFunc = equalsFunc;
        this._getHashFunc = getHashFunc;
    }

    public override bool Equals(T x, T y)
    {
        return this._equalsFunc(x, y);
    }

    public override int GetHashCode(T obj)
    {
        return this._getHashFunc.Maybe(v => v(obj));
    }

    public static EqualityComparerImpl<T> Create<TKey>([NotNull] Func<T, TKey> keySelector)
    {
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        var keyComparer = EqualityComparer<TKey>.Default;

        return new EqualityComparerImpl<T>((v1, v2) => keyComparer.Equals(keySelector(v1), keySelector(v2)));
    }
}
