using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Exceptions;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public partial class TargetSystemService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> : BLLContextContainer<IWorkflowBLLContext>, ITargetSystemService<TBLLContext, TPersistentDomainObjectBase>

        where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
                                   ISecurityBLLContext<IAuthorizationBLLContext, TPersistentDomainObjectBase, Guid>,
                                   IAccessDeniedExceptionServiceContainer<TPersistentDomainObjectBase>,
                                   ITypeResolverContainer<string>

        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TSecurityOperationCode : struct, Enum
    {
        private readonly TargetSystemServiceCompileCache<TBLLContext, TPersistentDomainObjectBase> compileCache;

        private readonly IDictionaryCache<WorkflowInstance, IWorkflowMachine> workflowMachineCache;


        public TargetSystemService(IWorkflowBLLContext context, TBLLContext targetSystemContext, TargetSystem targetSystem, TargetSystemServiceCompileCache<TBLLContext, TPersistentDomainObjectBase> compileCache, IEnumerable<Type> workflowSourceTypes)
            : base(context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));
            if (compileCache == null) throw new ArgumentNullException(nameof(compileCache));

            this.TargetSystemContext = targetSystemContext;
            this.TargetSystem = targetSystem;
            this.WorkflowSourceTypes = workflowSourceTypes;
            this.compileCache = compileCache;

            this.TypeResolver = new DomainTypeResolver(targetSystemContext.TypeResolver).WithCache().WithLock();

            this.WorkflowTypeBuilder = this.Context.AnonymousTypeBuilder.OverrideInput((Framework.Workflow.Domain.Definition.Workflow workflow) => this.GetTypeMap(workflow)).WithCache().WithLock();

            this.CommandTypeBuilder = this.Context.AnonymousTypeBuilder.OverrideInput((Framework.Workflow.Domain.Definition.Command command) => this.GetTypeMap(command)).WithCache().WithLock();

            this.CommandAccessService = new CommandAccessService(this);

            this.workflowMachineCache = new DictionaryCache<WorkflowInstance, IWorkflowMachine>(workflowInstance =>
            {
                var workflowType = this.WorkflowTypeBuilder.GetAnonymousType(workflowInstance.Definition);

                var domainType = workflowType.GetInterfaceImplementationArgument(typeof(IDomainObjectContainer<>));

                var method = new Func<WorkflowInstance, IWorkflowMachine>(this.GetWorkflowMachine<TPersistentDomainObjectBase, IDomainObjectContainer<TPersistentDomainObjectBase>>)
                            .Method
                            .GetGenericMethodDefinition()
                            .MakeGenericMethod(domainType, workflowType);

                return method.Invoke<IWorkflowMachine>(this, new object[] { workflowInstance });
            }).WithLock();
        }


        public ICommandAccessService CommandAccessService { get; private set; }


        public IAnonymousTypeBuilder<Domain.Definition.Workflow> WorkflowTypeBuilder { get; private set; }

        public IAnonymousTypeBuilder<Domain.Definition.Command> CommandTypeBuilder { get; private set; }

        public TBLLContext TargetSystemContext { get; private set; }

        public ITypeResolver<string> TypeResolverS
        {
            get { return this.TargetSystemContext.TypeResolver; }
        }

        public Type TargetSystemContextType
        {
            get { return typeof(TBLLContext); }
        }

        public TargetSystem TargetSystem { get; private set; }

        public IEnumerable<Type> WorkflowSourceTypes { get; set; }

        public ITypeResolver<DomainType> TypeResolver { get; private set; }


        public IWorkflowMachine GetWorkflowMachine(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            return this.workflowMachineCache[workflowInstance];
        }

        public IMassWorkflowMachine GetMassWorkflowMachine(Domain.Definition.Workflow definition, WorkflowInstance[] workflowInstances)
        {
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            if (workflowInstances == null) throw new ArgumentNullException(nameof(workflowInstances));

            var workflowType = this.WorkflowTypeBuilder.GetAnonymousType(definition);

            var domainType = workflowType.GetInterfaceImplementationArgument(typeof(IDomainObjectContainer<>));

            var method = new Func<Domain.Definition.Workflow, WorkflowInstance[], IMassWorkflowMachine>(this.GetMassWorkflowMachinee<TPersistentDomainObjectBase, IDomainObjectContainer<TPersistentDomainObjectBase>>)
                         .Method
                         .GetGenericMethodDefinition()
                         .MakeGenericMethod(domainType, workflowType);

            return (IMassWorkflowMachine)method.Invoke(this, new object[] { definition, workflowInstances });
        }

        public StartWorkflowRequest GetStartWorkflowRequest(Framework.Workflow.Domain.Definition.Workflow workflow, object parameters)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var parametersRequest = from pair in parameters.ToPropertyDictionary()

                                    let name = pair.Key

                                    let value = pair.Value

                                    join parameter in workflow.Parameters on name.ToLower() equals parameter.Name.ToLower()

                                    let serializedValue = (value as TPersistentDomainObjectBase).Maybe(v => v.Id, value)

                                    select new StartWorkflowRequestParameter
                                    {
                                        Definition = parameter,
                                        Value = serializedValue.Maybe(v => v.ToString())
                                    };

            return new StartWorkflowRequest
            {
                Workflow = workflow,
                Parameters = parametersRequest.ToList()
            };
        }

        public DALChanges<WorkflowInstance> ProcessDALChanges(DALChanges preChanges)
        {
            if (preChanges == null) throw new ArgumentNullException(nameof(preChanges));

            var changes = preChanges.GetSubset(typeof(TPersistentDomainObjectBase));

            ITargetSystemService targetSystemService = this;

            var createdInstances = from dalObject in changes.CreatedItems

                                   group dalObject.Object by dalObject.Type into objectGroup

                                   where this.WorkflowSourceTypes.Contains(objectGroup.Key)

                                   let domainType = this.Context.GetDomainType(objectGroup.Key)

                                   where this.Context.Logics.StartWorkflowDomainObjectCondition.GetAvailable(domainType).Any()

                                   let array = objectGroup.ToArray(objectGroup.Key)

                                   from wfInstance in targetSystemService.TryCreate(array)

                                   select wfInstance;


            var updatedInstances = from dalObject in changes.UpdatedItems

                                   group dalObject.Object by dalObject.Type into objectGroup

                                   where this.WorkflowSourceTypes.Contains(objectGroup.Key)

                                   let domainType = this.Context.Logics.DomainType.GetByType(objectGroup.Key)

                                   where this.Context.Logics.Workflow.GetForActiveLambdaAvailable(domainType).Any()

                                      || this.Context.Logics.State.GetForDomainObjectEventAvailable(domainType).Any()

                                   let array = objectGroup.ToArray(objectGroup.Key)

                                   from wfInstance in targetSystemService.TryChange(array)

                                   select wfInstance;


            var removedInstances = from dalObject in changes.RemovedItems

                                   group dalObject.Object by dalObject.Type into objectGroup

                                   where this.WorkflowSourceTypes.Contains(objectGroup.Key)

                                   let domainType = this.Context.GetDomainType(objectGroup.Key)

                                   where this.Context.Logics.Workflow.GetForAutoRemovingAvailable(domainType).Any()

                                   let array = objectGroup.ToArray(objectGroup.Key)

                                   from wfInstance in targetSystemService.TryRemove(array)

                                   select wfInstance;

            return new DALChanges<WorkflowInstance>(createdInstances, updatedInstances, removedInstances);
        }


        public IList<WorkflowInstance> TryCreate<TDomainObject>(IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var domainObjectBLL = this.TargetSystemContext.Logics.Default.Create<TDomainObject>();

            var currentRevision = domainObjectBLL.GetCurrentRevision();

            var startConditions = this.Context.Logics.StartWorkflowDomainObjectCondition.GetAvailable(typeof(TDomainObject)).ToList();

            var startWorkflowLambdaProcessor = this.Context.ExpressionParsers.GetByStartWorkflowDomainObjectCondition<TDomainObject>();

            var domainObjectPairs = domainObjects.ToList(domainObject => new
            {
                DomainObject = domainObject,
                PrevDomainObject = domainObjectBLL.GetPreviousRevision(domainObject.Id, currentRevision).MaybeNullable(revision => domainObjectBLL.GetObjectByRevision(domainObject.Id, revision))
            });


            var startedWorkflowInstancesRequest = from pair in domainObjectPairs

                                                  from startCondition in startConditions

                                                  let del = startWorkflowLambdaProcessor.GetDelegate(startCondition)

                                                  where del(pair.PrevDomainObject, pair.DomainObject)

                                                  let startRequest = this.GetStartWorkflowRequest(pair.PrevDomainObject, pair.DomainObject, startCondition)

                                                  select this.Context.Logics.WorkflowInstance.Start(startRequest);

            var startedWorkflowInstances = startedWorkflowInstancesRequest.ToList();

            return startedWorkflowInstances;
        }

        public IList<WorkflowInstance> TryChange<TDomainObject>(IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var cachedDomainObjects = domainObjects.ToList();

            return this.TryChangeByActive(cachedDomainObjects).Concat(this.TryChangeByDomainObjectEvent(cachedDomainObjects)).ToList();
        }

        public IList<WorkflowInstance> TryRemove<TDomainObject>(IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var activeAutoRemovingWorkflows = this.Context.Logics.Workflow.GetForAutoRemovingAvailable(typeof(TDomainObject)).ToList();

            Func<IList<Guid>, IEnumerable<WorkflowInstance>> getRemovingWorkflowInstancesRequestFunc =

                                                idents =>

                                                  from workflowInstance in this.Context.Logics.WorkflowInstance.GetUnsecureQueryable()

                                                  where activeAutoRemovingWorkflows.Contains(workflowInstance.Definition)

                                                     && idents.Contains(workflowInstance.DomainObjectId)

                                                  select workflowInstance;

            var domainObjectIdents = domainObjects.ToList(domainObject => domainObject.Id);

            var chunkSize = 2000;

            var removingWorkflowInstances = domainObjectIdents.Split(chunkSize).SelectMany(z => getRemovingWorkflowInstancesRequestFunc(z.ToList())).ToList();

            removingWorkflowInstances.Foreach(this.Context.Logics.WorkflowInstance.Remove);

            return removingWorkflowInstances;
        }


        public ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(IRoleSource roleSource)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (roleSource == null) throw new ArgumentNullException(nameof(roleSource));

            var request = from role in roleSource.GetUsingRoles()

                          select this.GetSecurityProvider<TDomainObject>(role);

            return request.Or(this.TargetSystemContext.AccessDeniedExceptionService);
        }

        public ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(Role role)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            if (role.CustomSecurityProvider != null)
            {
                var del = this.Context.ExpressionParsers.GetByRoleCustomSecurityProvider<TBLLContext, TDomainObject>().GetDelegate(role);

                return del(this.TargetSystemContext);
            }

            if (!role.SecurityOperationId.IsDefault())
            {
                var code = role.SecurityOperationId.ToSecurityOperation<TSecurityOperationCode>().GetValue(() =>
                                                                                                           $"Unknown operation id: {role.SecurityOperationId}");

                return this.TargetSystemContext.SecurityService.GetSecurityProvider<TDomainObject>(code);
            }

            throw new BusinessLogicException($"Invalid role ({role.Name})");
        }


        public IEnumerable<ITryResult<object>> GetAnonymousObjects(Framework.Workflow.Domain.Definition.Workflow workflow, IEnumerable<WorkflowInstance> workflowInstances)
        {
            if (workflowInstances == null) throw new ArgumentNullException(nameof(workflowInstances));

            var func = this.compileCache.GetSafeMassWorkflowMapFunc(this.WorkflowTypeBuilder.GetAnonymousType(workflow));

            return func(this.TargetSystemContext, workflowInstances);
        }

        public object GetAnonymousObject(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            var func = this.compileCache.GetWorkflowMapFunc(this.WorkflowTypeBuilder.GetAnonymousType(workflowInstance.Definition));

            try
            {
                return func(this.TargetSystemContext, workflowInstance);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException(ex, $"Can't create anonymous object: {ex.Message}");
            }
        }

        public object GetAnonymousObject(ExecutedCommand executedCommand)
        {
            if (executedCommand == null) throw new ArgumentNullException(nameof(executedCommand));

            var func = this.compileCache.GetCommandMapFunc(this.CommandTypeBuilder.GetAnonymousType(executedCommand.Definition));

            return func(this.TargetSystemContext, executedCommand);
        }

        private IWorkflowMachine GetWorkflowMachine<TDomainObject, TWorkflow>(WorkflowInstance workflowInstance)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TWorkflow : IDomainObjectContainer<TDomainObject>
        {
            return new WorkflowMachine<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TWorkflow>(this.Context, this, workflowInstance);
        }

        private IMassWorkflowMachine GetMassWorkflowMachinee<TDomainObject, TWorkflow>(Domain.Definition.Workflow definition, WorkflowInstance[] workflowInstances)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TWorkflow : IDomainObjectContainer<TDomainObject>
        {
            return new MassWorkflowMachine<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TWorkflow>(this.Context, this, definition, workflowInstances);
        }


        private StartWorkflowRequest GetStartWorkflowRequest<TDomainObject>(TDomainObject prevDomainObject, TDomainObject domainObject, StartWorkflowDomainObjectCondition startCondition)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));
            if (startCondition == null) throw new ArgumentNullException(nameof(startCondition));

            var workflow = startCondition.Workflow;

            if (startCondition.Factory == null)
            {
                var startParameters = workflow.Parameters.ToList(parameter => new StartWorkflowRequestParameter
                {
                    Definition = parameter,
                    Value = parameter.Role == WorkflowParameterRole.DomainObject ? domainObject.Id.ToString() : null
                });

                return new StartWorkflowRequest
                {
                    Workflow = startCondition.Workflow,
                    Parameters = startParameters
                };
            }
            else
            {
                var workflowType = this.WorkflowTypeBuilder.GetAnonymousType(workflow);

                return new Func<TDomainObject, TDomainObject, StartWorkflowDomainObjectCondition, StartWorkflowRequest>(this.GetStartWorkflowRequest<TDomainObject, object>)
                      .CreateGenericMethod(typeof(TDomainObject), workflowType)
                      .Invoke<StartWorkflowRequest>(this, prevDomainObject, domainObject, startCondition);
            }
        }

        private StartWorkflowRequest GetStartWorkflowRequest<TDomainObject, TWorkflow>(TDomainObject prevDomainObject, TDomainObject domainObject, StartWorkflowDomainObjectCondition startCondition)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var del = this.Context.ExpressionParsers.GetByStartWorkflowDomainObjectConditionFactory<TBLLContext, TDomainObject, TWorkflow>()
                                  .GetDelegate(startCondition);

            var startupObject = del(this.TargetSystemContext, prevDomainObject, domainObject);

            return this.GetStartWorkflowRequest(startCondition.Workflow, startupObject);
        }


        private IList<WorkflowInstance> TryChangeByActive<TDomainObject>(IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var domainObjectBLL = this.TargetSystemContext.Logics.Default.Create<TDomainObject>();

            var currentRevision = domainObjectBLL.GetCurrentRevision();

            var activeChangeWorkflows = this.Context.Logics.Workflow.GetForActiveLambdaAvailable(typeof(TDomainObject)).ToList();



            var domainObjectPairs = domainObjects.ToList(domainObject => new
            {
                DomainObject = domainObject,
                PrevDomainObject = domainObjectBLL.GetPreviousRevision(domainObject.Id, currentRevision).MaybeNullable(revision => domainObjectBLL.GetObjectByRevision(domainObject.Id, revision))
            });

            var domainObjectIdents = domainObjectPairs.Where(pair => pair.PrevDomainObject != null)
                                                      .ToList(pair => pair.DomainObject.Id);

            var changeActiveWorkflowInstancesRequest = from workflowInstance in this.Context.Logics.WorkflowInstance.GetUnsecureQueryable()

                                                       where activeChangeWorkflows.Contains(workflowInstance.Definition)

                                                          && domainObjectIdents.Contains(workflowInstance.DomainObjectId)

                                                       select workflowInstance;


            var changedActiveWorkflowsRequest = from workflowInstance in changeActiveWorkflowInstancesRequest.ToList()

                                                let machine = this.GetWorkflowMachine(workflowInstance)

                                                where machine.TryChangeActive()

                                                select workflowInstance;


            var changedActiveWorkflows = changedActiveWorkflowsRequest.ToList();

            return changedActiveWorkflows;
        }

        private IList<WorkflowInstance> TryChangeByDomainObjectEvent<TDomainObject>(IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var domainObjectIdents = domainObjects.ToList(domainObject => domainObject.Id);

            var domainObjectEventStatesRequest = this.Context.Logics.State.GetForDomainObjectEventAvailable(typeof(TDomainObject));

            var wiQ = this.Context.Logics.WorkflowInstance.GetUnsecureQueryable();

            var changeByDomainObjectEventsRequest = from state in domainObjectEventStatesRequest

                                                    join workflowInstance in wiQ on state equals workflowInstance.CurrentState.Definition

                                                    where workflowInstance.Active && domainObjectIdents.Contains(workflowInstance.DomainObjectId)

                                                    select workflowInstance;

            var changeByDomainObjectEvents = changeByDomainObjectEventsRequest.ToList();

            var changedActiveWorkflowsRequest = from workflowInstance in changeByDomainObjectEvents

                                                let machine = this.GetWorkflowMachine(workflowInstance)

                                                let result = machine.ProcessCurrentStateEvent(WorkflowProcessSettings.Default) // TODO: надо перевести на SkipTryFinishParallel

                                                where result != null

                                                select workflowInstance;


            var changedActiveWorkflows = changedActiveWorkflowsRequest.ToList();

            return changedActiveWorkflows;
        }

        #region ITargetSystemService Members

        Type ITargetSystemService.PersistentDomainObjectBaseType
        {
            get { return typeof(TPersistentDomainObjectBase); }
        }

        object ITargetSystemService.TargetSystemContext
        {
            get { return this.TargetSystemContext; }
        }


        IList<WorkflowInstance> ITargetSystemService.TryCreate(Array domainObjects)
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var domainObjectType = domainObjects.GetElementType();

            if (!typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainObjectType))
            {
                throw new BusinessLogicException("Domain Type {0} must be derived from {1}", domainObjectType.Name, typeof(TPersistentDomainObjectBase).Name);
            }

            var method = new Func<TPersistentDomainObjectBase[], IList<WorkflowInstance>>(this.TryCreate)
                .Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(domainObjectType);

            return (IList<WorkflowInstance>)method.Invoke(this, new object[] { domainObjects });
        }

        IList<WorkflowInstance> ITargetSystemService.TryChange(Array domainObjects)
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var domainObjectType = domainObjects.GetElementType();

            if (!typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainObjectType))
            {
                throw new BusinessLogicException("Domain Type {0} must be derived from {1}", domainObjectType.Name, typeof(TPersistentDomainObjectBase).Name);
            }

            var method = new Func<TPersistentDomainObjectBase[], IList<WorkflowInstance>>(this.TryChange)
                .Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(domainObjectType);

            return (IList<WorkflowInstance>)method.Invoke(this, new object[] { domainObjects });
        }

        IList<WorkflowInstance> ITargetSystemService.TryRemove(Array domainObjects)
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var domainObjectType = domainObjects.GetElementType();

            if (!typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainObjectType))
            {
                throw new BusinessLogicException("Domain Type {0} must be derived from {1}", domainObjectType.Name, typeof(TPersistentDomainObjectBase).Name);
            }

            var method = new Func<TPersistentDomainObjectBase[], IList<WorkflowInstance>>(this.TryRemove)
                .Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(domainObjectType);

            return (IList<WorkflowInstance>)method.Invoke(this, new object[] { domainObjects });
        }



        #endregion
    }
}
