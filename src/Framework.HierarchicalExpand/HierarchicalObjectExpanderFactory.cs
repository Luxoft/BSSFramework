using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.HierarchicalExpand;

public class HierarchicalObjectExpanderFactory<TIdent> : IHierarchicalObjectExpanderFactory<TIdent>
        where TIdent : struct
{
    private readonly IQueryableSource queryableSource;

    private readonly IRealTypeResolver realTypeResolver;

    private static readonly MethodInfo GenericCreateMethod = typeof(HierarchicalObjectExpanderFactory<TIdent>).GetMethod(nameof(Create), BindingFlags.Public | BindingFlags.Instance, true);

    private static readonly MethodInfo GenericQueryCreateMethod = typeof(HierarchicalObjectExpanderFactory<TIdent>).GetMethod(nameof(CreateQuery), BindingFlags.Public | BindingFlags.Instance, true);

    private static readonly MethodInfo GenericCreateHierarchicalMethod = typeof(HierarchicalObjectExpanderFactory<TIdent>).GetMethod(nameof(CreateHierarchical), BindingFlags.NonPublic | BindingFlags.Instance, true);

    private static readonly MethodInfo GenericCreateHierarchicalWithAncestorLinkMethod = typeof(HierarchicalObjectExpanderFactory<TIdent>).GetMethod(nameof(CreateHierarchicalWithAncestorLink), BindingFlags.NonPublic | BindingFlags.Instance, true);


    public HierarchicalObjectExpanderFactory(IQueryableSource queryableSource, IRealTypeResolver realTypeResolver)
    {
        this.queryableSource = queryableSource ?? throw new ArgumentNullException(nameof(queryableSource));
        this.realTypeResolver = realTypeResolver ?? throw new ArgumentNullException(nameof(realTypeResolver));
    }


    public virtual IHierarchicalObjectExpander<TIdent> Create<TDomainObject>()
            where TDomainObject : class, IIdentityObject<TIdent>
    {
        var realType = this.realTypeResolver.Resolve(typeof(TDomainObject));

        if (realType != typeof(TDomainObject))
        {
            return (IHierarchicalObjectExpander<TIdent>)GenericCreateMethod.MakeGenericMethod(realType).Invoke(this, Array.Empty<object>());
        }
        else
        {
            var ancestorInfo = typeof(TDomainObject).GetHierarchicalAncestorLinkType();

            if (ancestorInfo != null)
            {
                return (IHierarchicalObjectExpander<TIdent>)GenericCreateHierarchicalWithAncestorLinkMethod.MakeGenericMethod(typeof(TDomainObject), ancestorInfo.Value.AncestorToChildType, ancestorInfo.Value.SourceToAncestorChildType).Invoke(this, Array.Empty<object>());
            }
            else if (typeof(TDomainObject).IsHierarchical())
            {
                return (IHierarchicalObjectExpander<TIdent>)GenericCreateHierarchicalMethod.MakeGenericMethod(typeof(TDomainObject)).Invoke(this, Array.Empty<object>());
            }
            else
            {
                return this.CreatePlain<TDomainObject>();
            }
        }
    }


    public virtual IHierarchicalObjectQueryableExpander<TIdent> CreateQuery<TDomainObject>()
            where TDomainObject : class, IIdentityObject<TIdent>
    {
        return (IHierarchicalObjectQueryableExpander<TIdent>)this.Create<TDomainObject>();
    }

    protected virtual IHierarchicalObjectExpander<TIdent> CreatePlain<TDomainObject>()
            where TDomainObject : class, IIdentityObject<TIdent>
    {
        return new PlainHierarchicalObjectExpander<TIdent>();
    }

    protected virtual IHierarchicalObjectExpander<TIdent> CreateHierarchical<TDomainObject>()
            where TDomainObject : class, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
    {
        return new HierarchicalObjectLayerExpander<TDomainObject, TIdent>(this.queryableSource);
    }

    protected virtual IHierarchicalObjectExpander<TIdent> CreateHierarchicalWithAncestorLink<TDomainObject, TDomainObjectAncestorLink, TDomainObjectAncestorChildLink>()
            where TDomainObject : class, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
            where TDomainObjectAncestorLink : class, IHierarchicalAncestorLink<TDomainObject, TDomainObjectAncestorChildLink, TIdent>
            where TDomainObjectAncestorChildLink : class, IHierarchicalToAncestorOrChildLink<TDomainObject, TIdent>
    {
        return new HierarchicalObjectAncestorLinkExpander<TDomainObject, TDomainObjectAncestorLink, TDomainObjectAncestorChildLink, TIdent>(this.queryableSource);
    }

    IHierarchicalObjectExpander<TIdent> IHierarchicalObjectExpanderFactory<TIdent>.Create(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return (IHierarchicalObjectExpander<TIdent>)GenericCreateMethod.MakeGenericMethod(domainType).Invoke(this, Array.Empty<object>());
    }

    IHierarchicalObjectQueryableExpander<TIdent> IHierarchicalObjectExpanderFactory<TIdent>.CreateQuery(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return (IHierarchicalObjectQueryableExpander<TIdent>)GenericQueryCreateMethod.MakeGenericMethod(domainType).Invoke(this, Array.Empty<object>());
    }
}
