namespace Framework.Core;

public static class FuncExtensions
{
    public static TInterface ToLazyInterfaceImplement<TInterface>(this Func<TInterface> getValueFunc)
    {
        if (getValueFunc == null) throw new ArgumentNullException(nameof(getValueFunc));

        return getValueFunc.ToLazyInterfaceImplement<TInterface, TInterface>();
    }

    public static TInterface ToLazyInterfaceImplement<TInterface, TImplement>(this Func<TImplement> getValueFunc)
        where TImplement : TInterface
    {
        return LazyInterfaceImplementHelper<TInterface>.CreateProxy(() => getValueFunc());
    }
}
