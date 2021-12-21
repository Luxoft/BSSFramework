using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public interface ITargetSystemService :

        ITypeResolverContainer<DomainType>,

        ITargetSystemElement<TargetSystem>,

        IAnonymousObjectBuilder<Framework.Workflow.Domain.Runtime.WorkflowInstance>,
        IAnonymousObjectBuilder<Framework.Workflow.Domain.Runtime.ExecutedCommand>
    {
        IAnonymousTypeBuilder<Framework.Workflow.Domain.Definition.Workflow> WorkflowTypeBuilder { get; }

        IAnonymousTypeBuilder<Framework.Workflow.Domain.Definition.Command> CommandTypeBuilder { get; }


        ICommandAccessService CommandAccessService { get; }



        object TargetSystemContext { get; }

        ITypeResolver<string> TypeResolverS { get; }

        Type TargetSystemContextType { get; }


        Type PersistentDomainObjectBaseType { get; }


        IList<WorkflowInstance> TryCreate([NotNull]Array domainObjects);

        IList<WorkflowInstance> TryChange([NotNull]Array domainObjects);

        IList<WorkflowInstance> TryRemove([NotNull]Array domainObjects);




        IWorkflowMachine GetWorkflowMachine([NotNull]WorkflowInstance workflowInstance);

        IMassWorkflowMachine GetMassWorkflowMachine([NotNull]Domain.Definition.Workflow definition, [NotNull]WorkflowInstance[] workflowInstances);


        IEnumerable<ITryResult<object>> GetAnonymousObjects([NotNull]Framework.Workflow.Domain.Definition.Workflow workflow, [NotNull]IEnumerable<WorkflowInstance> workflowInstances);



        StartWorkflowRequest GetStartWorkflowRequest([NotNull] Framework.Workflow.Domain.Definition.Workflow workflow, [NotNull] object parameters);



        List<AvailableTaskInstanceWorkflowGroup> GetAvailableTaskInstanceWorkflowGroups([NotNull]DomainType sourceType, Guid domainObjectId = default (Guid));



        bool ExistsObject([NotNull]DomainType domainType, Guid domainObjectId);


        SecurityProvider<TaskInstance> GetTaskInstanceSecurityProvider([NotNull]IGrouping<Role, Task> taskRoleGroup);


        DALChanges<WorkflowInstance> ProcessDALChanges([NotNull] DALChanges changes);
    }

    public interface ITargetSystemService<out TBLLContext, in TPersistentDomainObjectBase> : ITargetSystemService
        where TPersistentDomainObjectBase : class
    {
        new TBLLContext TargetSystemContext { get; }


        IList<WorkflowInstance> TryCreate<TDomainObject>([NotNull]IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase;

        IList<WorkflowInstance> TryChange<TDomainObject>([NotNull]IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase;

        IList<WorkflowInstance> TryRemove<TDomainObject>([NotNull]IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase;


        ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>([NotNull]IRoleSource roleSource)
            where TDomainObject : class, TPersistentDomainObjectBase;

        ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>([NotNull]Role role)
            where TDomainObject : class, TPersistentDomainObjectBase;
    }
}
