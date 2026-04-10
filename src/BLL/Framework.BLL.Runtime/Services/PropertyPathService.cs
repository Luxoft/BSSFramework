using System.Collections.Concurrent;
using System.Collections.Immutable;

using System.Reflection;

using CommonFramework;

using Framework.BLL.Domain.Persistent.Attributes;
using Framework.BLL.Extensions;
using Framework.Core;
using Framework.ExtendedMetadata;

namespace Framework.BLL.Services;

public class PropertyPathService(IMetadataProxyProvider metadataProxyProvider) : IPropertyPathService
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Tuple<PropertyInfo, bool>, PropertyPath?>> singlePathCache = [];

    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<PropertyInfo, ImmutableArray<PropertyPath>>> manyPathCache = [];

    private readonly ConcurrentDictionary<PropertyInfo,  PropertyPath?> expandPathCache = [];

    public PropertyPath? TryGetPropertyPath<TAttribute>(PropertyInfo property, bool recurse)
        where TAttribute : Attribute, IPathAttribute =>

        this.singlePathCache
            .GetOrAdd(typeof(TAttribute), _ => [])
            .GetOrAdd(
                Tuple.Create(property, recurse),
                _ =>
                {
                    var pathRequest =

                        from pathAttribute in metadataProxyProvider.Wrap(property).GetCustomAttribute<TAttribute>().ToMaybe()

                        let basePath = pathAttribute.GetPropertyPath(property.ReflectedType!)

                        select recurse && !basePath.SequenceEqual([property])
                                   ? new PropertyPath(
                                       basePath.SelectMany(prop => (IEnumerable<PropertyInfo>?)this.TryGetPropertyPath<TAttribute>(prop, true) ?? [prop]))
                                   : basePath;

                    return pathRequest.GetValueOrDefault();
                });

    public ImmutableArray<PropertyPath> GetPropertyPaths<TAttribute>(PropertyInfo property)
        where TAttribute : Attribute, IPathAttribute =>

        this.manyPathCache
            .GetOrAdd(typeof(TAttribute), _ => [])
            .GetOrAdd(
                property,
                _ =>
                [
                    .. metadataProxyProvider.Wrap(property).GetCustomAttributes<TAttribute>()
                                            .Select(pathAttribute => pathAttribute.GetPropertyPath(property.ReflectedType!))
                ]);

    public PropertyPath? TryGetExpandPath(PropertyInfo property) =>
        this.expandPathCache.GetOrAdd(
            property,
            _ =>
            {
                var expandPath = this.TryGetPropertyPath<ExpandPathAttribute>(property, true);

                if (expandPath is not null)
                {
                    var isValid = expandPath.IsEmpty
                                      ? property.PropertyType.SafeEquals(property.ReflectedType!)
                                      : property.PropertyType.SafeIsAssignableFrom(expandPath.Last().PropertyType);

                    if (!isValid)
                    {
                        throw new Exception($"Property \"{property.Name}\" of Type \"{property.ReflectedType!.Name}\" has invalid ExpandPath");
                    }
                }

                return expandPath;
            });

    public PropertyPath GetExpandPathOrSelf(PropertyInfo property) => this.TryGetExpandPath(property) ?? new PropertyPath([property]);

    public PropertyPath WithExpand(PropertyPath propertyPath) => propertyPath.SelectMany(this.GetExpandPathOrSelf).ToPropertyPath();
}
