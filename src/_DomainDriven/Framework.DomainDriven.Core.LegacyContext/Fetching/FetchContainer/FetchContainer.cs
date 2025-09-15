using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

using CommonFramework;

namespace Framework.DomainDriven;

public class FetchContainer<TDomainObject> : IEquatable<FetchContainer<TDomainObject>>, IFetchContainer<TDomainObject>
{
    public FetchContainer(IEnumerable<IEnumerable<PropertyInfo>> loadPaths)
    {
        if (loadPaths == null) throw new ArgumentNullException(nameof(loadPaths));

        this.LoadPaths = loadPaths.ToReadOnlyCollection(path => new PropertyPath(path.ToList()));

        this.Fetchs = this.LoadPaths.Select(loadPath => loadPath.ToNode<TDomainObject>()).ToTree();
    }


    public IPropertyPathTree<TDomainObject> Fetchs { get; }

    public ReadOnlyCollection<PropertyPath> LoadPaths { get; }


    public bool Equals(FetchContainer<TDomainObject>? other)
    {
        return other != null && this.LoadPaths.SequenceEqual(other.LoadPaths);
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as FetchContainer<TDomainObject>);
    }

    public override int GetHashCode()
    {
        return this.LoadPaths.Count;
    }

    public override string ToString()
    {
        return this.LoadPaths.Join(Environment.NewLine);
    }



    public static FetchContainer<TDomainObject> operator+(FetchContainer<TDomainObject> fetch1, FetchContainer<TDomainObject> fetch2)
    {
        if (fetch1 == null) throw new ArgumentNullException(nameof(fetch1));
        if (fetch2 == null) throw new ArgumentNullException(nameof(fetch2));

        return new FetchContainer<TDomainObject>(fetch1.LoadPaths.Concat(fetch2.LoadPaths));
    }




    public static readonly FetchContainer<TDomainObject> Empty = new FetchContainer<TDomainObject>(new IEnumerable<PropertyInfo>[0]);
}


public static class FetchContainer
{
    public static IFetchContainer<TDomainObject> Create<TDomainObject>(IPropertyPathTree<TDomainObject> fetchs)
    {
        if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

        return new FetchContainerImpl<TDomainObject>(fetchs);
    }

    public static IFetchContainer<TDomainObject> Create<TDomainObject>(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
    {
        if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

        return Create(new PropertyPathTree<TDomainObject>(fetchs));
    }

    public static IFetchContainer<TDomainObject> Create<TDomainObject>(params Expression<Action<IPropertyPathNode<TDomainObject>>>[] fetchs)
    {
        if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

        return Create((IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>>)fetchs);
    }


    private class FetchContainerImpl<TDomainObject> : IFetchContainer<TDomainObject>
    {
        public FetchContainerImpl(IPropertyPathTree<TDomainObject> fetchs)
        {
            if (fetchs == null) throw new ArgumentNullException(nameof(fetchs));

            this.Fetchs = fetchs;
        }


        public IPropertyPathTree<TDomainObject> Fetchs{ get; }
    }
}
