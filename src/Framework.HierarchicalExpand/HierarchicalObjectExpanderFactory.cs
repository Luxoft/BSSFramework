using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;

using JetBrains.Annotations;

namespace Framework.HierarchicalExpand;

public class HierarchicalObjectExpanderFactory<TPersistentDomainObjectBase, TIdent> : IHierarchicalObjectExpanderFactory<TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TIdent : struct
{
    private readonly IQueryableSource<TPersistentDomainObjectBase> queryableSource;

    private readonly IHierarchicalRealTypeResolver realTypeResolver;

    private static readonly MethodInfo GenericCreateMethod = typeof(HierarchicalObjectExpanderFactory<TPersistentDomainObjectBase, TIdent>).GetMethod(nameof(Create), BindingFlags.Public | BindingFlags.Instance, true);

    private static readonly MethodInfo GenericQueryCreateMethod = typeof(HierarchicalObjectExpanderFactory<TPersistentDomainObjectBase, TIdent>).GetMethod(nameof(CreateQuery), BindingFlags.Public | BindingFlags.Instance, true);

    private static readonly MethodInfo GenericCreateHierarchicalMethod = typeof(HierarchicalObjectExpanderFactory<TPersistentDomainObjectBase, TIdent>).GetMethod(nameof(CreateHierarchical), BindingFlags.NonPublic | BindingFlags.Instance, true);

    private static readonly MethodInfo GenericCreateHierarchicalWithAncestorLinkMethod = typeof(HierarchicalObjectExpanderFactory<TPersistentDomainObjectBase, TIdent>).GetMethod(nameof(CreateHierarchicalWithAncestorLink), BindingFlags.NonPublic | BindingFlags.Instance, true);


    public HierarchicalObjectExpanderFactory([NotNull] IQueryableSource<TPersistentDomainObjectBase> queryableSource,
                                             [NotNull] IHierarchicalRealTypeResolver realTypeResolver)
    {
        this.queryableSource = queryableSource ?? throw new ArgumentNullException(nameof(queryableSource));
        this.realTypeResolver = realTypeResolver ?? throw new ArgumentNullException(nameof(realTypeResolver));
    }


    public virtual IHierarchicalObjectExpander<TIdent> Create<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
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
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return (IHierarchicalObjectQueryableExpander<TIdent>)this.Create<TDomainObject>();
    }

    protected virtual IHierarchicalObjectExpander<TIdent> CreatePlain<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return new PlainHierarchicalObjectExpander<TIdent>();
    }

    protected virtual IHierarchicalObjectExpander<TIdent> CreateHierarchical<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
    {
        return new HierarchicalObjectLayerExpander<TPersistentDomainObjectBase, TDomainObject, TIdent>(this.queryableSource);
    }

    protected virtual IHierarchicalObjectExpander<TIdent> CreateHierarchicalWithAncestorLink<TDomainObject, TDomainObjectAncestorLink, TDomainObjectAncestorChildLink>()
            where TDomainObject : class, TPersistentDomainObjectBase, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
            where TDomainObjectAncestorLink : class, TPersistentDomainObjectBase, IHierarchicalAncestorLink<TDomainObject, TDomainObjectAncestorChildLink, TIdent>
            where TDomainObjectAncestorChildLink : class, TPersistentDomainObjectBase, IHierarchicalToAncestorOrChildLink<TDomainObject, TIdent>
    {
        return new HierarchicalObjectAncestorLinkExpander<TPersistentDomainObjectBase, TDomainObject, TDomainObjectAncestorLink, TDomainObjectAncestorChildLink, TIdent>(this.queryableSource);
    }

    IHierarchicalObjectExpander<TIdent> IHierarchicalObjectExpanderFactory<TIdent>.Create(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (!typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainType))
        {
            throw new InvalidOperationException($"Domain Type {domainType.Name} must be derived from {typeof(TPersistentDomainObjectBase).Name}");
        }

        return (IHierarchicalObjectExpander<TIdent>)GenericCreateMethod.MakeGenericMethod(domainType).Invoke(this, Array.Empty<object>());
    }

    IHierarchicalObjectQueryableExpander<TIdent> IHierarchicalObjectExpanderFactory<TIdent>.CreateQuery(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (!typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainType))
        {
            throw new InvalidOperationException($"Domain Type {domainType.Name} must be derived from {typeof(TPersistentDomainObjectBase).Name}");
        }

        return (IHierarchicalObjectQueryableExpander<TIdent>)GenericQueryCreateMethod.MakeGenericMethod(domainType).Invoke(this, Array.Empty<object>());
    }
}
