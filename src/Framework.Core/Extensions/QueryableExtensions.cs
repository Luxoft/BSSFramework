using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core
{
    public static class QueryableExtensions
    {
        public static List<TResult> ToList<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).ToList();
        }

        public static TResult[] ToArray<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).ToArray();
        }

        public static HashSet<TResult> ToHashSet<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).ToHashSet();
        }

        public static Array ToBaseArray(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(source.ElementType).Invoke(null, new object[] { source }) as Array;
        }

        //#region SubSource



        //private static readonly AnonymousTypeByFieldBuilder SubSourceAnonymousTypeBuilder = new AnonymousTypeByFieldBuilder("_subSourceAnonymousTypeBuilder");


        //public static IQueryable GetSubSource<TSource>(this IQueryable<TSource> source, IEnumerable<KeyValuePair<string, string>> paths)
        //{
        //    var properties = paths.Select(pair => new KeyValuePair<string, Type>(pair.Key,
        //                                                                         pair.Value
        //                                                                             .Split('.')
        //                                                                             .Aggregate(typeof(TSource),
        //                                                                                        (t, path) => t.GetProperty(path, true).PropertyType)))
        //                          .ToDictionary();

        //    var anonymousType = SubSourceAnonymousTypeBuilder.GetAnonymousType(typeof(TSource).Name, properties, true);

        //    var subSourceBaseMethod = new Func<IQueryable<TSource>, IEnumerable<KeyValuePair<string, string>>, IQueryable<TSource>>(QueryableExtensions.GetSubSource<TSource, TSource>).Method.GetGenericMethodDefinition();

        //    return subSourceBaseMethod.MakeGenericMethod(typeof(TSource), anonymousType).Invoke(null, new object[] { source, paths }) as IQueryable;
        //}

        //public static IQueryable<TResult> GetSubSource<TSource, TResult>(this IQueryable<TSource> source, IEnumerable<KeyValuePair<string, string>> paths)
        //{
        //    var sourceParam = Expression.Parameter(typeof(TSource), "sourceElement");

        //    var ctor = Expression.New(typeof(TResult).GetConstructor(Type.EmptyTypes));

        //    var binds = paths.ToArray(pair =>
        //    {
        //        var bindExpr = pair.Value.Split('.').Aggregate((Expression)sourceParam, Expression.Property);

        //        return (MemberBinding)Expression.Bind(typeof(TResult).GetField(pair.Key), bindExpr);
        //    });


        //    var selectExpr = Expression.Lambda<Func<TSource, TResult>>(Expression.MemberInit(ctor, binds), sourceParam);

        //    return source.Select(selectExpr);
        //}

        //#endregion
    }
}
