using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

using Anch.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Core;

[DebuggerDisplay("{DebuggerDisplay}")]
public class PropertyPath(IEnumerable<PropertyInfo> properties) : ReadOnlyCollection<PropertyInfo>(properties.ToList()), IEquatable<PropertyPath>
{
    public PropertyPath(Type sourceType, IEnumerable<string> path)
        : this(GetProperties(sourceType, path))
    {
    }

    public PropertyPath(Type sourceType, string path)
        : this(sourceType, path.Split('.', StringSplitOptions.RemoveEmptyEntries))
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


    private static IEnumerable<PropertyInfo> GetProperties(Type sourceType, IEnumerable<string> properties) =>

        properties.Scan(
            default(PropertyInfo),
            (prevProperty, propertyName) =>
            {
                var currentType = prevProperty == null ? sourceType : prevProperty.PropertyType.GetCollectionElementTypeOrSelf();

                return currentType.GetRequiredProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            }).Skip(1).Select(v => v!);
}
