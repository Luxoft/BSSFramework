using System;

using Framework.Core;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem
{
    public abstract class DependencyDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TBaseDomainObject, TIdent, TSecurityOperationCode> :

        DomainSecurityService<TDomainObject>, IDomainSecurityService<TDomainObject, TSecurityOperationCode>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TSecurityOperationCode : struct, Enum
        where TBaseDomainObject : class, TPersistentDomainObjectBase
    {
        private readonly IDomainSecurityService<TBaseDomainObject, TSecurityOperationCode> baseDomainSecurityService;

        private readonly IDictionaryCache<SecurityOperation<TSecurityOperationCode>, ISecurityProvider<TDomainObject>> providersByOperationCache;

        private readonly IDictionaryCache<TSecurityOperationCode, ISecurityProvider<TDomainObject>> providersByOperationCodeCache;

        protected DependencyDomainSecurityServiceBase(
            IDisabledSecurityProviderContainer<TPersistentDomainObjectBase> disabledSecurityProviderContainer,
            [NotNull] IDomainSecurityService<TBaseDomainObject, TSecurityOperationCode> baseDomainSecurityService)
            : base(disabledSecurityProviderContainer)
        {
            this.baseDomainSecurityService = baseDomainSecurityService ?? throw new ArgumentNullException(nameof(baseDomainSecurityService));

            this.providersByOperationCache = new DictionaryCache<SecurityOperation<TSecurityOperationCode>, ISecurityProvider<TDomainObject>>(
                securityOperation => this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityOperation))).WithLock();

            this.providersByOperationCodeCache = new DictionaryCache<TSecurityOperationCode, ISecurityProvider<TDomainObject>>(
                securityOperationCode => this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityOperationCode))).WithLock();
        }


        protected abstract ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider);

        public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation<TSecurityOperationCode> securityOperation)
        {
            return this.providersByOperationCache[securityOperation];
        }

        public ISecurityProvider<TDomainObject> GetSecurityProvider(TSecurityOperationCode securityOperationCode)
        {
            return this.providersByOperationCodeCache[securityOperationCode];
        }

        protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(BLLSecurityMode securityMode)
        {
            return this.CreateDependencySecurityProvider(this.baseDomainSecurityService.GetSecurityProvider(securityMode));
        }
    }
}
