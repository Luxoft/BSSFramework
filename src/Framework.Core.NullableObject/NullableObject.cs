using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Framework.Core
{
    [DataContract(Name = "NullableObjectOf{0}", Namespace = "Framework.Core")]
    public class NullableObject<T> : IEquatable<NullableObject<T>>
    {
        public NullableObject(T value)
        {
            this.Value = value;
        }

        [DataMember]
        public T Value { get; private set; }


        public override string ToString()
        {
            return this.Value.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as NullableObject<T>);
        }

        public bool Equals(NullableObject<T> other)
        {
            return !object.ReferenceEquals(other, null) && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
        }


        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }

    public static class NullableObject
    {
        public static NullableObject<TResult> Select<TSource, TResult>(this NullableObject<TSource> source, Func<TSource, TResult> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source == null ? null : Return(selector(source.Value));
        }

        public static NullableObject<TResult> SelectMany<TSource, TResult>(this NullableObject<TSource> source, Func<TSource, NullableObject<TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source == null ? null : selector(source.Value);
        }

        public static NullableObject<T> Return<T>(T value)
        {
            return new NullableObject<T>(value);
        }

        public static NullableObject<T> OfReference<T>(T value)
            where T : class
        {
            return value.Maybe(v => Return(v));
        }

        public static NullableObject<T> OfNullable<T>(T? value)
            where T : struct
        {
            return value.MaybeNullable(v => Return(v));
        }

        public static Maybe<T> ToMaybe<T>(this NullableObject<T> nullableObject)
        {
            return nullableObject == null ? Maybe<T>.Nothing : Maybe.Return(nullableObject.Value);
        }

        public static void Match<TSource>(this NullableObject<TSource> maybeValue, Action<TSource> fromJustAction, Action fromNothingAction = null)
        {
            if (fromJustAction == null) throw new ArgumentNullException(nameof(fromJustAction));

            maybeValue.ToMaybe().Match(fromJustAction, fromNothingAction);
        }

        public static TResult Match<TSource, TResult>(this NullableObject<TSource> maybeValue, Func<TSource, TResult> fromJustResult, Func<TResult> fromNothingResult)
        {
            if (fromJustResult == null) throw new ArgumentNullException(nameof(fromJustResult));
            if (fromNothingResult == null) throw new ArgumentNullException(nameof(fromNothingResult));

            return maybeValue.ToMaybe().Match(fromJustResult, fromNothingResult);
        }
    }

    public static class MaybeNullableObject
    {
        public static Maybe<NullableObject<TResult>> SelectMany<TSource, TResult>(this Maybe<NullableObject<TSource>> source, Func<TSource, Maybe<NullableObject<TResult>>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));


            return source.Match(

                result1 => result1.Match(

                    nullRes1 => selector(nullRes1).Match(

                        result2 => result2.Match(

                            nullRes2 => Maybe.Return(NullableObject.Return(nullRes2)),

                            () => Maybe.Return(default(NullableObject<TResult>))),

                        () => Maybe<NullableObject<TResult>>.Nothing),

                    () => Maybe.Return(default(NullableObject<TResult>))),

                () => Maybe<NullableObject<TResult>>.Nothing);
        }
    }
}
