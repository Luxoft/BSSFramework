using System.Reflection;

using CommonFramework;
using CommonFramework.Maybe;

namespace Framework.Core;

public static class CoreTypeExtensions
{
    public static bool IsAssignableToAny(this Type type, IEnumerable<Type> baseTypes)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (baseTypes == null) throw new ArgumentNullException(nameof(baseTypes));

        return baseTypes.Any(type.IsAssignableTo);
    }

    public static Type ToDelegateType(this IEnumerable<Type> preParameterTypes, Type resultType)
    {
        if (preParameterTypes == null) throw new ArgumentNullException(nameof(preParameterTypes));
        if (resultType == null) throw new ArgumentNullException(nameof(resultType));

        var parameterTypes = preParameterTypes.ToArray();

        var rankDelegateTypeName =

            resultType == typeof(void)
                ? typeof(Action<>).FullName!.Replace("`1", "`" + parameterTypes.Length)
                : typeof(Func<>).FullName!.Replace("`1", "`" + (1 + parameterTypes.Length));

        var rankDelegateType = Type.GetType(rankDelegateTypeName)!;

        return rankDelegateType.MakeGenericType(resultType == typeof(void) ? parameterTypes : parameterTypes.Concat(new[] { resultType }).ToArray());
    }

    public static bool HasDefaultConstructor(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetConstructor(Type.EmptyTypes) != null;
    }

    public static IEnumerable<Type> GetReferencedTypes(this IEnumerable<Type> types)
    {
        return types.GetReferencedTypes(_ => true);
    }

    public static Type GetTopDeclaringType(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.GetTopProperty().DeclaringType!;
    }

    public static bool IsPrimitiveType(this Type value)
    {
        var nullableType = value.GetNullableElementType();

        if (nullableType != null)
        {
            return nullableType.IsPrimitiveType();
        }

        return typeof(Guid) == value
               || typeof(DateTime) == value
               || typeof(TimeSpan) == value
               || value.IsEnum
               || value.IsPrimitive
               || value == typeof(string)
               || value == typeof(decimal);
    }


    public static IEnumerable<FieldInfo> ExpandFields(this Type source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (source.IsCollection())
        {
            return new FieldInfo[0];
        }

        var currentType = source;

        var result = new List<FieldInfo>();
        while ((currentType != null) && (currentType != typeof(object)))
        {
            result.AddRange(currentType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            currentType = currentType.BaseType;
        }
        return result;

    }

    public static List<FieldInfo> GetInstanseFieldsDeep(this Type type)
    {
        Type currentType = type;
        List<FieldInfo> result = new List<FieldInfo>();
        while ((currentType != null) && (currentType != typeof(object)))
        {
            result.AddRange(currentType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            currentType = currentType.BaseType;
        }
        return result;
    }

    public static IEnumerable<Type> GetReferencedTypes(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetReferencedTypes(_ => true);
    }

    public static IEnumerable<Type> GetReferencedTypes(this Type type, Func<PropertyInfo, bool> filter)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return new[] { type }.GetReferencedTypes(filter);
    }

    public static IEnumerable<Type> GetReferencedTypes(this IEnumerable<Type> types, Func<PropertyInfo, bool> filter)
    {
        if (types == null) throw new ArgumentNullException(nameof(types));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        var graph = new HashSet<Type>();

        types.FillReferencedTypes(filter, graph);

        return graph;
    }

    private static void FillReferencedTypes(this IEnumerable<Type> types, Func<PropertyInfo, bool> filter, HashSet<Type> graph)
    {
        if (types == null) throw new ArgumentNullException(nameof(types));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        types.Foreach(type => type.FillReferencedTypes(filter, graph));
    }

    private static void FillReferencedTypes(this Type type, Func<PropertyInfo, bool> filter, HashSet<Type> graph)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (filter == null) throw new ArgumentNullException(nameof(filter));
        if (graph == null) throw new ArgumentNullException(nameof(graph));

        var genericElementType = type.GetCollectionOrArrayElementType() ?? type.GetNullableElementType();

        if (genericElementType != null)
        {
            genericElementType.FillReferencedTypes(filter, graph);
        }
        else if (graph.Add(type))
        {
            if (type.IsInterface)
            {
                type.GetAllInterfaces().FillReferencedTypes(filter, graph);
            }
            else if (type.IsEnum)
            {
                Enum.GetUnderlyingType(type).FillReferencedTypes(filter, graph);
            }
            else
            {
                type.GetProperties()
                    .Where(filter)
                    .Select(property => property.PropertyType)
                    .FillReferencedTypes(filter, graph);
            }
        }
    }

    public static Type? GetCollectionOrArrayElementType(this Type type)
    {
        return type.GetCollectionElementType() ?? type.GetArrayGenericType();
    }
    public static Type? GetArrayGenericType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.IsArray ? type.GetElementType() : null;
    }

    public static Type GetCollectionOrArrayElementTypeOrSelf(this Type type)
    {
        return type.GetCollectionOrArrayElementType() ?? type;
    }


    public static bool IsCollection(this Type type, Func<Type?, bool> elementTypeFilter)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (elementTypeFilter == null) throw new ArgumentNullException(nameof(elementTypeFilter));

        return type.IsCollection() && elementTypeFilter(type.GetCollectionElementType());
    }

    public static Func<TResult> WithLock<TResult>(this Func<TResult> func, object? baseLocker = null)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        var locker = baseLocker ?? new object();

        return () =>
               {
                   lock (locker)
                   {
                       return func();
                   }
               };
    }

    public static MethodInfo? GetInequalityMethod(this Type type, bool withBaseTypes = false)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        if (withBaseTypes)
        {
            return type.GetAllElements(t => t.BaseType).Select(t => t.GetInequalityMethod()).FirstOrDefault(t => t != null);
        }
        else
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(m =>

                m.ReturnType == typeof(bool) && m.Name == "op_Inequality"
                                             && m.GetParameters().Pipe(parameters =>

                                                                           parameters.Length == 2 && parameters.All(parameter => parameter.ParameterType == type)));
        }
    }

    public static Type? GetMemberType(this Type type, string memberName, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (memberName == null) throw new ArgumentNullException(nameof(memberName));

        return type.GetMemberType(memberName, raiseIfNotFound ? () => new Exception($"Member \"{memberName}\" not found in type \"{type.Name}\"") : default(Func<Exception>));
    }

    public static Type? GetMemberType(this Type type, string memberName, Func<Exception>? raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (memberName == null) throw new ArgumentNullException(nameof(memberName));

        var memberType = type.GetMemberType(memberName);

        if (memberType == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return memberType;
    }

    public static Type? GetMemberType(this Type type, string memberName)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (memberName == null) throw new ArgumentNullException(nameof(memberName));

        if (type.IsInterface)
        {
            var property = type.GetAllInterfaceProperties().FirstOrDefault(prop => prop.Name == memberName);

            if (property != null)
            {
                return property.PropertyType;
            }
        }

        return type.GetProperty(memberName).Maybe(p => p.PropertyType) ?? type.GetField(memberName).Maybe(f => f.FieldType);
    }

    public static PropertyInfo? GetProperty(this Type type, string propertyName, StringComparison stringComparison, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        return type.GetProperty(propertyName, stringComparison, raiseIfNotFound ? () => new Exception(
                                                                                   $"Property \"{propertyName}\" not found in type \"{type.Name}\"") : default(Func<Exception>));
    }

    public static PropertyInfo? GetProperty(this Type type, string propertyName, StringComparison stringComparison, Func<Exception>? raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        var property = type.GetProperties().SingleOrDefault(p => p.Name.Equals(propertyName, stringComparison));

        if (property == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return property;
    }

    public static IEnumerable<T> GetStaticPropertyValueList<T>(this Type type, Func<string, bool>? filter = null)
    {
        return from prop in type.GetProperties(BindingFlags.Static | BindingFlags.Public)

               where filter == null || filter(prop.Name)

               where typeof(T).IsAssignableFrom(prop.PropertyType)

               select (T)prop.GetValue(null);
    }

    public static PropertyInfo? GetProperty(this Type type, string propertyName, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        return type.GetProperty(propertyName, raiseIfNotFound ? () => new Exception($"Property \"{propertyName}\" not found in type \"{type.Name}\"") : default(Func<Exception>));
    }

    public static PropertyInfo? GetProperty(this Type type, string propertyName, Func<Exception>? raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        var property = type.GetProperty(propertyName);

        if (property == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return property;
    }

    public static PropertyInfo? GetProperty(this Type type, string propertyName, BindingFlags bindingAttr, bool raise = false)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        var property = type.GetProperty(propertyName, bindingAttr);

        return raise ? property.FromMaybe(() => new Exception($"Property \"{propertyName}\" not found in type:{type.Name}"))
                   : property;
    }

    public static MethodInfo? GetMethod(this Type type, string methodName, BindingFlags bindingFlags, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));

        return type.GetMethod(methodName, bindingFlags, raiseIfNotFound ? () => new Exception($"Method \"{methodName}\" not found") : default(Func<Exception>));
    }

    public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags bindingFlags, Func<Exception>? raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));

        var method = type.GetMethod(methodName, bindingFlags);

        if (method == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return method;
    }


    public static bool IsCollection(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetCollectionElementType() != null;
    }

    public static string ToCSharpFullName(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.ToCSharpName(t => t.FullName);
    }

    public static string ToCSharpShortName(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.ToCSharpName(t => t.Name);
    }


    private static string ToCSharpName(this Type type, Func<Type, string> nameSelector)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (nameSelector == null) throw new ArgumentNullException(nameof(nameSelector));

        if (type.IsGenericType)
        {
            var args = type.GetGenericArguments();

            var prefix = new string(nameSelector(type).TakeWhile(c => c != '`').ToArray());

            return prefix + "<" + args.Join(", ", arg => arg.ToCSharpName(nameSelector)) + ">";
        }
        else if (type.IsArray)
        {
            return type.GetElementType().ToCSharpName(nameSelector) + "[" + new string(',', type.GetArrayRank() - 1) + "]";
        }

        return nameSelector(type);
    }

    public static Type GetNullableElementTypeOrSelf(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetNullableElementType() ?? type;
    }

    public static bool IsNullable(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetNullableElementType() != null;
    }

    public static PropertyInfo GetImplementedProperty(this Type type, PropertyInfo property)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (property == null) throw new ArgumentNullException(nameof(property));

        if (!property.DeclaringType.IsAssignableFrom(type))
        {
            throw new Exception($"Type \"{property.DeclaringType}\" isn't assignable from \"{type}\"");
        }

        if (property.ReflectedType == type)
        {
            return property;
        }
        else if (property.DeclaringType.IsInterface)
        {
            if (type.IsInterface)
            {
                return property;
            }
            else
            {
                var implMethod = type.GetInterfaceMapDictionary(property.DeclaringType)[property.GetMethod];

                var implMethods = new[] { implMethod }.Concat(

                                                              implMethod.GetAllElements(m => m.GetBaseDefinition()).Windowed2((prev, current) => new { prev, current })
                                                                        .TakeWhile(pair => pair.prev != pair.current)
                                                                        .Select(pair => pair.current)).ToHashSet();

                var implPropertyRequest = from t in type.GetAllElements(t => t.BaseType)

                                          from prop in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)

                                          where implMethods.Contains(prop.GetMethod)

                                          select prop;

                return implPropertyRequest.First();
            }
        }
        else
        {
            var implPropertyRequest = from t in type.GetAllElements(t => t.BaseType)

                                      from prop in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)

                                      where prop.GetMethod.GetBaseDefinition() == property.GetMethod.GetBaseDefinition()

                                      select prop;

            return implPropertyRequest.First();
        }
    }

    public static Dictionary<MethodInfo, MethodInfo> GetInterfaceMapDictionary(this Type type, Type interfaceType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

        var interfaceMap = type.GetInterfaceMap(interfaceType);

        return interfaceMap.InterfaceMethods.ZipStrong(interfaceMap.TargetMethods, (m1, m2) => (m1, m2)).ToDictionary();
    }

    public static IEnumerable<TResult> Windowed2<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        using (var enumerator = source.GetEnumerator())
        {
            if (enumerator.MoveNext())
            {
                var prev = enumerator.Current;

                if (enumerator.MoveNext())
                {
                    var next = enumerator.Current;

                    yield return selector(prev, next);

                    while (enumerator.MoveNext())
                    {
                        yield return selector(next, next = enumerator.Current);
                    }
                }
            }
        }
    }

    public static Type GetSuperSet(this Type type, Type otherType, bool safe)
    {
        var res = type.IsSubsetOf(otherType) ? otherType : otherType.IsSubsetOf(type) ? type : null;

        return safe && res == null ? typeof(object) : res;
    }

    public static bool IsSubsetOf(this Type type, Type otherType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (otherType == null) throw new ArgumentNullException(nameof(otherType));

        var oneSetTypesRequest = from set in TypeSetsPriority

                                 select from p1 in set.GetMaybeValue(type)

                                        from p2 in set.GetMaybeValue(otherType)

                                        select p1 < p2;


        var res = oneSetTypesRequest.CollectMaybe().ToArray();

        if (res.Any())
        {
            return res.Single();
        }

        var isSubSetOfFloatRequest = FloatTypeSetPriority.ContainsKey(otherType)
                                     && (SignedTypeSetPriority.ContainsKey(type) || UnsignedTypeSetPriority.ContainsKey(type));

        return isSubSetOfFloatRequest;
    }

    public static IEnumerable<PropertyInfo> GetAllInterfaceProperties(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetAllInterfaces().SelectMany(t => t.GetProperties());
    }

    public static Type[] GetInterfaceImplementationArguments(this Type type, Type interfaceType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

        return type.GetInterfaceImplementationArguments(interfaceType, v => v);
    }

    public static MethodInfo? GetMethod(this Type type, string methodName, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));

        return type.GetMethod(methodName, raiseIfNotFound ? () => new Exception($"Method \"{methodName}\" not found") : default(Func<Exception>));
    }

    public static MethodInfo? GetMethod(this Type type, string methodName, Func<Exception> raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));

        var method = type.GetMethod(methodName);

        if (method == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return method;
    }

    private static readonly Dictionary<Type, int> SignedTypeSetPriority = new Dictionary<Type, int>
                                                                          {
                                                                                  { typeof(short), 0 },
                                                                                  { typeof(int), 1 },
                                                                                  { typeof(long), 2 },
                                                                          };

    private static readonly Dictionary<Type, int> UnsignedTypeSetPriority = new Dictionary<Type, int>
                                                                            {
                                                                                    { typeof(ushort), 0 },
                                                                                    { typeof(uint), 1 },
                                                                                    { typeof(ulong), 2 },
                                                                            };

    private static readonly Dictionary<Type, int> FloatTypeSetPriority = new Dictionary<Type, int>
                                                                         {
                                                                                 { typeof(float), 0 },
                                                                                 { typeof(double), 1 },
                                                                                 { typeof(decimal), 2 },
                                                                         };

    private static readonly List<Dictionary<Type, int>> TypeSetsPriority = new List<Dictionary<Type, int>>
                                                                           {
                                                                                   SignedTypeSetPriority, UnsignedTypeSetPriority, FloatTypeSetPriority
                                                                           };
}
