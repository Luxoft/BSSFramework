using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class FieldInfoExtensions
    {
        public static LambdaExpression ToSetLambdaExpression([NotNull] this FieldInfo field, Type sourceType = null)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return FieldLambdaCache.SetLambdaCache.GetValue(field, sourceType ?? field.ReflectedType);
        }

        public static Expression<Action<TSource, TField>> ToSetLambdaExpression<TSource, TField>([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return FieldLambdaCache<TSource, TField>.SetLambdaCache[field];
        }

        public static LambdaExpression ToLambdaExpression([NotNull] this FieldInfo field, Type sourceType = null)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return FieldLambdaCache.LambdaCache.GetValue(field, sourceType ?? field.ReflectedType);
        }

        public static Expression<Func<TSource, TField>> ToLambdaExpression<TSource, TField>([NotNull] this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            return FieldLambdaCache<TSource, TField>.LambdaCache[field];
        }
        public static Action<TSource, TProperty> GetSetValueAction<TSource, TProperty>(this FieldInfo field)
        {
            return FieldLambdaCache<TSource, TProperty>.SetActionCache[field];
        }

        private static class FieldLambdaCache
        {
            public static readonly IDictionaryCache<Tuple<FieldInfo, Type>, LambdaExpression> LambdaCache = new DictionaryCache<Tuple<FieldInfo, Type>, LambdaExpression>(tuple =>
            {
                var field = tuple.Item1;
                var fieldSource = tuple.Item2;

                var parameter = Expression.Parameter(fieldSource);

                return Expression.Lambda(Expression.Field(parameter, field), parameter);
            }).WithLock();

            public static readonly IDictionaryCache<Tuple<FieldInfo, Type>, LambdaExpression> SetLambdaCache = new DictionaryCache<Tuple<FieldInfo, Type>, LambdaExpression>(tuple =>
            {
                var field = tuple.Item1;
                var fieldSource = tuple.Item2;

                var sourceParameter = Expression.Parameter(fieldSource);
                var valueParameter = Expression.Parameter(field.FieldType);

                var fieldExp = Expression.Field(sourceParameter, field);

                return Expression.Lambda(Expression.Assign(fieldExp, valueParameter), sourceParameter, valueParameter);
            }).WithLock();
        }

        private static class FieldLambdaCache<TSource, TField>
        {
            public static readonly IDictionaryCache<FieldInfo, Expression<Func<TSource, TField>>> LambdaCache = new DictionaryCache<FieldInfo, Expression<Func<TSource, TField>>>(field =>
               (Expression<Func<TSource, TField>>)field.ToLambdaExpression(typeof(TSource))).WithLock();

            public static readonly IDictionaryCache<FieldInfo, Func<TSource, TField>> FuncCache = new DictionaryCache<FieldInfo, Func<TSource, TField>>(field =>
                LambdaCache[field].Compile()).WithLock();


            public static readonly IDictionaryCache<FieldInfo, Expression<Action<TSource, TField>>> SetLambdaCache = new DictionaryCache<FieldInfo, Expression<Action<TSource, TField>>>(field =>
                (Expression<Action<TSource, TField>>)field.ToSetLambdaExpression(typeof(TSource))).WithLock();

            public static readonly IDictionaryCache<FieldInfo, Action<TSource, TField>> SetActionCache = new DictionaryCache<FieldInfo, Action<TSource, TField>>(field =>
                SetLambdaCache[field].Compile()).WithLock();
        }
    }
}
