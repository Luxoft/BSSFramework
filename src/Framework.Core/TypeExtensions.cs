using System.Collections.ObjectModel;
using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;
using CommonFramework.Maybe;

namespace Framework.Core;

public static class TypeExtensions
{
    public static PropertyInfo GetProperty(this Type type, string propertyName, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        return type.GetProperty(propertyName, raiseIfNotFound ? () => new Exception($"Property \"{propertyName}\" not found in type \"{type.Name}\"") : default(Func<Exception>));
    }

    public static PropertyInfo GetProperty(this Type type, string propertyName, Func<Exception> raiseIfNotFoundException)
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

    public static PropertyInfo GetProperty(this Type type, string propertyName, BindingFlags bindingAttr, bool raise = false)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        var property = type.GetProperty(propertyName, bindingAttr);

        return raise ? property.FromMaybe(() => new Exception($"Property \"{propertyName}\" not found in type:{type.Name}"))
                   : property;
    }

    public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags bindingFlags, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));

        return type.GetMethod(methodName, bindingFlags, raiseIfNotFound ? () => new Exception($"Method \"{methodName}\" not found") : default(Func<Exception>));
    }

    public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags bindingFlags, Func<Exception> raiseIfNotFoundException)
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
