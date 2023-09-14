using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;
using nuSpec.Abstraction;

namespace Framework.DomainDriven.BLL.Security
{
    public interface IDefaultSecurityDomainBLLBase<in TPersistentDomainObjectBase, TDomainObject, TIdent> : IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        ISecurityProvider<TDomainObject> SecurityProvider { get; }

        void CheckAccess(TDomainObject domainObject);
    }

    public interface IDefaultSecurityDomainBLLBase<out TBLLContext, in TPersistentDomainObjectBase, TDomainObject, TIdent> :
        IDefaultSecurityDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent>,
        IDefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
    {

    }

    /// <summary>
    /// BLL с безопастностью
    /// </summary>
    /// <typeparam name="TBLLContext"></typeparam>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TOperation"></typeparam>
    public class DefaultSecurityDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation>
        : DefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation>,

        IDefaultSecurityDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TDomainObjectBase : class
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObject : class, TPersistentDomainObjectBase
        where TOperation : struct, Enum
        where TBLLContext : class, ISecurityBLLContext<IAuthorizationBLLContext<TIdent>, TPersistentDomainObjectBase, TDomainObjectBase, TIdent>, IAccessDeniedExceptionServiceContainer, IHierarchicalObjectExpanderFactoryContainer<TIdent>

    {
        protected DefaultSecurityDomainBLLBase(TBLLContext context, ISpecificationEvaluator specificationEvaluator = null)
            : this(context, new DisabledSecurityProvider<TDomainObject>(), specificationEvaluator)
        {
        }

        protected DefaultSecurityDomainBLLBase(TBLLContext context, ISecurityProvider<TDomainObject> securityProvider, ISpecificationEvaluator specificationEvaluator = null)
            : base(context, specificationEvaluator)
        {
            this.SecurityProvider = securityProvider;
        }


        public ISecurityProvider<TDomainObject> SecurityProvider { get; private set; }


        protected override IQueryable<TDomainObject> ProcessSecurity(IQueryable<TDomainObject> queryable)
        {
            return base.ProcessSecurity(queryable).Pipe(q => this.SecurityProvider.InjectFilter(q));
        }


        public virtual void CheckAccess(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.SecurityProvider.CheckAccess(domainObject, this.Context.AccessDeniedExceptionService);
        }

        protected virtual void CheckInsertAccess(TDomainObject domainObject, TIdent id)
        {
            this.CheckAccess(domainObject);
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

            this.CheckInsertAccess(domainObject, id);

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

                          let accessDeniedResult = this.SecurityProvider.GetAccessResult(objectWithoutPermission) as AccessResult.AccessDeniedResult

                          where accessDeniedResult != null

                          select this.Context.AccessDeniedExceptionService.GetAccessDeniedException(accessDeniedResult);

            return request.GetValueOrDefault(() => base.GetMissingObjectException(id));
        }
    }
}
