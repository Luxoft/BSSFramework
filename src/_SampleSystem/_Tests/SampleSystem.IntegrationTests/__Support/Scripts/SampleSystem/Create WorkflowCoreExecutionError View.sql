Create View [app].[WorkflowCoreExecutionError]
as
SELECT CONVERT(uniqueidentifier,0xefbeadde00000000 + CONVERT(binary(4), [PersistenceId])) as Id
      ,[ErrorTime]
      ,[Message]
      ,CAST(WorkflowId AS UNIQUEIDENTIFIER) as WorkflowInstanceId

FROM        WorkflowCore.wfc.ExecutionError