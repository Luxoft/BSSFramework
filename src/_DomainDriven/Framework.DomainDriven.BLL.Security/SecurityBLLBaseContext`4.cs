using Framework.DomainDriven.Tracking;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer> :
        DefaultBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer>,
        IAccessDeniedExceptionServiceContainer

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
        where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
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
            : base(serviceProvider, operationSenders, trackingService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService) =>

            this.AccessDeniedExceptionService = accessDeniedExceptionService ?? throw new ArgumentNullException(nameof(accessDeniedExceptionService));

    public virtual IAccessDeniedExceptionService AccessDeniedExceptionService { get; }

    /// <inheritdoc />
    public override bool AllowedExpandTreeParents<TDomainObject>()
    {
        var viewOperation = this.ServiceProvider.GetRequiredService<ISecurityOperationResolver<TPersistentDomainObjectBase>>()
                                .GetSecurityOperation<TDomainObject>(BLLSecurityMode.View);

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

    IAccessDeniedExceptionService IAccessDeniedExceptionServiceContainer.AccessDeniedExceptionService =>
            this.AccessDeniedExceptionService;
}
