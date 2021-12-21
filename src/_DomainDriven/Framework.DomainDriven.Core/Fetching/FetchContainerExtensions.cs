using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven
{
    public static class FetchContainerExtensions
    {
        public static Expression<Action<IPropertyPathNode<TDomainObject>>>[] TryGetFetchs<TDomainObject>(this IFetchContainer<TDomainObject> fetchContainer)
        {
            return fetchContainer.Maybe(c => c.Fetchs).EmptyIfNull().ToArray();
        }

        public static IFetchContainer<TDomainObject> ToFetchContainer<TDomainObject>(this IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
        {
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return fetchs.ToTree().ToFetchContainer();
        }

        public static IFetchContainer<TDomainObject> ToFetchContainer<TDomainObject>(this Expression<Action<IPropertyPathNode<TDomainObject>>>[] fetchs)
        {
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return fetchs.AnyA() ? fetchs.ToTree().ToFetchContainer() : FetchContainer<TDomainObject>.Empty;
        }

        public static IFetchContainer<TDomainObject> ToFetchContainer<TDomainObject>(this IPropertyPathTree<TDomainObject> fetchs)
        {
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return new FetchRulesFetchContainer<TDomainObject>(fetchs);
        }

        public static IFetchContainer<TDomainObject> Add<TDomainObject>(this IFetchContainer<TDomainObject> fetchContainer, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] fetchs)
        {
            if (fetchContainer == null) throw new ArgumentNullException(nameof(fetchContainer));
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            return fetchContainer.Fetchs.Concat(fetchs).ToFetchContainer();
        }

        public static IEnumerable<PropertyPath> GetPropertyPaths<TDomainObject>(this IFetchContainer<TDomainObject> container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            return container.Fetchs.SelectMany(f => f.ToPropertyPaths());
        }

        public static IFetchContainer<TDomainObject> Compress<TDomainObject>([NotNull] this IFetchContainer<TDomainObject> container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            if (container is CompressFetchContainer<TDomainObject>)
            {
                return container;
            }
            else
            {
                return new CompressFetchContainer<TDomainObject>(container.GetPropertyPaths().Compress());
            }
        }

        public static IEnumerable<PropertyPath> Compress(this IEnumerable<PropertyPath> paths)
        {
            var resultPaths = new List<PropertyPath>();

            paths.Foreach(path =>
            {
                var overridePath = resultPaths.FirstOrDefault(path.StartsWith);

                if (overridePath == null)
                {
                    if (resultPaths.All(rPath => !rPath.StartsWith(path)))
                    {
                        resultPaths.Add(path);
                    }
                }
                else
                {
                    resultPaths.Remove(overridePath);
                    resultPaths.Add(path);
                }
            });

            return resultPaths;
        }


        private class CompressFetchContainer<TDomainObject> : FetchContainer<TDomainObject>
        {
            public CompressFetchContainer(IEnumerable<IEnumerable<PropertyInfo>> loadPaths)
                : base(loadPaths)
            {

            }
        }

        private class FetchRulesFetchContainer<TDomainObject> : IFetchContainer<TDomainObject>
        {
            public FetchRulesFetchContainer(IPropertyPathTree<TDomainObject> fetchs)
            {
                if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

                this.Fetchs = fetchs;
            }


            public IPropertyPathTree<TDomainObject> Fetchs
            {
                get; private set;
            }
        }
    }
}
