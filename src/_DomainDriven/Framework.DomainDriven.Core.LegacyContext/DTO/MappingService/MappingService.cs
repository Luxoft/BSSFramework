using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace Framework.DomainDriven;

public abstract class DTOMappingService<TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TIdent, TVersion>

        : BLLContextContainer<TBLLContext>, IDTOMappingService<TPersistentDomainObjectBase, TIdent>

        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TAuditPersistentDomainObjectBase : class, TPersistentDomainObjectBase
        where TVersion : IEquatable<TVersion>
{
    protected DTOMappingService(TBLLContext context)
            : base(context)
    {
        this.VersionService = new DTOMappingVersionService<TBLLContext, TAuditPersistentDomainObjectBase, TIdent, TVersion>(this.Context);
    }


    public virtual IDTOMappingVersionService<TAuditPersistentDomainObjectBase, TVersion> VersionService { get; private set; }

    public virtual IBinaryConverter BinaryConverter { get; } = new BinaryConverter();


    public virtual TDomainObject GetById<TDomainObject>(TIdent ident, IdCheckMode checkMode = IdCheckMode.SkipEmpty, LockRole lockRole = LockRole.None)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return this.Context.Logics.Default.Create<TDomainObject>().GetById(ident, checkMode, null, lockRole);
    }

    protected virtual TDomainObject GetByIdOrCreate<TDomainObject>(TIdent ident, Func<TDomainObject> createFunc)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        return this.GetById<TDomainObject>(ident) ?? createFunc();
    }

    protected virtual ICollectionMappingService<TSource, TTarget> GetCollectionMappingService<TSource, TTarget>(
            Func<TSource, TTarget> createAndMapDetail, Action<TTarget> removeDetail)

            where TSource : IIdentityObject<TIdent>
            where TTarget : class, IIdentityObject<TIdent>
    {
        return new DefaultCollectionMappingService<TSource, TTarget, TIdent>(createAndMapDetail, removeDetail);
    }

    protected virtual IUpdateCollectionMappingService<TSourceItem, TSourceIdentity, TTarget> GetUpdateCollectionMappingService<TSourceItem, TSourceIdentity, TTarget>(
            Func<TSourceItem, TTarget> createAndMapDetail,
            Action<TTarget> removeDetail)
            where TSourceItem : class, IIdentityObjectContainer<TSourceIdentity>
            where TSourceIdentity : IIdentityObject<TIdent>
            where TTarget : class, IIdentityObject<TIdent>
    {
        return new DefaultUpdateCollectionMappingService<TSourceItem, TSourceIdentity, TTarget, TIdent>(createAndMapDetail, removeDetail);
    }
}
