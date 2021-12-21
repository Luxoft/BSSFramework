namespace Framework.Workflow.BLL
{
    public interface IExpressionParserFactory
    {
        StartWorkflowDomainObjectConditionLambdaProcessor<TDomainObject> GetByStartWorkflowDomainObjectCondition<TDomainObject>();

        StartWorkflowDomainObjectConditionFactoryLambdaProcessor<TBLLContext, TDomainObject, TStartupRootWorkflow>GetByStartWorkflowDomainObjectConditionFactory<TBLLContext, TDomainObject, TStartupRootWorkflow>();

        WorkflowActiveConditionLambdaProcessor<TBLLContext, TWorkflow> GetByWorkflowActiveCondition<TBLLContext, TWorkflow>();

        //WorkflowCurrentSourceNameLambdaProcessor<TBLLContext, TWorkflow> GetByWorkflowCurrentSourceName<TBLLContext, TWorkflow>();

        ConditionStateConditionLambdaProcessor<TBLLContext, TWorkflow> GetByConditionState<TBLLContext, TWorkflow>();

        StateTimeoutConditionLambdaProcessor<TBLLContext, TWorkflow> GetByStateTimeoutCondition<TBLLContext, TWorkflow>();

        StateDomainObjectConditionLambdaProcessor<TBLLContext, TWorkflow> GetByStateDomainObjectCondition<TBLLContext, TWorkflow>();

        ParallelStateFinalEventConditionLambdaProcessor<TBLLContext, TWorkflow> GetByParallelStateFinalEventCondition<TBLLContext, TWorkflow>();

        TransitionActionLambdaProcessor<TBLLContext, TWorkflow> GetByTransitionAction<TBLLContext, TWorkflow>();

        RoleCustomSecurityProviderLambdaProcessor<TBLLContext, TDomainObject> GetByRoleCustomSecurityProvider<TBLLContext, TDomainObject>();

        ParallelStateStartItemFactoryLambdaProcessor<TBLLContext, TWorkflow, TStartupSubWorkflow> GetByParallelStateStartItemFactory<TBLLContext, TWorkflow, TStartupSubWorkflow>();

        CommandExecuteActionLambdaProcessor<TBLLContext, TWorkflow, TCommand> GetByCommandExecuteAction<TBLLContext, TWorkflow, TCommand>();

        WorkflowSourceElementsLambdaProcessor<TBLLContext, TSourceDomainObject, TElementDomainObject> GetByWorkflowSourceElements<TBLLContext, TSourceDomainObject, TElementDomainObject>();

        WorkflowSourcePathLambdaProcessor<TBLLContext, TSourceDomainObject, TElementWorkflow> GetByWorkflowSourcePath<TBLLContext, TSourceDomainObject, TElementWorkflow>();
    }
}