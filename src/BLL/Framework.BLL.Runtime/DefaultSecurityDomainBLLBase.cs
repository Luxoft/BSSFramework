using CommonFramework;

using Framework.Application.Domain;
using Framework.BLL.Default;
using SecuritySystem.Providers;

namespace Framework.BLL
{
    /// <summary>
    /// BLL с безопастностью
    /// </summary>
    /// <typeparam name="TBLLContext"></typeparam>
    /// <typeparam name="TDomainObject"></typeparam>
    public class DefaultSecurityDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>
        : DefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>,

          IDefaultSecurityDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TBLLContext : class, ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>, IAccessDeniedExceptionServiceContainer, IHierarchicalObjectExpanderFactoryContainer
    {
        protected DefaultSecurityDomainBLLBase(
            TBLLContext context,
            ISecurityProvider<TDomainObject> securityProvider)
            : base(context) =>
            this.SecurityProvider = securityProvider;

        public ISecurityProvider<TDomainObject> SecurityProvider { get; }


        protected override IQueryable<TDomainObject> ProcessSecurity(IQueryable<TDomainObject> queryable) => base.ProcessSecurity(queryable).Pipe(q => this.SecurityProvider.Inject(q));

        public virtual void CheckAccess(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.SecurityProvider.CheckAccessAsync(domainObject, this.Context.AccessDeniedExceptionService).GetAwaiter().GetResult();
        }

        protected virtual void CheckInsertAccess(TDomainObject domainObject, TIdent id) => this.CheckAccess(domainObject);

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
                              this.SecurityProvider.GetAccessResultAsync(objectWithoutPermission).GetAwaiter().GetResult() as AccessResult.AccessDeniedResult

                          where accessDeniedResult != null

                          select this.Context.AccessDeniedExceptionService.GetAccessDeniedException(accessDeniedResult);

            return request.GetValueOrDefault(() => base.GetMissingObjectException(id));
        }
    }
}
