using Framework.DomainDriven.Tracking;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer> :
        DefaultBLLBaseContext<TPersistentDomainObjectBase, TIdent, TBLLFactoryContainer>, IAccessDeniedExceptionServiceContainer, ISecurityBLLContext<TPersistentDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
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

    public virtual IDisabledSecurityProviderSource DisabledSecurityProviderSource => this.ServiceProvider.GetRequiredService<IDisabledSecurityProviderSource>();

    public virtual ISecurityOperationResolver SecurityOperationResolver => this.ServiceProvider.GetRequiredService<ISecurityOperationResolver>();

    /// <inheritdoc />
    public override bool AllowedExpandTreeParents<TDomainObject>()
    {
        var viewOperation = this.SecurityOperationResolver.TryGetSecurityOperation<TDomainObject>(BLLSecurityMode.View);

        if (viewOperation is ContextSecurityOperation contextOperation)
        {
            return contextOperation.ExpandType.HasFlag(HierarchicalExpandType.Parents);
        }
        else
        {
            return true;
        }
    }
}
