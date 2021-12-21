using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial class TargetSystemService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>
    {
        public List<AvailableTaskInstanceWorkflowGroup> GetAvailableTaskInstanceWorkflowGroups(DomainType sourceType, Guid domainObjectId = new Guid())
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

            return this.GetAvailableTaskInstanceWorkflowGroups(this.TypeResolver.Resolve(sourceType, true), domainObjectId);
        }

        public bool ExistsObject(DomainType domainType, Guid domainObjectId)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return this.ExistsObject(this.TypeResolver.Resolve(domainType), domainObjectId);
        }

        private bool ExistsObject([NotNull] Type domainType, Guid domainObjectId)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return new Func<Guid, bool>(this.ExistsObject<TPersistentDomainObjectBase>)
                  .CreateGenericMethod(domainType)
                  .Invoke<bool>(this, domainObjectId);
        }

        private bool ExistsObject<TDomainObject>(Guid domainObjectId)
              where TDomainObject : class, TPersistentDomainObjectBase
        {
            return this.TargetSystemContext.Logics.Default.Create<TDomainObject>().GetUnsecureQueryable().Any(domainObject => domainObject.Id == domainObjectId);
        }


        private IEnumerable<AvailableTaskInstanceGroupItem> GetAvailableTaskInstanceGroupItems([NotNull] WorkflowSource workflowSource, [NotNull] IGrouping<Task, TaskInstance> byTaskGroup)
        {
            if (workflowSource == null) throw new ArgumentNullException(nameof(workflowSource));
            if (byTaskGroup == null) throw new ArgumentNullException(nameof(byTaskGroup));

            return from pathG in byTaskGroup.GroupBy(ti => this.GetWorkflowMachine(ti.Workflow).GetReversePath(workflowSource).Reverse().ToArray(), ArrayComparer<string>.Value)

                   from commanG in pathG.GroupBy(taskInstance => taskInstance.Definition
                                                                             .Commands
                                                                             .Where(command => this.CommandAccessService.HasAccess(command, taskInstance.Workflow))
                                                                             .OrderBy(command => command.Name)
                                                                             .ThenBy(command => command.OrderIndex)
                                                                             .ToArray(), ArrayComparer<Command>.Value)

                   where commanG.Key.Any()

                   select new AvailableTaskInstanceGroupItem { Path = pathG.Key, TaskInstances = commanG.ToList(), Commands = commanG.Key };
        }

        private List<AvailableTaskInstanceWorkflowGroup> GetAvailableTaskInstanceWorkflowGroups(Type domainType, Guid domainObjectId)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return new Func<Guid, List<AvailableTaskInstanceWorkflowGroup>>(this.GetAvailableGroups<TPersistentDomainObjectBase>)
                  .CreateGenericMethod(domainType)
                  .Invoke<List<AvailableTaskInstanceWorkflowGroup>>(this, domainObjectId);
        }

        private List<AvailableTaskInstanceWorkflowGroup> GetAvailableGroups<TDomainObject>(Guid domainObjectId)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            var sourceType = this.Context.GetDomainType(typeof(TDomainObject));

            var domainObject = this.TargetSystemContext.Logics.Default.Create<TDomainObject>().GetById(domainObjectId, IdCheckMode.SkipEmpty);

            return this.Context.Logics.Default.Create<WorkflowSource>()
                                              .GetListBy(source => source.Type == sourceType)
                                              .SelectMany(source => this.GetAvailableGroups(source, domainObject))
                                              .ToList();
        }

        private List<AvailableTaskInstanceWorkflowGroup> GetAvailableGroups<TSourceDomainObject>(WorkflowSource workflowSource, TSourceDomainObject groupDomainObject)
             where TSourceDomainObject : class, TPersistentDomainObjectBase
        {
            if (workflowSource == null) throw new ArgumentNullException(nameof(workflowSource));

            var elementType = this.TypeResolver.Resolve(workflowSource.Workflow.DomainType, true);

            var workflowInstances = new Func<WorkflowSource, TSourceDomainObject, IEnumerable<WorkflowInstance>>(this.GetAvailableWorkflowInstances<TSourceDomainObject, TSourceDomainObject>)
                  .CreateGenericMethod(typeof(TSourceDomainObject), elementType)
                  .Invoke<IEnumerable<WorkflowInstance>>(this, workflowSource, groupDomainObject)
                  .ToList();

            return this.GetAvailableGroupsI(workflowSource, workflowInstances);
        }

        private IEnumerable<WorkflowInstance> GetAvailableWorkflowInstances<TSourceDomainObject, TDomainObject>(WorkflowSource workflowSource, TSourceDomainObject singleGroupDomainObject)
            where TSourceDomainObject : class, TPersistentDomainObjectBase
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (workflowSource == null) throw new ArgumentNullException(nameof(workflowSource));

            var elementsDel = this.Context.ExpressionParsers.GetByWorkflowSourceElements<TBLLContext, TSourceDomainObject, TDomainObject>().GetDelegate(workflowSource);

            var elementsExpr = elementsDel(this.TargetSystemContext);

            if (singleGroupDomainObject == null)
            {
                var sourceQueryable = this.TargetSystemContext.Logics.Default.Create<TSourceDomainObject>().GetUnsecureQueryable();

                var domainObjectFilter = elementsExpr.SelectO((TDomainObject domainObject, IEnumerable<TDomainObject> elements) => elements.Contains(domainObject))
                                                     .Select(f => sourceQueryable.Any(f));

                var rootQueryable = this.TargetSystemContext.Logics.Default.Create<TDomainObject>().GetUnsecureQueryable().Where(domainObjectFilter);

                return from wf in workflowSource.Workflow.GetAllChildren()

                       where wf.Tasks.Any()

                       let toRootPath = wf.GetAllParents().TakeWhile(w => w != workflowSource.Workflow)
                                                          .Aggregate(ExpressionHelper.GetIdentity<WorkflowInstance>(), (filter, _) => filter.Select(wi => wi.Owner))

                       let rootFilter = toRootPath.Select(wi => wi.Definition == workflowSource.Workflow && rootQueryable.Select(r => r.Id).Contains(wi.DomainObjectId))

                       let workflowInstanceSecurityProvider = this.GetWorkflowInstanceSecurityProvider(wf)

                       let bll = this.Context.Logics.WorkflowInstanceFactory.Create(workflowInstanceSecurityProvider)

                       from wi in bll.GetUnsecureQueryable().Where(wi => wi.Active && !wi.IsFinished && wi.CurrentState.Definition.Type == StateType.Main)
                                                    .Where(rootFilter)

                       select wi;
            }
            else
            {
                var domainObjects = elementsExpr.Eval(singleGroupDomainObject, LambdaCompileCacheContainer.Get<TSourceDomainObject, TDomainObject>()).ToList();

                var domainObjectIdents = domainObjects.ToList(domainObject => domainObject.Id);

                var baseWorkflowInstances = this.Context.Logics.WorkflowInstance.GetListBy(wi => domainObjectIdents.Contains(wi.DomainObjectId)
                                                                                              && wi.Active
                                                                                              && !wi.IsFinished
                                                                                              && wi.Definition == workflowSource.Workflow);

                return baseWorkflowInstances.GetAllElements(workflowInstance => workflowInstance.CurrentState.SubWorkflows.Where(subWorkflow => subWorkflow.IsAvailable));
            }
        }

        private List<AvailableTaskInstanceWorkflowGroup> GetAvailableGroupsI([NotNull] WorkflowSource workflowSource, [NotNull] IEnumerable<WorkflowInstance> workflowInstances)
        {
            if (workflowSource == null) throw new ArgumentNullException(nameof(workflowSource));
            if (workflowInstances == null) throw new ArgumentNullException(nameof(workflowInstances));


            var resultRequest = from workflowInstance in workflowInstances

                                from taskInstance in workflowInstance.CurrentState.Tasks

                                group taskInstance by taskInstance.Definition into taskG

                                let items = this.GetAvailableTaskInstanceGroupItems(workflowSource, taskG).ToList()

                                where items.Any()

                                let availableTaskInstanceGroup = new AvailableTaskInstanceGroup
                                {
                                    Task = taskG.Key,
                                    Items = items
                                }

                                group availableTaskInstanceGroup by taskG.Key.Workflow into workflowG

                                select new AvailableTaskInstanceWorkflowGroup
                                {
                                    Source = workflowSource,
                                    Workflow = workflowG.Key,
                                    Items = workflowG.ToList()
                                };

            return resultRequest.ToList();
        }

        private static readonly LambdaCompileCacheContainer LambdaCompileCacheContainer = new LambdaCompileCacheContainer();
    }
}
