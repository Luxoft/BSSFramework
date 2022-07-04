using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;
using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem
{
    /// <summary>
    /// Сервис с кешированием доступа к контекстным операциям
    /// </summary>
    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TIdent"></typeparam>
    /// <typeparam name="TSecurityOperationCode"></typeparam>
    public abstract class ContextDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode> : NonContextDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TSecurityOperationCode : struct, Enum
    {
        private readonly IDisabledSecurityProviderContainer<TPersistentDomainObjectBase> disabledSecurityProviderContainer;

        private readonly ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory;

        private readonly IDictionaryCache<ContextSecurityOperation<TSecurityOperationCode>, ISecurityProvider<TDomainObject>> providersCache;

        protected ContextDomainSecurityServiceBase(
            [NotNull] IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull] IDisabledSecurityProviderContainer<TPersistentDomainObjectBase> disabledSecurityProviderContainer,
            [NotNull] ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode> securityOperationResolver,
            [NotNull] IAuthorizationSystem<TIdent> authorizationSystem,
            [NotNull] ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory)

            : base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem)
        {
            this.disabledSecurityProviderContainer = disabledSecurityProviderContainer ?? throw new ArgumentNullException(nameof(disabledSecurityProviderContainer));
            this.securityExpressionBuilderFactory = securityExpressionBuilderFactory ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));
            this.providersCache = new DictionaryCache<ContextSecurityOperation<TSecurityOperationCode>, ISecurityProvider<TDomainObject>>(this.CreateSecurityProvider).WithLock();
        }


        protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation<TSecurityOperationCode> operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            switch (operation)
            {
                case ContextSecurityOperation<TSecurityOperationCode> securityOperation:
                    return this.GetSecurityProvider(securityOperation);

                case NonContextSecurityOperation<TSecurityOperationCode> securityOperation:
                    return this.GetSecurityProvider(securityOperation);

                case DisabledSecurityOperation<TSecurityOperationCode> securityOperation:
                    return this.disabledSecurityProviderContainer.GetDisabledSecurityProvider<TDomainObject>();

                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }

        protected ISecurityProvider<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, TSecurityContext>> securityPath, ContextSecurityOperation<TSecurityOperationCode> securityOperation)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
        {
            if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            return this.Create(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.Create(securityPath), securityOperation);
        }

        protected ISecurityProvider<TDomainObject> Create<TSecurityContext>(Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> securityPath, ContextSecurityOperation<TSecurityOperationCode> securityOperation)
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
        {
            if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            return this.Create(SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.Create(securityPath), securityOperation);
        }

        protected virtual ISecurityProvider<TDomainObject> Create(SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent> securityPath, ContextSecurityOperation<TSecurityOperationCode> securityOperation)
        {
            if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            return securityPath.ToProvider(securityOperation, this.securityExpressionBuilderFactory, this.AccessDeniedExceptionService);
        }


        protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(ContextSecurityOperation<TSecurityOperationCode> securityOperation);


        public ISecurityProvider<TDomainObject> GetSecurityProvider(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
        {
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));


            return this.providersCache[securityOperation];
        }
    }

    public abstract class ContextDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode> : ContextDomainSecurityServiceBase<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TSecurityOperationCode : struct, Enum
    {
        protected ContextDomainSecurityService(

            [NotNull] IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull] IDisabledSecurityProviderContainer<TPersistentDomainObjectBase> disabledSecurityProviderContainer,
            [NotNull] ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode> securityOperationResolver,
            [NotNull] IAuthorizationSystem<TIdent> authorizationSystem,
            [NotNull] ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory)
            : base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
        {
        }

        protected abstract SecurityPathBase<TPersistentDomainObjectBase, TDomainObject, TIdent> GetSecurityPath();



        protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
        {

            return this.Create(this.GetSecurityPath(), securityOperation);
        }
    }
}
