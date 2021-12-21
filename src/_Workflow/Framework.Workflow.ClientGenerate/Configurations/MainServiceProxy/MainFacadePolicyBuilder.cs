using System;

using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;
using Framework.Workflow.WebApi;

namespace Framework.Workflow.ClientGenerate
{
    public class MainFacadePolicyBuilder : TypeScriptMethodPolicyBuilder<WorkflowSLJsonController>
    {
        public MainFacadePolicyBuilder()
        {
            this.AddCommandMethods();
            this.AddConditionStateMethods();
            this.AddParallelStateMethods();
            this.AddStateMethods();
            this.AddTaskMethods();
            this.AddTransitionMethods();
            this.AddWorkflowLambdaMethods();
            this.AddStartWorkflowDomainObjectConditionMethods();
            this.AddWorkflowSourceMethods();
            this.AddWorkflowMethods();

            this.AddWorkflowInstanceMethods();
            this.AddTaskInstanceMethods();
            this.AddStateInstanceMethods();

            this.AddUnsortedMethods();
        }

        private void AddCommandMethods()
        {
            this.Add(facade => facade.GetFullCommandsByIdents(default));

            this.Add(facade => facade.GetFullCommandsByRootFilter(default));
            this.Add(facade => facade.GetSimpleCommandsByRootFilter(default));
            this.Add(facade => facade.GetFullCommandsByRootFilter(default));

            this.Add(facade => facade.GetRichCommand(default));
            this.Add(facade => facade.SaveCommand(default));
            this.Add(facade => facade.RemoveCommand(default));
        }

        private void AddConditionStateMethods()
        {
            this.Add(facade => facade.GetFullConditionStatesByIdents(default));

            this.Add(facade => facade.GetFullConditionStatesByRootFilter(default));
            this.Add(facade => facade.GetSimpleConditionStatesByRootFilter(default));
            this.Add(facade => facade.GetFullConditionStatesByRootFilter(default));

            this.Add(facade => facade.GetRichConditionState(default));
            this.Add(facade => facade.SaveConditionState(default));
            this.Add(facade => facade.RemoveConditionState(default));
        }

        private void AddParallelStateMethods()
        {
            this.Add(facade => facade.GetFullParallelStatesByIdents(default));

            this.Add(facade => facade.GetFullParallelStatesByRootFilter(default));
            this.Add(facade => facade.GetSimpleParallelStatesByRootFilter(default));
            this.Add(facade => facade.GetFullParallelStatesByRootFilter(default));

            this.Add(facade => facade.GetRichParallelState(default));
            this.Add(facade => facade.SaveParallelState(default));
            this.Add(facade => facade.RemoveParallelState(default));
        }

        private void AddStateMethods()
        {
            this.Add(facade => facade.GetFullStatesByIdents(default));

            this.Add(facade => facade.GetFullStatesByRootFilter(default));
            this.Add(facade => facade.GetSimpleStatesByRootFilter(default));
            this.Add(facade => facade.GetFullStatesByRootFilter(default));

            this.Add(facade => facade.GetRichState(default));
            this.Add(facade => facade.SaveState(default));
            this.Add(facade => facade.RemoveState(default));
        }

        private void AddTaskMethods()
        {
            this.Add(facade => facade.GetFullTasksByIdents(default));

            this.Add(facade => facade.GetFullTasksByRootFilter(default));
            this.Add(facade => facade.GetSimpleTasksByRootFilter(default));
            this.Add(facade => facade.GetFullTasksByRootFilter(default));

            this.Add(facade => facade.GetRichTask(default));
            this.Add(facade => facade.SaveTask(default));
            this.Add(facade => facade.RemoveTask(default));
        }

        private void AddTransitionMethods()
        {
            this.Add(facade => facade.GetFullTransitionsByIdents(default));

            this.Add(facade => facade.GetFullTransitionsByRootFilter(default));
            this.Add(facade => facade.GetSimpleTransitionsByRootFilter(default));
            this.Add(facade => facade.GetFullTransitionsByRootFilter(default));

            this.Add(facade => facade.GetRichTransition(default));
            this.Add(facade => facade.SaveTransition(default));
            this.Add(facade => facade.RemoveTransition(default));
        }

        private void AddWorkflowLambdaMethods()
        {
            this.Add(facade => facade.GetFullWorkflowLambdasByIdents(default));

            this.Add(facade => facade.GetFullWorkflowLambdasByRootFilter(default));
            this.Add(facade => facade.GetSimpleWorkflowLambdasByRootFilter(default));
            this.Add(facade => facade.GetFullWorkflowLambdasByRootFilter(default));

            this.Add(facade => facade.GetRichWorkflowLambda(default));
            this.Add(facade => facade.SaveWorkflowLambda(default));
            this.Add(facade => facade.RemoveWorkflowLambda(default));
        }

