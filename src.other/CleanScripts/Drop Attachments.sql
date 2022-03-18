begin tran

DROP TABLE [configuration].[AttachmentTag]
DROP TABLE [configuration].[Attachment]
DROP TABLE [configuration].[AttachmentContainer]

ALTER TABLE configuration.DomainType DROP COLUMN hasSecurityAttachment
	
commit tran