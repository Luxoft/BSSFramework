begin tran

DROP TABLE [configuration].[RegularJob]
DROP TABLE [configurationAudit].[RegularJobAudit]

commit tran