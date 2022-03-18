--SELECT *
--FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
--where INFORMATION_SCHEMA.TABLE_CONSTRAINTS.TABLE_SCHEMA = 'workflow'
--  and INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_TYPE = 'FOREIGN KEY'

begin tran

ALTER TABLE [workflow].[ExecutedCommand] DROP CONSTRAINT [FK_ExecutedCommand_taskId_TaskInstance]
ALTER TABLE [workflow].[ExecutedCommand] DROP CONSTRAINT [FK_ExecutedCommand_definitionId_Command]
ALTER TABLE [workflow].[ExecutedCommandParameter] DROP CONSTRAINT [FK_ExecutedCommandParameter_commandId_ExecutedCommand]
ALTER TABLE [workflow].[ExecutedCommandParameter] DROP CONSTRAINT [FK_ExecutedCommandParameter_definitionId_CommandParameter]
ALTER TABLE [workflow].[WorkflowInstanceParameter] DROP CONSTRAINT [FK_WorkflowInstanceParameter_workflowInstanceId_WorkflowInstance]
ALTER TABLE [workflow].[WorkflowInstanceParameter] DROP CONSTRAINT [FK_WorkflowInstanceParameter_definitionId_WorkflowParameter]
ALTER TABLE [workflow].[StateInstance] DROP CONSTRAINT [FK_StateInstance_workflowId_WorkflowInstance]
ALTER TABLE [workflow].[StateInstance] DROP CONSTRAINT [FK_StateInstance_definitionId_StateBase]
ALTER TABLE [workflow].[TaskInstance] DROP CONSTRAINT [FK_TaskInstance_stateId_StateInstance]
ALTER TABLE [workflow].[TaskInstance] DROP CONSTRAINT [FK_TaskInstance_definitionId_Task]
ALTER TABLE [workflow].[TransitionInstance] DROP CONSTRAINT [FK_TransitionInstance_workflowInstanceId_WorkflowInstance]
ALTER TABLE [workflow].[TransitionInstance] DROP CONSTRAINT [FK_TransitionInstance_fromId_StateInstance]
ALTER TABLE [workflow].[TransitionInstance] DROP CONSTRAINT [FK_TransitionInstance_toId_StateInstance]
ALTER TABLE [workflow].[TransitionInstance] DROP CONSTRAINT [FK_TransitionInstance_definitionId_Transition]
ALTER TABLE [workflow].[WorkflowInstance] DROP CONSTRAINT [FK_WorkflowInstance_ownerStateId_StateInstance]
ALTER TABLE [workflow].[WorkflowInstance] DROP CONSTRAINT [FK_WorkflowInstance_definitionId_Workflow]
ALTER TABLE [workflow].[WorkflowInstance] DROP CONSTRAINT [FK_WorkflowInstance_currentStateId_StateInstance]
ALTER TABLE [workflow].[WorkflowInstanceWatcher] DROP CONSTRAINT [FK_WorkflowInstanceWatcher_workflowInstanceId_WorkflowInstance]
ALTER TABLE [workflow].[Command] DROP CONSTRAINT [FK_Command_taskId_Task]
ALTER TABLE [workflow].[Command] DROP CONSTRAINT [FK_Command_executeActionId_WorkflowLambda]
ALTER TABLE [workflow].[CommandRoleAccess] DROP CONSTRAINT [FK_CommandRoleAccess_commandId_Command]
ALTER TABLE [workflow].[CommandRoleAccess] DROP CONSTRAINT [FK_CommandRoleAccess_roleId_Role]
ALTER TABLE [workflow].[CommandMetadata] DROP CONSTRAINT [FK_CommandMetadata_commandId_Command]
ALTER TABLE [workflow].[StartWorkflowDomainObjectCondition] DROP CONSTRAINT [FK_StartWorkflowDomainObjectCondition_workflowId_Workflow]
ALTER TABLE [workflow].[StartWorkflowDomainObjectCondition] DROP CONSTRAINT [FK_StartWorkflowDomainObjectCondition_conditionId_WorkflowLambda]
ALTER TABLE [workflow].[StartWorkflowDomainObjectCondition] DROP CONSTRAINT [FK_StartWorkflowDomainObjectCondition_factoryId_WorkflowLambda]
ALTER TABLE [workflow].[WorkflowLambda] DROP CONSTRAINT [FK_WorkflowLambda_workflowId_Workflow]
ALTER TABLE [workflow].[Role] DROP CONSTRAINT [FK_Role_workflowId_Workflow]
ALTER TABLE [workflow].[Role] DROP CONSTRAINT [FK_Role_customSecurityProviderId_WorkflowLambda]
ALTER TABLE [workflow].[ParallelStateStartItem] DROP CONSTRAINT [FK_ParallelStateStartItem_stateId_ParallelState]
ALTER TABLE [workflow].[ParallelStateStartItem] DROP CONSTRAINT [FK_ParallelStateStartItem_subWorkflowId_Workflow]
ALTER TABLE [workflow].[ParallelStateStartItem] DROP CONSTRAINT [FK_ParallelStateStartItem_factoryId_WorkflowLambda]
ALTER TABLE [workflow].[StateBase] DROP CONSTRAINT [FK_StateBase_workflowId_Workflow]
ALTER TABLE [workflow].[ConditionState] DROP CONSTRAINT [FK_ConditionState_conditionId_WorkflowLambda]
ALTER TABLE [workflow].[ConditionState] DROP CONSTRAINT [FK_ConditionState_StateBase]
ALTER TABLE [workflow].[ParallelState] DROP CONSTRAINT [FK_ParallelState_StateBase]
ALTER TABLE [workflow].[State] DROP CONSTRAINT [FK_State_StateBase]
ALTER TABLE [workflow].[CommandParameter] DROP CONSTRAINT [FK_CommandParameter_commandId_Command]
ALTER TABLE [workflow].[CommandParameter] DROP CONSTRAINT [FK_CommandParameter_typeId_DomainType]
ALTER TABLE [workflow].[WorkflowParameter] DROP CONSTRAINT [FK_WorkflowParameter_workflowId_Workflow]
ALTER TABLE [workflow].[WorkflowParameter] DROP CONSTRAINT [FK_WorkflowParameter_typeId_DomainType]
ALTER TABLE [workflow].[DomainType] DROP CONSTRAINT [FK_DomainType_targetSystemId_TargetSystem]
ALTER TABLE [workflow].[Task] DROP CONSTRAINT [FK_Task_stateId_State]
ALTER TABLE [workflow].[TaskMetadata] DROP CONSTRAINT [FK_TaskMetadata_taskId_Task]
ALTER TABLE [workflow].[Transition] DROP CONSTRAINT [FK_Transition_workflowId_Workflow]
ALTER TABLE [workflow].[Transition] DROP CONSTRAINT [FK_Transition_fromId_StateBase]
ALTER TABLE [workflow].[Transition] DROP CONSTRAINT [FK_Transition_toId_StateBase]
ALTER TABLE [workflow].[Transition] DROP CONSTRAINT [FK_Transition_triggerEventId_Event]
ALTER TABLE [workflow].[TransitionAction] DROP CONSTRAINT [FK_TransitionAction_transitionId_Transition]
ALTER TABLE [workflow].[TransitionAction] DROP CONSTRAINT [FK_TransitionAction_actionId_WorkflowLambda]
ALTER TABLE [workflow].[Workflow] DROP CONSTRAINT [FK_Workflow_ownerId_Workflow]
ALTER TABLE [workflow].[Workflow] DROP CONSTRAINT [FK_Workflow_domainTypeId_DomainType]
ALTER TABLE [workflow].[Workflow] DROP CONSTRAINT [FK_Workflow_activeConditionId_WorkflowLambda]
ALTER TABLE [workflow].[WorkflowSource] DROP CONSTRAINT [FK_WorkflowSource_workflowId_Workflow]
ALTER TABLE [workflow].[WorkflowSource] DROP CONSTRAINT [FK_WorkflowSource_typeId_DomainType]
ALTER TABLE [workflow].[WorkflowSource] DROP CONSTRAINT [FK_WorkflowSource_elementsId_WorkflowLambda]
ALTER TABLE [workflow].[WorkflowSource] DROP CONSTRAINT [FK_WorkflowSource_pathId_WorkflowLambda]
ALTER TABLE [workflow].[WorkflowMetadata] DROP CONSTRAINT [FK_WorkflowMetadata_workflowId_Workflow]

