Create View [app].[WorkflowCoreInstance]
as
SELECT     Data, CAST(InstanceId AS UNIQUEIDENTIFIER) AS Id, WorkflowDefinitionId
FROM        WorkflowCore.wfc.Workflow