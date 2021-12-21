using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL.Tracking
{
    public static class TrackingServiceExtensions
    {
        public static IEnumerable<TRemovedItem> GetRemovedItems<TPersistentDomainObjectBase, TDomainObject, TRemovedItem>(
            [NotNull] this ITrackingService<TPersistentDomainObjectBase> trackingService,
            [NotNull] TDomainObject source)
                                            where TPersistentDomainObjectBase : class
                                            where TDomainObject : class, TPersistentDomainObjectBase
                                            where TRemovedItem : class, TPersistentDomainObjectBase
        {
            if (trackingService == null) throw new ArgumentNullException(nameof(trackingService));
            if (source == null) throw new ArgumentNullException(nameof(source));

            return InternalHelper<TPersistentDomainObjectBase, TDomainObject, TRemovedItem>.SourceFunc(trackingService, source);
        }

        private static class InternalHelper<TPersistentDomainObjectBase, TDomainObject, TRemovedItem>
            where TPersistentDomainObjectBase : class
            where TDomainObject : class, TPersistentDomainObjectBase
            where TRemovedItem : class, TPersistentDomainObjectBase
        {
            public static readonly Func<ITrackingService<TPersistentDomainObjectBase>, TDomainObject, IEnumerable<TRemovedItem>> SourceFunc = GetSourceFunc();

            private static Func<ITrackingService<TPersistentDomainObjectBase>, TDomainObject, IEnumerable<TRemovedItem>> GetSourceFunc()
            {
                return typeof(TDomainObject).GetPropertyPaths(typeof(TRemovedItem)).Select(GetSourceFunc).Sum();
            }

            private static Func<ITrackingService<TPersistentDomainObjectBase>, TDomainObject, IEnumerable<TRemovedItem>> GetSourceFunc([NotNull] PropertyPath propertyPath)
            {
                if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));

                var prop = propertyPath.Head;

                var directFunc = new Func<PropertyPath, Expression<Func<TDomainObject, IEnumerable<TDomainObject>>>, Func<ITrackingService<TPersistentDomainObjectBase>, TDomainObject, IEnumerable<TRemovedItem>>>(GetSourceFuncDirect)
                                .CreateGenericMethod(prop.PropertyType.GetCollectionElementType())
                                .Invoke<Func<ITrackingService<TPersistentDomainObjectBase>, TDomainObject, IEnumerable<TRemovedItem>>>(null, propertyPath, prop.ToLambdaExpression());

                if (propertyPath.Count > 1)
                {
                    var subMergeFunc = new Func<PropertyPath, Expression<Func<TDomainObject, IEnumerable<TDomainObject>>>, Func<ITrackingService<TPersistentDomainObjectBase>, TDomainObject, IEnumerable<TRemovedItem>>>(GetSourceFuncSubMerge)
                                      .CreateGenericMethod(prop.PropertyType.GetCollectionElementType())
                                      .Invoke<Func<ITrackingService<TPersistentDomainObjectBase>, TDomainObject, IEnumerable<TRemovedItem>>>(null, propertyPath, prop.ToLambdaExpression());

                    return new[] { directFunc, subMergeFunc }.Sum();
                }
                else
                {
                    return directFunc;
                }
            }

            private static Func<ITrackingService<TPersistentDomainObjectBase>, TDomainObject, IEnumerable<TRemovedItem>> GetSourceFuncDirect<TDetail>([NotNull] PropertyPath propertyPath, [NotNull] Expression<Func<TDomainObject, IEnumerable<TDetail>>> propertyExpression)
            {
                if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));
                if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

                var getItemsFunc = propertyPath.Tail.GetAllElements<TDetail, TRemovedItem>();

                return (trakingService, domainObject) =>
                {
                    var changes = trakingService.GetChanges(domainObject);

                    if (changes.HasChange(propertyExpression))
                    {
                        var megreResult = trakingService.GetChanges(domainObject).ManyProperty(propertyExpression).ToMergeResult();

                        return megreResult.RemovingItems.Pipe(getItemsFunc);
                    }
                    else
                    {
                        return new TRemovedItem[0];
                    }
                };
            }


            private static Func<ITrackingService<TPersistentDomainObjectBase>, TDomainObject, IEnumerable<TRemovedItem>> GetSourceFuncSubMerge<TDetail>([NotNull] PropertyPath propertyPath, [NotNull] Expression<Func<TDomainObject, IEnumerable<TDetail>>> propertyExpression)
                where TDetail : class, TPersistentDomainObjectBase
            {
                if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));
                if (propertyExpression == null) throw new ArgumentNullException(nameof(propertyExpression));

                var getSubSourceFunc = InternalHelper<TPersistentDomainObjectBase, TDetail, TRemovedItem>.SourceFunc;

                return (trakingService, domainObject) =>
                {
                    var changes = trakingService.GetChanges(domainObject);

                    if (changes.HasChange(propertyExpression))
                    {
                        var megreResult = trakingService.GetChanges(domainObject).ManyProperty(propertyExpression).ToMergeResult();

                        return megreResult.CombineItems.Select(t => t.Item2).SelectMany(detail => getSubSourceFunc(trakingService, detail));
                    }
                    else
                    {
                        return new TRemovedItem[0];
                    }
                };
            }
        }




        private static IEnumerable<PropertyPath> GetPropertyPaths([NotNull] this Type sourceType, [NotNull] Type targetType)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            return sourceType.GetDetailProperties()
                             .Where(detailProp => !detailProp.IsHierarchical())
                             .SelectMany(prop => prop.GetPropertyPaths(targetType));
        }

        private static IEnumerable<PropertyPath> GetPropertyPaths([NotNull] this PropertyInfo propertyInfo, [NotNull] Type targetType)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            var detailType = propertyInfo.PropertyType.GetCollectionElementType();

            if (targetType.IsAssignableFrom(detailType))
            {
                yield return new PropertyPath(new[] { propertyInfo });
            }
            else
            {
                foreach (var subPath in detailType.GetPropertyPaths(targetType))
                {
                    yield return propertyInfo + subPath;
                }
            }
        }


        internal static Func<IEnumerable<TSource>, IEnumerable<TElement>> GetAllElements<TSource, TElement>([NotNull] this PropertyPath propertyPath)
        {
            if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));

            var baseSource = propertyPath.GetAllElements() ?? FuncHelper.Create((IEnumerable<TSource> s) => s);

            return (Func<IEnumerable<TSource>, IEnumerable<TElement>>)baseSource;
        }

        internal static Delegate GetAllElements([NotNull] this PropertyPath propertyPath)
        {
            if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));

            var cachedProperties = propertyPath.ToArray(true);

            if (cachedProperties.Any())
            {
                var selectManyMethod = new Func<IEnumerable<Ignore>, Func<Ignore, IEnumerable<Ignore>>, IEnumerable<Ignore>>(Enumerable.SelectMany).Method.GetGenericMethodDefinition();

                var parameter = Expression.Parameter(typeof(IEnumerable<>).MakeGenericType(cachedProperties.First().DeclaringType));

                var resultExpr = cachedProperties.Aggregate(
                    (Expression)parameter,
                    (expr, property) =>
                        {
                            var sourceType = property.DeclaringType;

                    var detailType = property.PropertyType.GetCollectionElementType();

                    return Expression.Call(selectManyMethod.MakeGenericMethod(sourceType, detailType), expr, property.ToLambdaExpression());
                });

                return Expression.Lambda(resultExpr, parameter).Compile();
            }
            else
            {
                return null;
            }
        }
    }
}
