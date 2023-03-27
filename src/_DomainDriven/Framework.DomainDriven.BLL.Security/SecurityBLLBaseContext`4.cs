using Framework.DomainDriven.BLL.Tracking;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL.Security;

public abstract class SecurityBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer> :
        DefaultBLLBaseContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent, TBLLFactoryContainer>,
        IAccessDeniedExceptionServiceContainer<TPersistentDomainObjectBase>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
        where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentDomainObjectBase, TIdent>>
{
    protected SecurityBLLBaseContext(
            [NotNull] IServiceProvider serviceProvider,
            [NotNull] IOperationEventSenderContainer<TPersistentDomainObjectBase> operationSenders,
            [NotNull] IObjectStateService objectStateService,
            [NotNull] IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull] IStandartExpressionBuilder standartExpressionBuilder,
            [NotNull] IValidator validator,
            [NotNull] IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory,
            [NotNull] IFetchService<TPersistentDomainObjectBase, FetchBuildRule> fetchService)
            : base(serviceProvider, operationSenders, objectStateService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService) =>

            this.AccessDeniedExceptionService = accessDeniedExceptionService ?? throw new ArgumentNullException(nameof(accessDeniedExceptionService));

    public virtual IAccessDeniedExceptionService<TPersistentDomainObjectBase> AccessDeniedExceptionService { get; }

    /// <inheritdoc />
    public override bool AllowedExpandTreeParents<TDomainObject>() => false;

    IAccessDeniedExceptionService IAccessDeniedExceptionServiceContainer.AccessDeniedExceptionService =>
            this.AccessDeniedExceptionService;
}
