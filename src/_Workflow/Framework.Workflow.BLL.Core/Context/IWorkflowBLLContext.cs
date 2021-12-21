using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.BLL.Tracking;

using Framework.Authorization.BLL;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Validation;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public partial interface IWorkflowBLLContext :

        ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, DomainObjectBase, Guid>,

        ITrackingServiceContainer<PersistentDomainObjectBase>,

        IImpersonateObject<IWorkflowBLLContext>,

        IIAnonymousTypeBuilderContainer<TypeMap<ParameterizedTypeMapMember>>,

        ITypeResolverContainer<string>,

        IConfigurationBLLContextContainer<IConfigurationBLLContext>,

        IDateTimeServiceContainer,

        IHierarchicalObjectExpanderFactoryContainer<Guid>
    {
        IExpressionParserFactory ExpressionParsers { get; }

        IValidator AnonymousObjectValidator { get; }


        ITargetSystemService GetTargetSystemService(Type domainType, bool throwOnNotFound);

        ITargetSystemService GetTargetSystemService(TargetSystem targetSystem);

        ITargetSystemService GetTargetSystemService(string targetSystemName);


        IEnumerable<ITargetSystemService> GetTargetSystemServices();



        StateBase GetNestedStateBase(StateBase stateBase);

        Event GetNestedEvent(Event @event);

        DomainType GetDomainType(Type type);

        //void RecalculateTaskInstancesAssigneesByLogin(string login);

        //void RecalculateTaskInstancesAssigneesByDomainObject<TDomainObject>(TDomainObject domainObject)
        //    where TDomainObject : class, IIdentityObject<Guid>;
    }
}