DROP TABLE [workflow].Command
DROP TABLE [workflow].CommandEvent
DROP TABLE [workflow].CommandMetadata
DROP TABLE [workflow].CommandParameter
DROP TABLE [workflow].CommandRoleAccess
DROP TABLE [workflow].ConditionState
DROP TABLE [workflow].ConditionStateEvent
DROP TABLE [workflow].DomainType
DROP TABLE [workflow].Event
DROP TABLE [workflow].ExecutedCommand
DROP TABLE [workflow].ExecutedCommandParameter
DROP TABLE [workflow].ExecutedScripts
DROP TABLE [workflow].NamedLock
DROP TABLE [workflow].ParallelState
DROP TABLE [workflow].ParallelStateFinalEvent
DROP TABLE [workflow].ParallelStateStartItem
DROP TABLE [workflow].Role
DROP TABLE [workflow].StartWorkflowDomainObjectCondition
DROP TABLE [workflow].State
DROP TABLE [workflow].StateBase
DROP TABLE [workflow].StateDomainObjectEvent
DROP TABLE [workflow].StateInstance
DROP TABLE [workflow].StateTimeoutEvent
DROP TABLE [workflow].TargetSystem
DROP TABLE [workflow].Task
DROP TABLE [workflow].TaskInstance
DROP TABLE [workflow].TaskMetadata
DROP TABLE [workflow].Transition
DROP TABLE [workflow].TransitionAction
DROP TABLE [workflow].TransitionInstance
DROP TABLE [workflow].Workflow
DROP TABLE [workflow].WorkflowInstance
DROP TABLE [workflow].WorkflowInstanceParameter
DROP TABLE [workflow].WorkflowInstanceWatcher
DROP TABLE [workflow].WorkflowLambda
DROP TABLE [workflow].WorkflowMetadata
DROP TABLE [workflow].WorkflowParameter
DROP TABLE [workflow].WorkflowSource

commit tran