DROP TABLE IF EXISTS [auth].[SubBusinessRoleLink]
DROP TABLE IF EXISTS [authAudit].[SubBusinessRoleLinkAudit]

DROP TABLE IF EXISTS [auth].[BusinessRoleOperationLink]
DROP TABLE IF EXISTS [authAudit].[BusinessRoleOperationLinkAudit]

DROP TABLE IF EXISTS [auth].[Operation]
DROP TABLE IF EXISTS [authAudit].[OperationAudit]

DROP TABLE IF EXISTS [auth].[PermissionFilterItem]
DROP TABLE IF EXISTS [authAudit].[PermissionFilterItemAudit]

DROP TABLE IF EXISTS [auth].[PermissionFilterEntity]
DROP TABLE IF EXISTS [authAudit].[PermissionFilterEntityAudit]

DROP TABLE IF EXISTS [auth].[EntityType]
DROP TABLE IF EXISTS [authAudit].[EntityTypeAudit]

ALTER TABLE [auth].[Permission] DROP COLUMN IF EXISTS [status]
ALTER TABLE [authAudit].[PermissionAudit] DROP COLUMN IF EXISTS [status]
ALTER TABLE [authAudit].[PermissionAudit] DROP COLUMN IF EXISTS [Status_MOD]
