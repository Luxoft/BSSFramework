using System.Collections.Concurrent;
using System.Collections.Immutable;

using Anch.Core;

namespace Framework.Core.TypeResolving;

public class TypeResolverHelper
{
    public static ITypeResolver<T> Create<T>(IReadOnlyDictionary<T, Type> dict)
        where T : notnull => Create<T>(dict.GetValueOrDefault, dict.Values);

    public static ITypeResolver<TypeNameIdentity> Create(ITypeSource typeSource, TypeSearchMode searchMode)
    {
        var filter = searchMode.ToFilter();

        return new FuncTypeResolver<TypeNameIdentity>(
            ident => typeSource.Types.FirstOrDefault(type => filter(type, ident)),
            () => typeSource.Types);
    }

    public static ITypeResolver<T> Create<T>(Func<T, Type?> resolveFunc, IEnumerable<Type> sourceTypes)
        where T : notnull =>
        Create(resolveFunc, [.. sourceTypes]);

    public static ITypeResolver<T> Create<T>(Func<T, Type?> resolveFunc, Func<IEnumerable<Type>> getSourceTypes)
        where T : notnull =>
        new FuncTypeResolver<T>(resolveFunc, () => [.. getSourceTypes()]);

    public static ITypeResolver<T> Create<T>(Func<T, Type?> resolveFunc, ImmutableHashSet<Type> sourceTypes)
        where T : notnull =>
        new FuncTypeResolver<T>(resolveFunc, () => sourceTypes);

    public static ITypeResolver<TypeNameIdentity> CreateDefault(ITypeSource typeSource) => Create(typeSource, TypeSearchMode.Both);

    public static ITypeResolver<TypeNameIdentity> Base { get; } = new TypeSource([typeof(object).Assembly, typeof(Ignore).Assembly]).ToDefaultTypeResolver();


    private class FuncTypeResolver<T>(Func<T, Type?> resolveFunc, Func<ImmutableHashSet<Type>> getSourceTypesFunc) : ITypeResolver<T>
        where T : notnull
    {
        private readonly ConcurrentDictionary<T, Type?> cache = [];

        public Type? TryResolve(T identity) => this.cache.GetOrAdd(identity, _ => resolveFunc(identity));

        public ImmutableHashSet<Type> Types => getSourceTypesFunc();
    }
}
