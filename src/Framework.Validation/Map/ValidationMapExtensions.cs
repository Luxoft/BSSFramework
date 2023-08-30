using System.Reflection;

using Framework.Core;

namespace Framework.Validation;

public static class ValidationMapExtensions
{
    public static IClassValidationMap<TSource> GetClassMap<TSource>(this IValidationMap validationMap)
    {
        if (validationMap == null) throw new ArgumentNullException(nameof(validationMap));

        return (IClassValidationMap<TSource>)validationMap.GetClassMap(typeof(TSource));
    }

    public static IValidationMap WithFixedTypes(this IValidationMap baseValidationMap, IEnumerable<Type> types)
    {
        if (baseValidationMap == null) throw new ArgumentNullException(nameof(baseValidationMap));
        if (types == null) throw new ArgumentNullException(nameof(types));

        return new FixedValidationMap(baseValidationMap, types);
    }

    public static IValidationMap WithFixedTypes<TDomainObjectBase>(this IValidationMap baseValidationMap, IEnumerable<Assembly> assemblies)
    {
        if (baseValidationMap == null) throw new ArgumentNullException(nameof(baseValidationMap));
        if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

        return baseValidationMap.WithFixedTypes<TDomainObjectBase>(assemblies.ToArray());
    }

    public static IValidationMap WithFixedTypes<TDomainObjectBase>(this IValidationMap baseValidationMap, params Assembly[] assemblies)
    {
        if (baseValidationMap == null) throw new ArgumentNullException(nameof(baseValidationMap));
        if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));


        var types = from assembly in assemblies.Any() ? assemblies : new[] { typeof(TDomainObjectBase).Assembly }

                    from type in assembly.GetTypes()

                    where !type.IsAbstract && typeof(TDomainObjectBase).IsAssignableFrom(type)

                    select type;


        return baseValidationMap.WithFixedTypes(types);
    }


    private class FixedValidationMap : IValidationMap
    {
        private readonly IValidationMap _baseValidationMap;

        private readonly Dictionary<Type, IClassValidationMap> _classMapCache;


        public FixedValidationMap(IValidationMap baseValidationMap, IEnumerable<Type> types)
        {
            if (baseValidationMap == null) throw new ArgumentNullException(nameof(baseValidationMap));
            if (types == null) throw new ArgumentNullException(nameof(types));

            this._baseValidationMap = baseValidationMap;

            this._classMapCache = types.ToDictionary(t => t, t => this._baseValidationMap.GetClassMap(t));
        }


        public IServiceProvider ServiceProvider => this._baseValidationMap.ServiceProvider;


        public IClassValidationMap GetClassMap(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var classValidator = this._classMapCache.GetMaybeValue(type);

            return classValidator.Match(map => map, () =>
                                                    {
                                                        if (type.IsSystemOrCoreType())
                                                        {
                                                            return this._baseValidationMap.GetClassMap(type);
                                                        }
                                                        else
                                                        {
                                                            throw new Exception($"ClassValidationMap for type {type.Name} not found");
                                                        }
                                                    });
        }
    }
}
