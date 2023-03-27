using JetBrains.Annotations;

namespace Framework.Core;

public class TypeResolverHelper
{
    public static ITypeResolver<T> Create<T>([NotNull] IReadOnlyDictionary<T, Type> dict)
    {
        if (dict == null) throw new ArgumentNullException(nameof(dict));

        return Create<T>(dict.GetValueOrDefault, () => dict.Values);
    }

    public static ITypeResolver<string> Create([NotNull] ITypeSource typeSource, TypeSearchMode searchMode)
    {
        if (typeSource == null) throw new ArgumentNullException(nameof(typeSource));

        var filter = searchMode.ToFilter();

        return new FuncTypeResolver<string>(ident => typeSource.GetTypes().FirstOrDefault(type => filter(type, ident)),
                                            ()     => typeSource.GetTypes());
    }

    public static ITypeResolver<T> Create<T>(Func<T, Type> resolveFunc, Func<IEnumerable<Type>> getSourceTypesFunc)
    {
        return new FuncTypeResolver<T>(resolveFunc, getSourceTypesFunc);
    }


    public static ITypeResolver<string> CreateDefault([NotNull] ITypeSource typeSource)
    {
        if (typeSource == null) throw new ArgumentNullException(nameof(typeSource));

        return Create(typeSource, TypeSearchMode.Both).WithCache().WithLock();
    }

    public static readonly ITypeResolver<string> Base = new TypeSource(typeof(object).Assembly, typeof(Ignore).Assembly).ToDefaultTypeResolver();


    private class FuncTypeResolver<T> : ITypeResolver<T>
    {
        private readonly Func<T, Type> _resolveFunc;

        private readonly Func<IEnumerable<Type>> _getSourceTypesFunc;


        public FuncTypeResolver(Func<T, Type> resolveFunc, Func<IEnumerable<Type>> getSourceTypesFunc)
        {
            if (resolveFunc == null) throw new ArgumentNullException(nameof(resolveFunc));
            if (getSourceTypesFunc == null) throw new ArgumentNullException(nameof(getSourceTypesFunc));

            this._resolveFunc = resolveFunc;
            this._getSourceTypesFunc = getSourceTypesFunc;
        }


        public Type Resolve(T identity)
        {
            return this._resolveFunc(identity);
        }

        public IEnumerable<Type> GetTypes()
        {
            return this._getSourceTypesFunc();
        }
    }
}
