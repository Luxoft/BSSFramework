using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core
{
    public static class AnonymousTypeBuilderExtensions
    {
        public static IAnonymousTypeBuilder<TNewSource> OverrideInput<TBaseSource, TNewSource>(this IAnonymousTypeBuilder<TBaseSource> anonymousTypeBuilder, Func<TNewSource, TBaseSource> selector)
        {
            if (anonymousTypeBuilder == null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new FuncAnonymousTypeBuilder<TNewSource>(newSource => anonymousTypeBuilder.GetAnonymousType(selector(newSource)));
        }

        private static string GenerateNamePosfix(this IEnumerable<ITypeMapMember> members)
        {
            if (members == null) throw new ArgumentNullException(nameof(members));

            return members.Concat(member => " | " + member.Name + "_" + member.Type.FullName);
        }


        public static IAnonymousTypeBuilder<TMap> WithGenerateNamePosfix<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder)
            where TMap : ISwitchNameObject<TMap>, ITypeMap<ITypeMapMember>
        {
            if (anonymousTypeBuilder == null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

            return anonymousTypeBuilder.WithSwitchName(map => map.Name + map.Members.GenerateNamePosfix());
        }

        public static IAnonymousTypeBuilder<TMap> WithCompressName<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder)
            where TMap : ITypeMap, ISwitchNameObject<TMap>
        {
            if (anonymousTypeBuilder == null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

            var shortNameCache = new DictionaryCache<string, string>(name => $"{name.Take(70).Concat()}_{Guid.NewGuid()}");

            return anonymousTypeBuilder.WithSwitchName(map => shortNameCache[map.Name]);
        }

        public static IAnonymousTypeBuilder<TMap> WithSwitchName<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder, Func<TMap, string> getNewName)
            where TMap : ISwitchNameObject<TMap>
        {
            if (anonymousTypeBuilder == null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

            return new FuncAnonymousTypeBuilder<TMap>(map => anonymousTypeBuilder.GetAnonymousType(map.SwitchName(getNewName(map))));
        }

        public static IAnonymousTypeBuilder<TMap> WithCache<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder, IEqualityComparer<TMap> equalityComparer = null)
        {
            if (anonymousTypeBuilder == null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

            var cache = new Dictionary<TMap, Type>(equalityComparer ?? EqualityComparer<TMap>.Default);

            return new FuncAnonymousTypeBuilder<TMap>(map => cache.GetValueOrCreate(map, () => anonymousTypeBuilder.GetAnonymousType(map)));
        }

        public static IAnonymousTypeBuilder<TMap> WithLock<TMap>(this IAnonymousTypeBuilder<TMap> anonymousTypeBuilder)
        {
            if (anonymousTypeBuilder == null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

            var locker = new object();

            return new FuncAnonymousTypeBuilder<TMap>(map =>
            {
                lock(locker)
                {
                    return anonymousTypeBuilder.GetAnonymousType(map);
                }
            });
        }

        private class FuncAnonymousTypeBuilder<TMap> : IAnonymousTypeBuilder<TMap>
        {
            private readonly Func<TMap, Type> _func;


            public FuncAnonymousTypeBuilder(Func<TMap, Type> func)
            {
                if (func == null) throw new ArgumentNullException(nameof(func));

                this._func = func;
            }

            public Type GetAnonymousType(TMap typeMap)
            {
                return this._func(typeMap);
            }
        }
    }
}
