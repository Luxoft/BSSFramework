using Framework.ExpressionParsers;

namespace Framework.Workflow.BLL
{
    public class ExpressionParserFactory : DynamicExpressionParserFactory, IExpressionParserFactory
    {
        public ExpressionParserFactory(INativeExpressionParser parser)
            : base(parser)
        {

        }


        public StartWorkflowDomainObjectConditionLambdaProcessor<TDomainObject> GetByStartWorkflowDomainObjectCondition<TDomainObject>()
        {
            return this.GetValue(() => new StartWorkflowDomainObjectConditionLambdaProcessor<TDomainObject>(this.Parser));
        }

        public StartWorkflowDomainObjectConditionFactoryLambdaProcessor<TBLLContext, TDomainObject, TStartupRootWorkflow>GetByStartWorkflowDomainObjectConditionFactory<TBLLContext, TDomainObject, TStartupRootWorkflow>()
        {
            return this.GetValue(() => new StartWorkflowDomainObjectConditionFactoryLambdaProcessor<TBLLContext, TDomainObject, TStartupRootWorkflow>(this.Parser));
        }

        public WorkflowActiveConditionLambdaProcessor<TBLLContext, TWorkflow> GetByWorkflowActiveCondition<TBLLContext, TWorkflow>()
        {
            return this.GetValue(() => new WorkflowActiveConditionLambdaProcessor<TBLLContext, TWorkflow>(this.Parser));
        }

        public ConditionStateConditionLambdaProcessor<TBLLContext, TWorkflow> GetByConditionState<TBLLContext, TWorkflow>()
        {
            return this.GetValue(() => new ConditionStateConditionLambdaProcessor<TBLLContext, TWorkflow>(this.Parser));
        }

        public StateTimeoutConditionLambdaProcessor<TBLLContext, TWorkflow> GetByStateTimeoutCondition<TBLLContext, TWorkflow>()
        {
            return this.GetValue(() => new StateTimeoutConditionLambdaProcessor<TBLLContext, TWorkflow>(this.Parser));
        }

        public StateDomainObjectConditionLambdaProcessor<TBLLContext, TWorkflow> GetByStateDomainObjectCondition<TBLLContext, TWorkflow>()
        {
            return this.GetValue(() => new StateDomainObjectConditionLambdaProcessor<TBLLContext, TWorkflow>(this.Parser));
        }

        public ParallelStateFinalEventConditionLambdaProcessor<TBLLContext, TWorkflow> GetByParallelStateFinalEventCondition<TBLLContext, TWorkflow>()
        {
            return this.GetValue(() => new ParallelStateFinalEventConditionLambdaProcessor<TBLLContext, TWorkflow>(this.Parser));
        }

        public TransitionActionLambdaProcessor<TBLLContext, TWorkflow> GetByTransitionAction<TBLLContext, TWorkflow>()
        {
            return this.GetValue(() => new TransitionActionLambdaProcessor<TBLLContext, TWorkflow>(this.Parser));
        }

        public RoleCustomSecurityProviderLambdaProcessor<TBLLContext, TDomainObject> GetByRoleCustomSecurityProvider<TBLLContext, TDomainObject>()
        {
            return this.GetValue(() => new RoleCustomSecurityProviderLambdaProcessor<TBLLContext, TDomainObject>(this.Parser));
        }

        public ParallelStateStartItemFactoryLambdaProcessor<TBLLContext, TMainWorkflow, TCreatedSubWorkflow> GetByParallelStateStartItemFactory<TBLLContext, TMainWorkflow, TCreatedSubWorkflow>()
        {
            return this.GetValue(() => new ParallelStateStartItemFactoryLambdaProcessor<TBLLContext, TMainWorkflow, TCreatedSubWorkflow>(this.Parser));
        }

        public CommandExecuteActionLambdaProcessor<TBLLContext, TWorkflow, TCommand> GetByCommandExecuteAction<TBLLContext, TWorkflow, TCommand>()
        {
            return this.GetValue(() => new CommandExecuteActionLambdaProcessor<TBLLContext, TWorkflow, TCommand>(this.Parser));
        }

        public WorkflowSourceElementsLambdaProcessor<TBLLContext, TSourceDomainObject, TElementDomainObject> GetByWorkflowSourceElements<TBLLContext, TSourceDomainObject, TElementDomainObject>()
        {
            return this.GetValue(() => new WorkflowSourceElementsLambdaProcessor<TBLLContext, TSourceDomainObject, TElementDomainObject>(this.Parser));
        }

        public WorkflowSourcePathLambdaProcessor<TBLLContext, TWorkflow, TElementWorkflow> GetByWorkflowSourcePath<TBLLContext, TWorkflow, TElementWorkflow>()
        {
            return this.GetValue(() => new WorkflowSourcePathLambdaProcessor<TBLLContext, TWorkflow, TElementWorkflow>(this.Parser));
        }
    }
}