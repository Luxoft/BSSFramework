using System.Collections.ObjectModel;
using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;
using CommonFramework.Maybe;

using Framework.Core;

namespace Framework.Persistent;

public static class PropertyPathExtensions
{
    private static readonly IDictionaryCache<Type, IDictionaryCache<Tuple<PropertyInfo, bool>, PropertyPath?>> SinglePathCache = new DictionaryCache<Type, IDictionaryCache<Tuple<PropertyInfo, bool>, PropertyPath?>>(attributeType =>

            new Func<IDictionaryCache<Tuple<PropertyInfo, bool>, PropertyPath?>>(GetInternalSingleCache<PathAttribute>)
                    .CreateGenericMethod(attributeType)
                    .Invoke<IDictionaryCache<Tuple<PropertyInfo, bool>, PropertyPath>>(null)).WithLock();

    private static readonly IDictionaryCache<Type, IDictionaryCache<PropertyInfo, ReadOnlyCollection<PropertyPath>>> ManyPathCache = new DictionaryCache<Type, IDictionaryCache<PropertyInfo, ReadOnlyCollection<PropertyPath>>>(attributeType =>

            new Func<IDictionaryCache<PropertyInfo, ReadOnlyCollection<PropertyPath>>>(GetInternalManyCache<PathAttribute>)
                    .CreateGenericMethod(attributeType)
                    .Invoke<IDictionaryCache<PropertyInfo, ReadOnlyCollection<PropertyPath>>>(null)).WithLock();

    private static IDictionaryCache<Tuple<PropertyInfo, bool>, PropertyPath?> GetInternalSingleCache<TAttribute>()
            where TAttribute : Attribute, IPathAttribute
    {
        return new DictionaryCache<Tuple<PropertyInfo, bool>, PropertyPath?>((propertyPair, cache) =>
                                                                            {
                                                                                var property = propertyPair.Item1;

                                                                                var recurse = propertyPair.Item2;

                                                                                var pathRequest = from pathAttribute in property.GetCustomAttribute<TAttribute>().ToMaybe()

                                                                                    let basePath = property.ReflectedType!.GetPropertyPath(pathAttribute.Path)

                                                                                    select recurse && !basePath.SequenceEqual(new[] { property }) ? new PropertyPath(basePath.SelectMany(prop => (IEnumerable<PropertyInfo>)cache.GetValue(prop, true) ?? new[] { prop })) : basePath;

                                                                                return pathRequest.GetValueOrDefault();
                                                                            }).WithLock();
    }

    private static IDictionaryCache<PropertyInfo, ReadOnlyCollection<PropertyPath>> GetInternalManyCache<TAttribute>()
            where TAttribute : Attribute, IPathAttribute
    {
        return new DictionaryCache<PropertyInfo, ReadOnlyCollection<PropertyPath>>(property =>
                                                                                   {
                                                                                       var pathsRequest = from pathAttribute in property.GetCustomAttributes<TAttribute>()

                                                                                           select property.ReflectedType!.GetPropertyPath(pathAttribute.Path);

                                                                                       return pathsRequest.ToReadOnlyCollection();
                                                                                   }).WithLock();
    }

    extension(Type domainObjectType)
    {
        private PropertyPath GetPropertyPath(string path)
        {
            if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (path.Any())
            {
                return path.Split('.').Aggregate(

                    new { LastType = domainObjectType, TotalPath = Enumerable.Empty<PropertyInfo>() },

                    (state, propertyName) =>
                    {
                        var property = state.LastType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, true);

                        return new { LastType = property.GetNestedType(), TotalPath = state.TotalPath.Concat(new[] { property }) };
                    },

                    state => state.TotalPath.ToPropertyPath());
            }
            else
            {
                return PropertyPath.Empty;
            }
        }

        public PropertyPath GetPropertyPath<TAttribute>()
            where TAttribute : Attribute, IPathAttribute
        {
            if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));

            var attr = domainObjectType.GetCustomAttribute<TAttribute>()!;

            return domainObjectType.GetPropertyPath(attr.Path);
        }
    }

    extension(PropertyInfo property)
    {
        public PropertyPath? GetPropertyPath<TAttribute>(bool recurse)
            where TAttribute : Attribute, IPathAttribute
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            return SinglePathCache[typeof(TAttribute)].GetValue(property, recurse);
        }

        public ReadOnlyCollection<PropertyPath> GetPropertyPaths<TAttribute>()
            where TAttribute : Attribute, IPathAttribute
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            return ManyPathCache[typeof(TAttribute)][property];
        }

        public Type GetNestedType()
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            return property.PropertyType.GetCollectionElementType() ?? property.PropertyType;
        }

        public PropertyPath? GetExpandPath(bool validate = true)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var expandPath = property.GetPropertyPath<ExpandPathAttribute>(true);

            if (validate && expandPath is not null)
            {
                var isValid = expandPath.IsEmpty ? property.PropertyType.SafeEquals(property.ReflectedType!)
                                  : property.PropertyType.SafeIsAssignableFrom(expandPath.Last().PropertyType);

                if (!isValid)
                {
                    throw new Exception($"Property \"{property.Name}\" of Type \"{property.ReflectedType!.Name}\" has invalid ExpandPath");
                }
            }

            return expandPath;
        }

        public PropertyPath GetExpandPathOrSelf()
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            return property.GetExpandPath() ?? new PropertyPath(new[] { property });
        }
    }

    public static PropertyPath WithExpand(this PropertyPath propertyPath)
    {
        if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));

        return propertyPath.SelectMany(prop => prop.GetExpandPathOrSelf()).ToPropertyPath();
    }
}
