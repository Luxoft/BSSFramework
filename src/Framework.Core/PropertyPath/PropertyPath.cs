using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

using CommonFramework;

// ReSharper disable once CheckNamespace
namespace Framework.Core;

[DebuggerDisplay("{DebuggerDisplay}")]
public class PropertyPath(List<PropertyInfo> list) : ReadOnlyCollection<PropertyInfo>(list), IEquatable<PropertyPath>
{
    public PropertyPath(IEnumerable<PropertyInfo> list)
        : this(list.ToList())
    {
    }

    //TODO: add path validate

    private string DebuggerDisplay => this.ToString();

    public bool IsEmpty => !this.Any();

    public PropertyInfo Head
    {
        get
        {
            if (this.IsEmpty)
            {
                throw new Exception("Empty Path");
            }
            else
            {
                return this[0];
            }
        }
    }

    public PropertyPath Tail
    {
        get
        {
            if (this.IsEmpty)
            {
                throw new Exception("Empty Path");
            }
            else
            {
                return this.Skip(1).ToPropertyPath();
            }
        }
    }


    public override string ToString() => this.Join(".", prop => prop.Name);

    public bool Equals(PropertyPath? other) => other is not null && this.SequenceEqual(other);

    public override bool Equals(object? obj) => this.Equals(obj as PropertyPath);

    public override int GetHashCode() => this.Count;

    public bool StartsWith(PropertyPath otherPath) => this.Count >= otherPath.Count && this.Take(otherPath.Count).SequenceEqual(otherPath);

    public static PropertyPath operator +(PropertyInfo propertyInfo, PropertyPath path) => new[] { propertyInfo }.Concat(path).ToPropertyPath();

    public static PropertyPath operator +(PropertyPath path, PropertyInfo propertyInfo) => path.Concat([propertyInfo]).ToPropertyPath();

    public static bool operator ==(PropertyPath path1, PropertyPath path2) => ReferenceEquals(path1, path2) || (!ReferenceEquals(path1, null) && path1.Equals(path2));

    public static bool operator !=(PropertyPath path1, PropertyPath path2) => !(path1 == path2);

    public new static PropertyPath Empty { get; } = new([]);

    public static PropertyPath Create(Type sourceType, string[] properties)
    {
        var typedProperties = properties.Scan(
            default(PropertyInfo),
            (prevProperty, propertyName) =>
            {
                var currentType = prevProperty == null ? sourceType : prevProperty.PropertyType.GetCollectionElementTypeOrSelf();

                return currentType.GetRequiredProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            }).Skip(1).Select(v => v!);

        return new PropertyPath(typedProperties);
    }
}
