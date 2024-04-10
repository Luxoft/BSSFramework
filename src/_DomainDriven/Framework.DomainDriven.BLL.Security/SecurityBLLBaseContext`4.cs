using Framework.DomainDriven.Tracking;
using Framework.Events;
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
            IEventOperationSender operationSender,
            ITrackingService<TPersistentDomainObjectBase> trackingService,
            IAccessDeniedExceptionService accessDeniedExceptionService,
            IStandartExpressionBuilder standartExpressionBuilder,
            IValidator validator,
            IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
            IFetchService<TPersistentDomainObjectBase, FetchBuildRule> fetchService)
            : base(serviceProvider, operationSender, trackingService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService) =>

            this.AccessDeniedExceptionService = accessDeniedExceptionService ?? throw new ArgumentNullException(nameof(accessDeniedExceptionService));

    public virtual IAccessDeniedExceptionService AccessDeniedExceptionService { get; }

    public ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
        where TDomainObject : TPersistentDomainObjectBase
    {
        return this.ServiceProvider.GetRequiredService<ISecurityProvider<TDomainObject>>();
    }

    public virtual ISecurityRuleExpander SecurityRuleExpanders => this.ServiceProvider.GetRequiredService<ISecurityRuleExpander>();

    /// <inheritdoc />
    public override bool AllowedExpandTreeParents<TDomainObject>()
    {
        var viewSecurityRule = this.SecurityRuleExpanders.TryExpand<TDomainObject>(SecurityRule.View);

        if (viewSecurityRule != null)
        {
            return viewSecurityRule.ExpandType.HasFlag(HierarchicalExpandType.Parents);
        }
        else
        {
            return true;
        }
    }
}
