-------------------------
-- 'Location' workflow --
-------------------------

INSERT INTO [workflow].[TargetSystem] ([active],[createDate],[createdBy],[description],[id],[isBase],[isMain],[modifiedBy],[modifyDate],[name],[version])
VALUES (1,getdate(),null,null,'2D362091-7DAC-4BEC-A5AB-351B93B338D7',0,1,null,getdate(),'WorkflowSampleSystem',1)

INSERT INTO [workflow].[DomainType] ([active],[createDate],[createdBy],[description],[id],[modifiedBy],[modifyDate],[name],[nameSpace],[role],[targetSystemId],[version])
VALUES (1,getdate(),null,null,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715',null,getdate(),'Location','WorkflowSampleSystem.Domain',1,'2D362091-7DAC-4BEC-A5AB-351B93B338D7',1)

INSERT INTO [workflow].[Workflow] ([domainTypeId],[hasAttachments],[id],[activeConditionId],[autoRemoveWithDomainObject],[isValid],[active],[createDate],[createdBy],[description],[modifiedBy],[modifyDate],[name],[ownerId],[validationError],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715',0,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715',null,0,1,1,getdate(),null,null,null,getdate(),'Location workflow',null,null,1)

-- WorkflowSource.Elements
INSERT INTO [workflow].[WorkflowLambda] ([workflowId],[value],[name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715','context => obj => new [] { obj }','DefaultSource','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

INSERT INTO [workflow].[WorkflowSource] ([active],[createDate],[createdBy],[description],[elementsId],[id],[modifiedBy],[modifyDate],[name],[pathId],[typeId],[version],[workflowId])
VALUES (1,getdate(),null,null,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',null,getdate(),'Default',null,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715')

-- Security - 'SystemIntegration' operation
INSERT INTO [workflow].[Role] ([workflowId],[securityOperationId],[name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[customSecurityProviderId],[description],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715','0BA8A6B0-43B9-4F59-90CE-2FCBE37B97C9','Sstem integration','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,null,1)

-- start condition
INSERT INTO [workflow].[WorkflowLambda] ([workflowId],[value],[name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715','(p, c) => p == null','StartCondition','DACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

INSERT INTO [workflow].[StartWorkflowDomainObjectCondition] ([workflowId],[conditionId],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[factoryId],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715','DACA9DB4-9DA6-48AA-9FD3-A311016CB715','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

INSERT INTO [workflow].[WorkflowParameter] ([workflowId],[typeId],[allowNull],[name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',0,'DomainObject','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

-- state 'Begin' (id 'CACA9DB4-9DA6-48AA-9FD3-A311016CB715')
INSERT INTO [workflow].[State] ([isFinal],[id])
VALUES (0,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715')

INSERT INTO [workflow].[StateBase]([workflowId],[type],[isInitial],[autoSetStatePropertyName],[autoSetStatePropertyValue],[name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,1,null,null,'Begin','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

-- state 'End' (id 'DACA9DB4-9DA6-48AA-9FD3-A311016CB715')
INSERT INTO [workflow].[State] ([isFinal],[id])
VALUES (1,'DACA9DB4-9DA6-48AA-9FD3-A311016CB715')

INSERT INTO [workflow].[StateBase]([workflowId],[type],[isInitial],[autoSetStatePropertyName],[autoSetStatePropertyValue],[name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,0,null,null,'End','DACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

-- Task 'Act' from state 'Begin'
INSERT INTO [workflow].[Task] ([id],[active],[createDate],[createdBy],[description],[modifiedBy],[modifyDate],[name],[stateId],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,getdate(),null,null,null,getdate(),'Act','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1)

-- Command 'Act' from task 'Act'
INSERT INTO [workflow].[Command] ([taskId],[name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[executeActionId],[orderIndex],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715','Act','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,null,1,1)

INSERT INTO [workflow].[CommandRoleAccess] ([commandId],[roleId],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715','CACA9DB4-9DA6-48AA-9FD3-A311016CB715','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,1)

-- Event 'Act'
INSERT INTO [workflow].[Event] ([name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('Act','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

INSERT INTO [workflow].[CommandEvent] ([id],[ownerId])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715','CACA9DB4-9DA6-48AA-9FD3-A311016CB715')

-- Condition 'Always true' (id 'EACA9DB4-9DA6-48AA-9FD3-A311016CB715')
INSERT INTO [workflow].[WorkflowLambda] ([workflowId],[value],[name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715','(ctx, w) => true','AlwaysTrue','EACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

INSERT INTO [workflow].[ConditionState] ([conditionId],[id])
VALUES ('EACA9DB4-9DA6-48AA-9FD3-A311016CB715','EACA9DB4-9DA6-48AA-9FD3-A311016CB715')

INSERT INTO [workflow].[StateBase]([workflowId],[type],[isInitial],[autoSetStatePropertyName],[autoSetStatePropertyValue],[name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715',0,0,null,null,'Always true','EACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

INSERT INTO [workflow].[Event] ([name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('True','DACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

INSERT INTO [workflow].[ConditionStateEvent] ([id],[isTrue],[ownerId])
VALUES ('DACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,'EACA9DB4-9DA6-48AA-9FD3-A311016CB715')

INSERT INTO [workflow].[Event] ([name],[id],[active],[createdBy],[createDate],[modifyDate],[modifiedBy],[description],[version])
VALUES ('False','EACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,null,getdate(),getdate(),null,null,1)

INSERT INTO [workflow].[ConditionStateEvent] ([id],[isTrue],[ownerId])
VALUES ('EACA9DB4-9DA6-48AA-9FD3-A311016CB715',0,'EACA9DB4-9DA6-48AA-9FD3-A311016CB715')

-- Transition 'Act' from 'Begin' to 'Always true'
INSERT INTO [workflow].[Transition] ([id],[active],[createDate],[createdBy],[description],[fromId],[modifiedBy],[modifyDate],[name],[toId],[triggerEventId],[version],[workflowId])
VALUES ('CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,getdate(),null,null,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715',null,getdate(),'Act','EACA9DB4-9DA6-48AA-9FD3-A311016CB715','CACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715')

-- Transition 'True' from 'Always true' to 'End'
INSERT INTO [workflow].[Transition] ([id],[active],[createDate],[createdBy],[description],[fromId],[modifiedBy],[modifyDate],[name],[toId],[triggerEventId],[version],[workflowId])
VALUES ('DACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,getdate(),null,null,'EACA9DB4-9DA6-48AA-9FD3-A311016CB715',null,getdate(),'True','DACA9DB4-9DA6-48AA-9FD3-A311016CB715','DACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715')

-- Transition 'False' from 'Always true' to 'End'
INSERT INTO [workflow].[Transition] ([id],[active],[createDate],[createdBy],[description],[fromId],[modifiedBy],[modifyDate],[name],[toId],[triggerEventId],[version],[workflowId])
VALUES ('EACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,getdate(),null,null,'EACA9DB4-9DA6-48AA-9FD3-A311016CB715',null,getdate(),'False','DACA9DB4-9DA6-48AA-9FD3-A311016CB715','EACA9DB4-9DA6-48AA-9FD3-A311016CB715',1,'CACA9DB4-9DA6-48AA-9FD3-A311016CB715')