        private void AddStartWorkflowDomainObjectConditionMethods()
        {
            this.Add(facade => facade.GetFullStartWorkflowDomainObjectConditionsByIdents(default));

            this.Add(facade => facade.GetFullStartWorkflowDomainObjectConditionsByRootFilter(default));
            this.Add(facade => facade.GetSimpleStartWorkflowDomainObjectConditionsByRootFilter(default));
            this.Add(facade => facade.GetFullStartWorkflowDomainObjectConditionsByRootFilter(default));

            this.Add(facade => facade.GetRichStartWorkflowDomainObjectCondition(default));
            this.Add(facade => facade.SaveStartWorkflowDomainObjectCondition(default));
            this.Add(facade => facade.RemoveStartWorkflowDomainObjectCondition(default));
        }

        private void AddWorkflowSourceMethods()
        {
            this.Add(facade => facade.GetFullWorkflowSourcesByIdents(default));

            this.Add(facade => facade.GetFullWorkflowSourcesByRootFilter(default));
            this.Add(facade => facade.GetFullWorkflowSourcesByRootFilter(default));

            this.Add(facade => facade.GetRichWorkflowSource(default));
        }

        private void AddWorkflowMethods()
        {
            this.Add(facade => facade.GetFullWorkflowsByIdents(default));

            this.Add(facade => facade.CreateWorkflow(default));

            this.Add(facade => facade.GetFullWorkflowsByRootFilter(default));
            this.Add(facade => facade.GetSimpleWorkflowsByRootFilter(default));
            this.Add(facade => facade.GetFullWorkflowsByRootFilter(default));

            this.Add(facade => facade.GetRichWorkflow(default));
            this.Add(facade => facade.SaveWorkflow(default));
            this.Add(facade => facade.RemoveWorkflow(default));
        }

        private void AddWorkflowInstanceMethods()
        {
            this.Add(facade => facade.GetFullWorkflowInstancesByIdents(default));

            this.Add(facade => facade.GetFullWorkflowInstancesByRootFilter(default));

            this.Add(facade => facade.GetRichWorkflowInstance(default));
            this.Add(facade => facade.SaveWorkflowInstance(default));
            this.Add(facade => facade.RemoveWorkflowInstance(default));
        }

        private void AddTaskInstanceMethods()
        {
            this.Add(facade => facade.GetFullTaskInstancesByIdents(default));

            this.Add(facade => facade.GetFullTaskInstancesByRootFilter(default));

            this.Add(facade => facade.GetRichTaskInstance(default));
            this.Add(facade => facade.SaveTaskInstance(default));
        }

        private void AddStateInstanceMethods()
        {
            this.Add(facade => facade.GetFullStateInstancesByIdents(default));
        }


        private void AddUnsortedMethods()
        {
            this.Add(facade => facade.GetSimpleDomainTypesByRootFilter(default));

            this.Add(facade => facade.GetSimpleEventsByRootFilter(default));
            this.Add(facade => facade.GetSimpleCommandEventsByRootFilter(default));
            this.Add(facade => facade.GetSimpleStateBasesByRootFilter(default));
            this.Add(facade => facade.GetSimpleRolesByRootFilter(default));

            this.Add(facade => facade.GetMiniAvailableTaskInstancesByMainFilter(default));

            this.Add(facade => facade.GetSimpleCommandsByAvailableCommandFilter(default));

            this.Add(facade => facade.StartWorkflow(default));
            this.Add(facade => facade.ExecuteCommand(default));
            this.Add(facade => facade.ExecuteCommands(default));

            this.Add(facade => facade.GetSimpleDomainTypes());
            this.Add(facade => facade.GetSimpleEvents());
            this.Add(facade => facade.GetSimpleStateBases());
            this.Add(facade => facade.GetSimpleTargetSystems());
            this.Add(facade => facade.GetSimpleTransitions());
            this.Add(facade => facade.GetSimpleWorkflowLambdas());
            this.Add(facade => facade.GetSimpleWorkflows());

            this.Add(facade => facade.GetRichRolesByIdents(default));
            this.Add(facade => facade.GetSimpleStateBase(default));
            this.Add(facade => facade.GetSimpleStateBasesByIdents(default));

            this.Add(facade => facade.AbortWorkflow(default));

            this.Add(facade => facade.DrawWorkflow(default));

            this.Add(facade => facade.DrawWorkflowInstance(default));

            this.Add(facade => facade.GetFullTask(default));
        }
    }
}
