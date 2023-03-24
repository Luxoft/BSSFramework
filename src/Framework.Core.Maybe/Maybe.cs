using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Framework.Core;

public interface IMaybe
{
    Type ValueType { get; }

    bool HasValue { get; }
}

public interface IMaybe<out T> : IMaybe
{
}

[DataContract(Name = "MaybeOf{0}", Namespace = "Framework.Core")]
[KnownType("GetKnownTypes")]
public abstract class Maybe<T> : IMaybe<T>, IEquatable<Maybe<T>>
{
    internal Maybe()
    {
    }

    public bool HasValue => !(this is Nothing<T>);

    public static IEnumerable<Type> GetKnownTypes()
    {
        yield return typeof(Nothing<T>);
        yield return typeof(Just<T>);
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as Maybe<T>);
    }

    public override int GetHashCode()
    {
        return (this as Just<T>).Maybe(just => just.Value == null ? 0 : just.Value.GetHashCode());
    }

    public bool Equals(Maybe<T> other)
    {
        return !object.ReferenceEquals(other, null)
               && ((!this.HasValue && !other.HasValue)
                   || this.MaybeJustValue(v1 => other.MaybeJustValue(v2 => EqualityComparer<T>.Default.Equals(v1, v2))));
    }

    public static bool operator == (Maybe<T> v1, Maybe<T> v2)
    {
        return object.ReferenceEquals(v1, v2) || (!object.ReferenceEquals(v1, null) && v1.Equals(v2));
    }

    public static bool operator !=(Maybe<T> v1, Maybe<T> v2)
    {
        return !(v1 == v2);
    }

    public static readonly Maybe<T> Nothing = new Nothing<T>();

    Type IMaybe.ValueType => typeof(T);
}
