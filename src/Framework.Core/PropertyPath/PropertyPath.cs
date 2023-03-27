using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core;

[DebuggerDisplay("{DebuggerDisplay}")]
public class PropertyPath : ReadOnlyCollection<PropertyInfo>, IEquatable<PropertyPath>
{
    public PropertyPath(IEnumerable<PropertyInfo> list)
            : this(list.ToList())
    {
    }

    public PropertyPath(IList<PropertyInfo> list)
            : base(list)
    {
        //TODO: add path validate
    }


    private string DebuggerDisplay
    {
        get { return this.ToString(); }
    }

    public bool IsEmpty
    {
        get { return !this.Any(); }
    }

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


    public override string ToString()
    {
        return this.Join(".", prop => prop.Name);
    }


    public bool Equals(PropertyPath other)
    {
        return other != null && this.SequenceEqual(other);
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as PropertyPath);
    }

    public override int GetHashCode()
    {
        return this.Count;
    }

    public bool StartsWith([NotNull] PropertyPath otherPath)
    {
        if (otherPath == null) throw new ArgumentNullException(nameof(otherPath));

        return this.Count >= otherPath.Count && this.Take(otherPath.Count).SequenceEqual(otherPath);
    }


    public static PropertyPath operator +([NotNull] PropertyInfo propertyInfo, [NotNull] PropertyPath path)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
        if (path == null) throw new ArgumentNullException(nameof(path));

        return new[] { propertyInfo }.Concat(path).ToPropertyPath();
    }

    public static PropertyPath operator +([NotNull] PropertyPath path, [NotNull] PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
        if (path == null) throw new ArgumentNullException(nameof(path));

        return path.Concat(new[] { propertyInfo }).ToPropertyPath();
    }

    public static bool operator ==([NotNull] PropertyPath path1, [NotNull] PropertyPath path2)
    {
        return object.ReferenceEquals(path1, path2) || (!object.ReferenceEquals(path1, null) && path1.Equals(path2));
    }

    public static bool operator !=([NotNull] PropertyPath path1, [NotNull] PropertyPath path2)
    {
        return !(path1 == path2);
    }

    public static readonly PropertyPath Empty = new PropertyPath(new PropertyInfo[0]);
}
