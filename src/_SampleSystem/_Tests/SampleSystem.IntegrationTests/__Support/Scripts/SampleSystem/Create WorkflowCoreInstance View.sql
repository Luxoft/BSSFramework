Create View [app].[WorkflowCoreInstance]
as
SELECT     Data, CAST(InstanceId AS UNIQUEIDENTIFIER) AS Id, WorkflowDefinitionId, Status
FROM        WorkflowCore.wfc.Workflow