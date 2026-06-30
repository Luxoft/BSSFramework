using Anch.Core;
using Anch.Core.DictionaryCache;

namespace Framework.Core.AnonymousTypeBuilder;

public static class AnonymousTypeBuilderExtensions
{
    public static IAnonymousTypeBuilder<TNewSource> OverrideInput<TBaseSource, TNewSource>(this IAnonymousTypeBuilder<TBaseSource> anonymousTypeBuilder, Func<TNewSource, TBaseSource> selector)
    {
        if (anonymousTypeBuilder is null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        return new FuncAnonymousTypeBuilder<TNewSource>(newSource => anonymousTypeBuilder.GetAnonymousType(selector(newSource)));
    }

    private static string GenerateNamePostfix(this IEnumerable<ITypeMapMember> members)
    {
        if (members is null) throw new ArgumentNullException(nameof(members));

        return members.Concat(member => " | " + member.Name + "_" + member.Type.FullName);
    }


    public static IAnonymousTypeBuilder<TMap> WithGenerateNamePostfix<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder)
            where TMap : ISwitchNameObject<TMap>, ITypeMap<ITypeMapMember>
    {
        if (anonymousTypeBuilder is null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

        return anonymousTypeBuilder.WithSwitchName(map => map.Name + map.Members.GenerateNamePostfix());
    }

    public static IAnonymousTypeBuilder<TMap> WithCompressName<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder)
            where TMap : ITypeMap, ISwitchNameObject<TMap>
    {
        if (anonymousTypeBuilder is null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

        var shortNameCache = new DictionaryCache<string, string>(name => $"{name.Take(70).Concat()}_{Guid.NewGuid()}");

        return anonymousTypeBuilder.WithSwitchName(map => shortNameCache[map.Name]);
    }

    public static IAnonymousTypeBuilder<TMap> WithSwitchName<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder, Func<TMap, string> getNewName)
            where TMap : ISwitchNameObject<TMap>
    {
        if (anonymousTypeBuilder is null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

        return new FuncAnonymousTypeBuilder<TMap>(map => anonymousTypeBuilder.GetAnonymousType(map.SwitchName(getNewName(map))));
    }

    public static IAnonymousTypeBuilder<TMap> WithCache<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder, IEqualityComparer<TMap>? equalityComparer = null)
            where TMap : notnull
    {
        if (anonymousTypeBuilder is null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

        var cache = new Dictionary<TMap, Type>(equalityComparer ?? EqualityComparer<TMap>.Default);

        return new FuncAnonymousTypeBuilder<TMap>(map => cache.GetValueOrCreate(map, () => anonymousTypeBuilder.GetAnonymousType(map)));
    }

    public static IAnonymousTypeBuilder<TMap> WithLock<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder)
    {
        if (anonymousTypeBuilder is null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

        var locker = new object();

        return new FuncAnonymousTypeBuilder<TMap>(map =>
                                                  {
                                                      lock (locker)
                                                      {
                                                          return anonymousTypeBuilder.GetAnonymousType(map);
                                                      }
                                                  });
    }

    private class FuncAnonymousTypeBuilder<TMap>(Func<TMap, Type> func) : IAnonymousTypeBuilder<TMap>
    {
        private readonly Func<TMap, Type> func = func ?? throw new ArgumentNullException(nameof(func));

        public Type GetAnonymousType(TMap typeMap) => this.func(typeMap);
    }
}

