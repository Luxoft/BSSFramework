using System.Collections.ObjectModel;
using System.Reflection;

namespace Framework.Core;

public static class TypeExtensions
{
    public static IEnumerable<T> GetStaticPropertyValueList<T>(this Type type, Func<string, bool> filter = null)
    {
        return from prop in type.GetProperties(BindingFlags.Static | BindingFlags.Public)

               where filter == null || filter(prop.Name)

               where typeof(T).IsAssignableFrom(prop.PropertyType)

               select (T)prop.GetValue(null);
    }

    public static Dictionary<MethodInfo, MethodInfo> GetInterfaceMapDictionary(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetAllInterfaces().SelectMany(type.GetInterfaceMapDictionary).ToDictionary();
    }

    public static Dictionary<MethodInfo, MethodInfo> GetInterfaceMapDictionary(this Type type, Type interfaceType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

        var interfaceMap = type.GetInterfaceMap(interfaceType);

        return interfaceMap.InterfaceMethods.ZipStrong(interfaceMap.TargetMethods, (m1, m2) => (m1, m2)).ToDictionary();
    }

    public static IEnumerable<MemberInfo> GetMemberAccessors(this Type type, BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetFields(flags).Concat<MemberInfo>(type.GetProperties(flags));
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

    public static Type MakeGenericType(this Type type, Tuple<Type, Type> args)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (args == null) throw new ArgumentNullException(nameof(args));

        return type.MakeGenericType(args.Item1, args.Item2);
    }

    public static Type MakeGenericType(this Type type, Tuple<Type, Type, Type> args)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (args == null) throw new ArgumentNullException(nameof(args));

        return type.MakeGenericType(args.Item1, args.Item2, args.Item3);
    }

    public static Type MakeGenericType(this Type type, Tuple<Type, Type, Type, Type> args)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (args == null) throw new ArgumentNullException(nameof(args));

        return type.MakeGenericType(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Type[] GetGenericArguments(this Type type, Type baseGenericType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (baseGenericType == null) throw new ArgumentNullException(nameof(baseGenericType));

        if (baseGenericType.IsClass)
        {
            return type.GetAllElements(t => t.BaseType)
                       .Select(t => t.GetGenericTypeImplementationArguments(baseGenericType, args => args))
                       .SingleOrDefault(args => args != null);
        }
        else if (baseGenericType.IsInterface)
        {
            return type.GetAllInterfaces()
                       .Where(i => i.IsGenericTypeImplementation(baseGenericType))
                       .Select(i => i.GetGenericArguments())
                       .SingleOrDefault();
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(baseGenericType));
        }
    }


    public static bool IsPrimitiveTypeLegacy(this Type value)
    {
        return value.IsPrimitive || value == typeof(string) || value == typeof(decimal);
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

    public static IEnumerable<IEnumerable<FieldInfo>> ExpandToCRLTypes(this Type sourceType)
    {
        var fields = sourceType.ExpandFields();

        var partialResult = fields.Partial(z => z.FieldType.IsPrimitiveType(),
                                           (primitive, composite) => new { primitive, composite });

        yield return partialResult.primitive;

        foreach (var fieldInfo in partialResult.composite)
        {
            foreach (var field in fieldInfo.FieldType.ExpandToCRLTypes())
            {
                yield return new[] { fieldInfo }.Concat(field);
            }
        }
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

    public static bool IsArray(this Type type, Func<Type, bool> elementTypeFilter)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (elementTypeFilter == null) throw new ArgumentNullException(nameof(elementTypeFilter));

        return type.IsArray && elementTypeFilter(type.GetElementType());
    }

    public static bool IsPrimitiveTypeArray(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.IsArray(el => el.IsPrimitiveType());
    }



    public static bool IsArray(this Type type, Type elementType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (elementType == null) throw new ArgumentNullException(nameof(elementType));

        return type.IsArray(v => v == elementType);
    }

    public static bool IsValueTypeArray(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.IsArray(v => v.IsValueType);
    }

    public static Type GetTopDeclaringType(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.GetTopProperty().DeclaringType;
    }


    public static bool IsAssignableFrom(this PropertyInfo property, PropertyInfo nestedProp, bool byGet = true)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return nestedProp.GetBaseProperties(byGet).Contains(property);
    }

    public static bool IsTopProperty(this PropertyInfo property, bool byGet = true)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.GetTopProperty(byGet) == property;
    }



    public static bool IsObservableCollection(this Type type, Type argumentGenericType = null)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.IsGenericTypeImplementation(typeof(ObservableCollection<>), argumentGenericType.Maybe(arg => new[] { arg }));
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

    public static IEnumerable<Type> GetReferencedTypes(this IEnumerable<Type> types)
    {
        return types.GetReferencedTypes(_ => true);
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

    public static bool HasDefaultConstructor(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetConstructor(Type.EmptyTypes) != null;
    }

    public static string GetSafeFullName(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.Namespace + "." + type.Name.TakeWhileNot("`");
    }


    public static bool IsAssignableTo(this Type type, Type baseType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (baseType == null) throw new ArgumentNullException(nameof(baseType));

        return baseType.IsAssignableFrom(type);
    }

    public static bool IsAssignableToAll(this Type type, IEnumerable<Type> baseTypes)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (baseTypes == null) throw new ArgumentNullException(nameof(baseTypes));

        return baseTypes.All(type.IsAssignableTo);
    }

    public static bool IsAssignableToAny(this Type type, IEnumerable<Type> baseTypes)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (baseTypes == null) throw new ArgumentNullException(nameof(baseTypes));

        return baseTypes.Any(type.IsAssignableTo);
    }

    public static bool IsAssignableFromAny(this Type type, IEnumerable<Type> baseTypes)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (baseTypes == null) throw new ArgumentNullException(nameof(baseTypes));

        return baseTypes.Any(type.IsAssignableFrom);
    }

    public static Type ToDelegateType(this IEnumerable<Type> preParameterTypes, Type resultType)
    {
        if (preParameterTypes == null) throw new ArgumentNullException(nameof(preParameterTypes));
        if (resultType == null) throw new ArgumentNullException(nameof(resultType));

        var parameterTypes = preParameterTypes.ToArray();

        var rankDelegateTypeName =

                resultType == typeof(void)
                        ? typeof(Action<>).FullName.Replace("`1", "`" + parameterTypes.Length)
                        : typeof(Func<>).FullName.Replace("`1", "`" + (1 + parameterTypes.Length));

        var rankDelegateType = Type.GetType(rankDelegateTypeName);

        return rankDelegateType.MakeGenericType(resultType == typeof(void) ? parameterTypes : parameterTypes.Concat(new[] { resultType }).ToArray());
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
