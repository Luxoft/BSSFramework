using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public abstract partial class WorkflowMachineBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TWorkflow> : BLLContextContainer<IWorkflowBLLContext>, IWorkflowMachine

        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>

        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>

        where TDomainObject : class, TPersistentDomainObjectBase

        where TWorkflow : IDomainObjectContainer<TDomainObject>
    {
        protected WorkflowMachineBase(IWorkflowBLLContext context, ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> targetSystemService, WorkflowInstance workflowInstance)
            : base(context)
        {
            if (targetSystemService == null) throw new ArgumentNullException(nameof(targetSystemService));
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            this.TargetSystemService = targetSystemService;
            this.WorkflowInstance = workflowInstance;
        }

        public WorkflowInstance WorkflowInstance { get; private set; }


        protected ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> TargetSystemService { get; private set; }




        protected TBLLContext MainContext
        {
             get { return this.TargetSystemService.TargetSystemContext; }
        }

        protected abstract TWorkflow WorkflowObject { get; }


        public WorkflowProcessResult ProcessTransition(Transition transition, WorkflowProcessSettings processSettings)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            return this.InternalProcessTransition(transition, processSettings).Sum();
        }

        private IEnumerable<WorkflowProcessResult> InternalProcessTransition(Transition transition, WorkflowProcessSettings processSettings)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            var newState = this.Context.GetNestedStateBase(transition.To);

            var transitionInstance = this.SwitchState(transition, newState);

            yield return new WorkflowProcessResult(transitionInstance);

            if (transition.PostActions.Any())
            {
                this.ExecutePostAction(transition);
            }

            yield return this.ProcessState(newState, processSettings);

            this.Save();
            this.Context.Logics.StateInstance.Save(transitionInstance.To);
        }


        public Event GetCurrentStateEvent()
        {
            return new StateInstanceEventProcessor(this.Context, this).GetEvent(this.WorkflowInstance.CurrentState);
        }

        public WorkflowProcessResult TryFinishParallel()
        {
            if (this.WorkflowInstance.IsAborted)
            {
                return WorkflowProcessResult.Empty;
            }

            if (this.WorkflowInstance.CurrentStateDefinition.Type != StateType.Parallel)
            {
                throw new BusinessLogicException("Invalid current state, exptected parallel");
            }

            var transition = this.GetCurrentStateEventTransition();

            if (transition == null)
            {
                if (this.WorkflowInstance.CurrentState.SubWorkflows.All(subWF => subWF.IsFinished))
                {
                    throw new BusinessLogicException($"All subWorkflow in workflow {this.WorkflowInstance.Name} finished, but transition not selected");
                }

                return WorkflowProcessResult.Empty;
            }
            else
            {
                this.WorkflowInstance.CurrentState.SubWorkflows.Where(subWF => !subWF.IsFinished).GetAllChildrenM().Foreach(subWF =>
                    subWF.Abort());

                var res = this.ProcessTransition(transition, WorkflowProcessSettings.Default);

                return res;
            }
        }

        public bool IsTimeout(StateTimeoutEvent stateTimeoutEvent, DateTime checkDate)
        {
            if (stateTimeoutEvent == null) throw new ArgumentNullException(nameof(stateTimeoutEvent));

            if (stateTimeoutEvent.Workflow != this.WorkflowInstance.Definition)
            {
                throw new System.ArgumentException("stateTimeout");
            }

            var del = this.Context.ExpressionParsers.GetByStateTimeoutCondition<TBLLContext, TWorkflow>().GetDelegate(stateTimeoutEvent);

            var timeoutDate = del(this.MainContext, this.WorkflowObject);

            return timeoutDate < checkDate;
        }

        public bool IsEvaluated(StateDomainObjectEvent stateDomainObjectEvent)
        {
            if (stateDomainObjectEvent == null) throw new ArgumentNullException(nameof(stateDomainObjectEvent));

            if (stateDomainObjectEvent.Workflow != this.WorkflowInstance.Definition)
            {
                throw new System.ArgumentException("stateTimeout");
            }

            var del = this.Context.ExpressionParsers.GetByStateDomainObjectCondition<TBLLContext, TWorkflow>().GetDelegate(stateDomainObjectEvent);

            var isEvaluated = del(this.MainContext, this.WorkflowObject);

            return isEvaluated;
        }

        public bool GetConditionResult(ConditionState conditionState)
        {
            if (conditionState == null) throw new ArgumentNullException(nameof(conditionState));

            var del = this.Context.ExpressionParsers.GetByConditionState<TBLLContext, TWorkflow>().GetDelegate(conditionState);

            var condResult = del(this.MainContext, this.WorkflowObject);

            return condResult;
        }

        public bool GetParallelStateFinalEventResult(ParallelStateFinalEvent parallelStateFinalEvent)
        {
            if (parallelStateFinalEvent == null) throw new ArgumentNullException(nameof(parallelStateFinalEvent));

            var del = this.Context.ExpressionParsers.GetByParallelStateFinalEventCondition<TBLLContext, TWorkflow>()
                                                        .GetDelegate(parallelStateFinalEvent);

            var condResult = del(this.MainContext, this.WorkflowObject, this.WorkflowInstance);

            return condResult;
        }

        public WorkflowProcessResult ProcessCurrentState(WorkflowProcessSettings processSettings)
        {
            if (this.WorkflowInstance.IsAborted)
            {
                return WorkflowProcessResult.Empty;
            }

            return this.ProcessState(this.WorkflowInstance.CurrentStateDefinition, processSettings);
        }

        public WorkflowProcessResult ProcessCurrentStateEvent(WorkflowProcessSettings processSettings)
        {
            return this.GetCurrentStateEvent().Maybe(@event => this.ProcessEvent(@event, processSettings)) ?? WorkflowProcessResult.Empty;
        }

        public WorkflowProcessResult ProcessEvent(Event @event, WorkflowProcessSettings processSettings)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            var transition = this.WorkflowInstance.GetEventTransition(@event);

            return this.ProcessTransition(transition, processSettings);
        }

        public void ExecuteCommandAction(ExecutedCommand executedCommand)
        {
            var commandType = this.TargetSystemService.CommandTypeBuilder.GetAnonymousType(executedCommand.Definition);

            var method = new Action<ExecutedCommand>(this.ExecuteCommandAction<object>)
                        .Method
                        .GetGenericMethodDefinition()
                        .MakeGenericMethod(commandType);

            method.Invoke(this, new object[] { executedCommand });
        }

        public bool HasAccess(Task task)
        {

            if (task == null) throw new ArgumentNullException(nameof(task));

            return task.Commands.Any(this.HasAccess);
        }

        public bool HasAccess(Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            return this.TargetSystemService.GetSecurityProvider<TDomainObject>(command)
                                           .HasAccess(this.WorkflowObject.DomainObject);
        }

        public UnboundedList<string> GetAccessors(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            return this.TargetSystemService.GetSecurityProvider<TDomainObject>(task)
                                           .GetAccessors(this.WorkflowObject.DomainObject);
        }

        public UnboundedList<string> GetAccessors(Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            return this.TargetSystemService.GetSecurityProvider<TDomainObject>(command)
                                           .GetAccessors(this.WorkflowObject.DomainObject);
        }

        public bool TryChangeActive()
        {
            var activeLambdaProcessor = this.Context.ExpressionParsers.GetByWorkflowActiveCondition<TBLLContext, TWorkflow>();

            var oldActive = this.WorkflowInstance.Active;

            var del = activeLambdaProcessor.GetDelegate(this.WorkflowInstance.Definition);

            var newActive = del(this.MainContext, this.WorkflowObject);

            var activeChanged = oldActive != newActive;

            if (activeChanged)
            {
                this.WorkflowInstance.Active = newActive;
                this.Save();
            }

            return activeChanged;
        }


        public StateInstance SwitchState(StateBase newState)
        {
            if (newState == null) throw new ArgumentNullException(nameof(newState));

            var newStateInstance = new StateInstance(this.WorkflowInstance, newState);

            this.WorkflowInstance.CurrentState = newStateInstance;

            if (newState is State)
            {
                var state = newState as State;

                this.WorkflowInstance.IsFinished = state.IsFinal;

                foreach (var task in state.Tasks)
                {
                    new TaskInstance(newStateInstance, task);
                }
            }

            if (!string.IsNullOrWhiteSpace(newState.AutoSetStatePropertyName))
            {
                var domainObject = this.WorkflowObject.DomainObject;

                var property = typeof(TDomainObject).GetProperty(newState.AutoSetStatePropertyName, true);

                var setValue = property.PropertyType == typeof(string) ? newState.AutoSetStatePropertyValue
                                                                   : Enum.Parse(property.PropertyType, newState.AutoSetStatePropertyValue, false);
                var previousValue = property.GetValue(domainObject, null);

                if (!previousValue.Equals(setValue))
                {
                    property.SetValue(domainObject, setValue, null);

                    this.MainContext.Logics.Implemented.Create<TDomainObject>().Save(domainObject);
                }
            }

            return newStateInstance;
        }

        public void Abort()
        {
            if (this.WorkflowInstance.IsFinished)
            {
                throw new BusinessLogicException("Aborting workflow {0} already {1}", this.WorkflowInstance.Name, this.WorkflowInstance.IsAborted ? "aborted" : "finished");
            }

            this.WorkflowInstance.Abort();

            foreach (var subWFInstance in this.WorkflowInstance.CurrentState.SubWorkflows.Where(wf => !wf.IsFinished))
            {
                this.Context.GetWorkflowMachine(subWFInstance).Abort();
            }

            if (this.WorkflowInstance.Owner != null)
            {
                this.Context.GetWorkflowMachine(this.WorkflowInstance.Owner).TryFinishParallel();
            }

            this.Save();
        }

        public void Save()
        {
            this.Context.Logics.WorkflowInstance.Save(this.WorkflowInstance);
        }






        private TransitionInstance SwitchState(Transition newTransition, StateBase newState)
        {
            if (newTransition == null) throw new ArgumentNullException(nameof(newTransition));

            var prevStateIntance = this.WorkflowInstance.CurrentState;

            if (newTransition.From != prevStateIntance.Definition)
            {
                throw new BusinessLogicException("Invalid transition");
            }

            var newStateInstance = this.SwitchState(newState);

            return new TransitionInstance(this.WorkflowInstance, newTransition, prevStateIntance, newStateInstance);
        }

        private Transition GetCurrentStateEventTransition()
        {
            return this.GetCurrentStateEvent().Maybe(e => this.WorkflowInstance.GetEventTransition(e));
        }

        private WorkflowProcessResult ProcessState(StateBase state, WorkflowProcessSettings processSettings)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (state.Workflow != this.WorkflowInstance.Definition)
            {
                throw new System.ArgumentException("state");
            }

            if (state is ConditionState)
            {
                var ifThenElseTransition = this.GetCurrentStateEventTransition();

                return this.ProcessTransition(ifThenElseTransition, processSettings);
            }
            else if (state is ParallelState)
            {
                return this.ExecuteParallel();
            }
            else if (state is State)
            {
                if (((State)state).Maybe(v => v.IsFinal) && this.WorkflowInstance.Owner != null)
                {
                    if (processSettings.HasFlag(WorkflowProcessSettings.SkipTryFinishParallel))
                    {
                        return new WorkflowProcessResult(this.WorkflowInstance.Owner);
                    }
                    else
                    {
                        return this.Context.GetWorkflowMachine(this.WorkflowInstance.Owner).TryFinishParallel();
                    }
                }
                else
                {
                    return WorkflowProcessResult.Empty;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(state));
            }
        }

        private void ExecutePostAction(Transition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            var lambdaWorker = this.Context.ExpressionParsers.GetByTransitionAction<TBLLContext, TWorkflow>();

            foreach (var transitionAction in transition.PostActions.OrderBy(p => p.OrderIndex))
            {
                var del = lambdaWorker.GetDelegate(transitionAction);

                del(this.MainContext, this.WorkflowObject);
            }
        }

        private WorkflowProcessResult ExecuteParallel()
        {
            var currentState = this.WorkflowInstance.CurrentState;
            var currentStateDefinition = (ParallelState)currentState.Definition;

            var subWfRequests = this.GetStartSubWorkflowRequests().ToList();

            if (subWfRequests.Any())
            {
                var resultReqest = from startRequest in subWfRequests

                                   let subMachine = this.Context.StartWorkflowMachine(startRequest)

                                   select subMachine.ProcessCurrentState(WorkflowProcessSettings.SkipTryFinishParallel);

                var preResult = resultReqest.Sum();

                var tryFinishInstance = preResult.TryFinishParallelInstances.SingleOrDefault(() => new BusinessLogicException("Expected single instance"));

                if (tryFinishInstance.Maybe(wi => wi != this.WorkflowInstance))
                {
                    throw new BusinessLogicException("Expected current wf");
                }

                return this.Context.FinishParallels(preResult);
            }
            else
            {
                var res = this.TryFinishParallel();

                if (currentState == this.WorkflowInstance.CurrentState)
                {
                    throw new BusinessLogicException($"Empty parallel states (\"{currentStateDefinition.Name})\"");
                }

                return res;
            }
        }

        private IEnumerable<StartWorkflowRequest> GetStartSubWorkflowRequests()
        {
            var currentState = this.WorkflowInstance.CurrentState;
            var currentStateDefinition = (ParallelState)currentState.Definition;

            foreach (var item in currentStateDefinition.StartItems)
            {
                if (item.Factory == null)
                {
                    var baseStartRequest = this.TargetSystemService.GetStartWorkflowRequest(item.SubWorkflow, new { this.WorkflowObject.DomainObject });

                    yield return new StartWorkflowRequest.StartSubWorkflowRequest
                    {
                        Workflow = item.SubWorkflow,
                        Parameters = baseStartRequest.Parameters,
                        OwnerWorkflowState = this.WorkflowInstance.CurrentState
                    };
                }
                else
                {
                    var startupObjects = new Func<ParallelStateStartItem, IEnumerable<object>>(this.GetCreateSubWorkflows<object>)
                                        .CreateGenericMethod(this.TargetSystemService.WorkflowTypeBuilder.GetAnonymousType(item.SubWorkflow))
                                        .Invoke<IEnumerable<object>>(this, item).ToList();

                    foreach (var startupObject in startupObjects)
                    {
                        var baseStartRequest = this.TargetSystemService.GetStartWorkflowRequest(item.SubWorkflow, startupObject);

                        yield return new StartWorkflowRequest.StartSubWorkflowRequest
                        {
                            Workflow = baseStartRequest.Workflow,
                            Parameters = baseStartRequest.Parameters,
                            OwnerWorkflowState = this.WorkflowInstance.CurrentState
                        };
                    }
                }
            }
        }


        private IEnumerable<TCreatedSubWorkflow> GetCreateSubWorkflows<TCreatedSubWorkflow>(ParallelStateStartItem parallelStateItem)
        {
            var del = this.Context.ExpressionParsers.GetByParallelStateStartItemFactory<TBLLContext, TWorkflow, TCreatedSubWorkflow>()
                                                        .GetDelegate(parallelStateItem);

            return del(this.MainContext, this.WorkflowObject);
        }

        private void ExecuteCommandAction<TCommand>(ExecutedCommand executedCommand)
        {
            var lambdaProcessor = this.Context.ExpressionParsers.GetByCommandExecuteAction<TBLLContext, TWorkflow, TCommand>();

            var del = lambdaProcessor.GetDelegate(executedCommand.Definition);

            del(this.MainContext, this.WorkflowObject, (TCommand)this.TargetSystemService.GetAnonymousObject(executedCommand));
        }
    }


    public class WorkflowMachine<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TWorkflow> : WorkflowMachineBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TWorkflow> where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, Guid> where TPersistentDomainObjectBase : class, IIdentityObject<Guid> where TDomainObject : class, TPersistentDomainObjectBase where TWorkflow : IDomainObjectContainer<TDomainObject>
    {
        private readonly Lazy<TWorkflow> _lazyWorkflowObject;


        public WorkflowMachine(IWorkflowBLLContext context, ITargetSystemService<TBLLContext, TPersistentDomainObjectBase> targetSystemService, WorkflowInstance workflowInstance)
            : base(context, targetSystemService, workflowInstance)
        {
            this._lazyWorkflowObject = LazyHelper.Create(() => (TWorkflow)this.TargetSystemService.GetAnonymousObject(this.WorkflowInstance));
        }


        protected override TWorkflow WorkflowObject
        {
            get { return this._lazyWorkflowObject.Value; }
        }
    }

    //public static class PropertySetHelper
    //{
    //    public static Action<TDomainObject> GetSetAction<TDomainObject>(string propertyName, string propertyValue)
    //    {
    //        if (string.IsNullOrWhiteSpace(propertyName))
    //        {
    //            if (string.IsNullOrWhiteSpace(propertyValue))
    //            {
    //                return null;
    //            }

    //            throw new BusinessLogicException(string.Format("State \"{0}\" Error. Can't apply AutoSetStatePropertyValue \"{1}\", because AutoSetStatePropertyName is empty", stateBase.Name, stateBase.AutoSetStatePropertyValue));
    //        }

    //        return InternalHelper<TDomainObject>.Cache.GetValue(propertyName, propertyValue);
    //    }

    //    private static class InternalHelper<TDomainObject>
    //    {
    //        public static readonly IDictionaryCache<Tuple<string, string>, Action<TDomainObject>> Cache = new ConcurrentDictionaryCache<Tuple<string, string>, Action<TDomainObject>>(t =>
    //        {


    //            var prop = domainType.GetProperty(stateBase.AutoSetStatePropertyName, true);

    //            if (!prop.HasSetMethod())
    //            {
    //                throw new BusinessLogicException(string.Format("State \"{0}\" Error. Property \"{1}\" in domainType \"{2}\" must be have public setter", stateBase.Name, prop.Name, domainType.Name));
    //            }

    //            if (prop.PropertyType == typeof(string))
    //            {
    //                return;
    //            }

    //            if (!prop.PropertyType.IsEnum)
    //            {
    //                throw new BusinessLogicException(string.Format("State \"{0}\" Error. Property \"{1}\" in domainType \"{2}\" must be System.Enum or System.String", stateBase.Name, prop.Name, domainType.Name));
    //            }
    //            else
    //            {
    //                try
    //                {
    //                    Enum.Parse(prop.PropertyType, stateBase.AutoSetStatePropertyValue, false);
    //                }
    //                catch
    //                {
    //                    throw new BusinessLogicException(string.Format("State \"{0}\" Error. Can't parse \"{1}\" to Enum \"{2}\"", stateBase.Name, stateBase.AutoSetStatePropertyValue, prop.PropertyType.Name));
    //                }
    //            }

    //            var propertyName = t.Item1;
    //            var propertyValue = t.Item2;

    //            if (!string.IsNullOrWhiteSpace(newState.AutoSetStatePropertyName))
    //            {
    //                var domainObject = this.WorkflowObject.DomainObject;

    //                var prop = typeof(TDomainObject).GetProperty(newState.AutoSetStatePropertyName, true);

    //                var setValue = prop.PropertyType == typeof(string) ? newState.AutoSetStatePropertyValue
    //                                                                   : Enum.Parse(prop.PropertyType, newState.AutoSetStatePropertyValue, false);

    //                prop.SetValue(domainObject, setValue, null);

    //                this.MainContext.Logics.Implemented.Create<TDomainObject>().Save(domainObject);
    //            }
    //        });
    //    }
    //}
}
