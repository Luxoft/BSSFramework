using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public class MassWorkflowMachine<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TWorkflow> : BLLContextContainer<IWorkflowBLLContext>, IMassWorkflowMachine

        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TWorkflow : IDomainObjectContainer<TDomainObject>
    {
        private readonly ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> _targetSystemService;
        private readonly Domain.Definition.Workflow _workflow;

        private readonly WorkflowInstance[] _workflowInstances;



        public MassWorkflowMachine(IWorkflowBLLContext context, ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> targetSystemService, Framework.Workflow.Domain.Definition.Workflow workflow, WorkflowInstance[] workflowInstances)
            : base(context)
        {
            if (targetSystemService == null) throw new ArgumentNullException(nameof(targetSystemService));
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));
            if (workflowInstances == null) throw new ArgumentNullException(nameof(workflowInstances));

            this._targetSystemService = targetSystemService;
            this._workflow = workflow;
            this._workflowInstances = workflowInstances;
        }


        public ITryResult<WorkflowProcessResult>[] ProcessTimeouts()
        {
            var tryWfObjects = this._targetSystemService.GetAnonymousObjects(this._workflow, this._workflowInstances)
                                   .ZipStrong(this._workflowInstances, (tryWfObject, wfInstance) => new { TryWfObject = tryWfObject.Select(obj => (TWorkflow)obj), State = wfInstance.CurrentState }).ToList();


            var resultRequest = from pair in tryWfObjects

                                where pair.State.IsCurrent

                                let res = pair.TryWfObject.SelectMany(workflowObject =>

                                    new ImplementedWorkflowMachine(this.Context, this._targetSystemService, pair.State.Workflow, workflowObject)
                                   .Pipe(machine => TryResult.Catch(() => machine.ProcessCurrentStateEvent(WorkflowProcessSettings.Default)))) // TODO: надо перевести на SkipTryFinishParallel

                                select res;

            return resultRequest.ToArray();
        }

        private class ImplementedWorkflowMachine : WorkflowMachineBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TWorkflow>
        {
            private readonly TWorkflow _workflowObject;


            public ImplementedWorkflowMachine(IWorkflowBLLContext context, ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> targetSystemService, WorkflowInstance workflowInstance, TWorkflow workflowObject)
                : base(context, targetSystemService, workflowInstance)
            {
                this._workflowObject = workflowObject;
            }


            protected override TWorkflow WorkflowObject
            {
                get { return this._workflowObject; }
            }
        }
    }
}