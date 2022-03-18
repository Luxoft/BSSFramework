begin tran

ALTER TABLE [configuration].[UserAction] DROP CONSTRAINT [FK_UserAction_domainTypeId_DomainType]
ALTER TABLE [configuration].[UserActionObject] DROP CONSTRAINT [FK_UserActionObject_userActionId_UserAction]

DROP TABLE [configuration].UserAction
DROP TABLE [configuration].UserActionObject

commit tran