using System;

namespace Framework.Core;

public static class FuncExtensions
{
    public static TInterface ToLazyInterfaceImplement<TInterface>(this Func<TInterface> getValueFunc, bool startAsync = false)
    {
        if (getValueFunc == null) throw new ArgumentNullException(nameof(getValueFunc));

        return LazyInterfaceImplementHelper.CreateProxy(getValueFunc, startAsync);
    }

    public static TInterface ToLazyInterfaceImplement<TInterface, TImplement>(this Func<TImplement> getValueFunc, bool startAsync = false)
            where TImplement : TInterface
    {
        if (getValueFunc == null) throw new ArgumentNullException(nameof(getValueFunc));

        var func = getValueFunc.Pipe(startAsync, f => f.WithCache(true));

        return LazyInterfaceImplementHelper<TInterface>.CreateProxy(() => func());
    }
}
