using System;

using Framework.DomainDriven.BLL.Tracking;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL.Security
{
    public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer, TSecurityOperationCode> :

        SecurityBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer>, ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
        where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
        where TSecurityOperationCode : struct, Enum
    {
        protected SecurityBLLBaseContext(
            [NotNull] IServiceProvider serviceProvider,
            [NotNull] IDALFactory<TPersistentDomainObjectBase, TIdent> dalFactory,
            [NotNull] IOperationEventListenerContainer<TDomainObjectBase> operationListeners,
            [NotNull] BLLSourceEventListenerContainer<TPersistentDomainObjectBase> sourceListeners,
            [NotNull] IObjectStateService objectStateService,
            [NotNull] IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull] IStandartExpressionBuilder standartExpressionBuilder,
            [NotNull] IValidator validator,
            [NotNull] IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
            [NotNull] IFetchService<TPersistentDomainObjectBase, FetchBuildRule> fetchService)
            : base(serviceProvider, dalFactory, operationListeners, sourceListeners, objectStateService, accessDeniedExceptionService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService)
        {
        }

        /// <inheritdoc />
        public override bool AllowedExpandTreeParents<TDomainObject>()
        {
            var viewOperation = this.GetSecurityOperation<TDomainObject>(BLLSecurityMode.View);

            if (viewOperation is ContextSecurityOperation<TSecurityOperationCode>)
            {
                var contextOperation = viewOperation as ContextSecurityOperation<TSecurityOperationCode>;

                return contextOperation.SecurityExpandType.HasFlag(HierarchicalExpandType.Parents);
            }
            else
            {
                return true;
            }
        }

        public abstract SecurityOperation<TSecurityOperationCode> GetSecurityOperation(TSecurityOperationCode securityOperationCode);

        public abstract SecurityOperation<TSecurityOperationCode> GetSecurityOperation<TDomainObject>(BLLSecurityMode securityMode)
            where TDomainObject : TPersistentDomainObjectBase;
    }
}
