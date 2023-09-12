using Framework.DomainDriven.Tracking;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.Validation;

namespace Framework.DomainDriven.BLL.Security;

public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer> :

        SecurityBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer>, ISecurityOperationResolver<TPersistentDomainObjectBase>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
        where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
        where TSecurityOperationCode : struct, Enum
{
    protected SecurityBLLBaseContext(
            IServiceProvider serviceProvider,
            IOperationEventSenderContainer<TPersistentDomainObjectBase> operationSenders,
            ITrackingService<TPersistentDomainObjectBase> trackingService,
            IAccessDeniedExceptionService accessDeniedExceptionService,
            IStandartExpressionBuilder standartExpressionBuilder,
            IValidator validator,
            IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
            IFetchService<TPersistentDomainObjectBase, FetchBuildRule> fetchService)
            : base(serviceProvider, operationSenders, trackingService, accessDeniedExceptionService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService)
    {
    }

    /// <inheritdoc />
    public override bool AllowedExpandTreeParents<TDomainObject>()
    {
        var viewOperation = this.GetSecurityOperation<TDomainObject>(BLLSecurityMode.View);

        if (viewOperation is ContextSecurityOperation)
        {
            var contextOperation = viewOperation as ContextSecurityOperation;

            return contextOperation.ExpandType.HasFlag(HierarchicalExpandType.Parents);
        }
        else
        {
            return true;
        }
    }

    public abstract SecurityOperation GetSecurityOperation(SecurityOperation securityOperation);

    public abstract SecurityOperation GetSecurityOperation<TDomainObject>(BLLSecurityMode securityMode)
            where TDomainObject : TPersistentDomainObjectBase;
}
