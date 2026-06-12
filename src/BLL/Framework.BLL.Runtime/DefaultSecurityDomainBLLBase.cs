using Anch.Core;
using Anch.SecuritySystem.Providers;

using Framework.Application.Domain;
using Framework.BLL.Default;

namespace Framework.BLL;

/// <summary>
/// BLL с безопастностью
/// </summary>
/// <typeparam name="TBLLContext"></typeparam>
/// <typeparam name="TDomainObject"></typeparam>
public class DefaultSecurityDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>(
    TBLLContext context,
    ISecurityProvider<TDomainObject> securityProvider)
    : DefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>(context),

      IDefaultSecurityDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TBLLContext : class, ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>, IAccessDeniedExceptionServiceContainer,
    IHierarchicalObjectExpanderFactoryContainer
    where TIdent : notnull
{
    public ISecurityProvider<TDomainObject> SecurityProvider { get; } = securityProvider;


    protected override IQueryable<TDomainObject> ProcessSecurity(IQueryable<TDomainObject> queryable) =>
        base.ProcessSecurity(queryable).Pipe(q => this.SecurityProvider.Inject(q));

    public virtual void CheckAccess(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.DefaultCancellationTokenSource.RunSync(async ct => await this.SecurityProvider.CheckAccessAsync(domainObject, this.Context.AccessDeniedExceptionService, ct));
    }

    public override void Save(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.CheckAccess(domainObject);

        base.Save(domainObject);
    }

    public override void Insert(TDomainObject domainObject, TIdent id)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.CheckAccess(domainObject);

        base.Insert(domainObject, id);
    }

    public override void Remove(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.CheckAccess(domainObject);

        base.Remove(domainObject);
    }

    protected sealed override Exception GetMissingObjectException(TIdent id)
    {
        var request = from objectWithoutPermission in this.Context.Logics.Default.Create<TDomainObject>().GetById(id).ToMaybe()

                      let accessDeniedResult =
                          this.DefaultCancellationTokenSource.RunSync(async ct => await this.SecurityProvider.GetAccessResultAsync(objectWithoutPermission, ct))
                              as AccessResult.AccessDeniedResult

                      where accessDeniedResult != null

                      select this.Context.AccessDeniedExceptionService.GetAccessDeniedException(accessDeniedResult);

        return request.GetValueOrDefault(() => base.GetMissingObjectException(id));
    }
}
