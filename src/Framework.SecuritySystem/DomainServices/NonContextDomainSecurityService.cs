using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem
{
    /// <summary>
    /// Сервис с кешированием доступа к неконтекстным операциям
    /// </summary>

    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TIdent"></typeparam>

    /// <typeparam name="TSecurityOperationCode"></typeparam>

    public abstract class NonContextDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode> : DomainSecurityService<TDomainObject>,

        IDomainSecurityService<TDomainObject, TSecurityOperationCode>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TSecurityOperationCode : struct, Enum
    {
        private readonly ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode> securityOperationResolver;

        private readonly IDictionaryCache<SecurityOperation<TSecurityOperationCode>, ISecurityProvider<TDomainObject>> providersCache;


        protected NonContextDomainSecurityServiceBase(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode> securityOperationResolver)
            : base(disabledSecurityProviderSource)
        {
            this.securityOperationResolver = securityOperationResolver ?? throw new ArgumentNullException(nameof(securityOperationResolver));

            this.providersCache = new DictionaryCache<SecurityOperation<TSecurityOperationCode>, ISecurityProvider<TDomainObject>>(this.CreateSecurityProvider).WithLock();
        }

        protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode)
        {
            return this.GetSecurityProvider(this.securityOperationResolver.GetSecurityOperation<TDomainObject>(securityMode));
        }

        protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation<TSecurityOperationCode> securityOperation);


        public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation<TSecurityOperationCode> securityOperation)
        {
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            return this.providersCache[securityOperation];
        }

        public ISecurityProvider<TDomainObject> GetSecurityProvider(TSecurityOperationCode securityOperationCode)
        {
            return this.GetSecurityProvider(this.securityOperationResolver.GetSecurityOperation(securityOperationCode));
        }
    }

    public abstract class NonContextDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode> : NonContextDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode>,

        IDomainSecurityService<TDomainObject, TSecurityOperationCode>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TSecurityOperationCode : struct, Enum
    {
        private readonly IDisabledSecurityProviderSource disabledSecurityProviderSource;

        [NotNull]
        private readonly IAuthorizationSystem<TIdent> authorizationSystem;

        private readonly IDictionaryCache<NonContextSecurityOperation<TSecurityOperationCode>, ISecurityProvider<TDomainObject>> providersCache;



        protected NonContextDomainSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode> securityOperationResolver,
            IAuthorizationSystem<TIdent> authorizationSystem)
            : base(disabledSecurityProviderSource, securityOperationResolver)
        {
            this.disabledSecurityProviderSource = disabledSecurityProviderSource;
            this.authorizationSystem = authorizationSystem ?? throw new ArgumentNullException(nameof(authorizationSystem));

            this.providersCache = new DictionaryCache<NonContextSecurityOperation<TSecurityOperationCode>, ISecurityProvider<TDomainObject>>(this.CreateSecurityProvider).WithLock();
        }



        protected ISecurityProvider<TDomainObject> Create(Expression<Func<TDomainObject, bool>> securityFilter, TSecurityOperationCode securityOperationCode)
        {
            if (securityFilter == null) throw new ArgumentNullException(nameof(securityFilter));

            return new NonContextSecurityProvider<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode>(new NonContextSecurityOperation<TSecurityOperationCode>(securityOperationCode), this.authorizationSystem)
                  .And(securityFilter);
        }

        public ISecurityProvider<TDomainObject> GetSecurityProvider(NonContextSecurityOperation<TSecurityOperationCode> securityOperation)
        {
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            return this.providersCache[securityOperation];
        }

        protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(NonContextSecurityOperation<TSecurityOperationCode> securityOperation)
        {
            return new NonContextSecurityProvider<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode>(securityOperation, this.authorizationSystem);
        }

        protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation<TSecurityOperationCode> securityOperation)
        {
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            if (securityOperation is DisabledSecurityOperation<TSecurityOperationCode>)
            {
                return this.disabledSecurityProviderSource.GetDisabledSecurityProvider<TDomainObject>();
            }
            else
            {
                return this.GetSecurityProvider(new NonContextSecurityOperation<TSecurityOperationCode>(securityOperation.Code));
            }
        }
    }
}
