ALTER TABLE [auth].[BusinessRole] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[BusinessRoleAudit] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[BusinessRoleAudit] DROP COLUMN IF EXISTS [Active_MOD]

ALTER TABLE [auth].[Permission] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[PermissionAudit] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[PermissionAudit] DROP COLUMN IF EXISTS [Active_MOD]

ALTER TABLE [auth].[Permission] DROP COLUMN IF EXISTS [isDelegatedFrom]
ALTER TABLE [authAudit].[PermissionAudit] DROP COLUMN IF EXISTS [isDelegatedFrom]
ALTER TABLE [authAudit].[PermissionAudit] DROP COLUMN IF EXISTS [IsDelegatedFrom_MOD]

ALTER TABLE [auth].[Permission] DROP COLUMN IF EXISTS [isDelegatedTo]
ALTER TABLE [authAudit].[PermissionAudit] DROP COLUMN IF EXISTS [isDelegatedTo]
ALTER TABLE [authAudit].[PermissionAudit] DROP COLUMN IF EXISTS [IsDelegatedTo_MOD]

ALTER TABLE [auth].[PermissionRestriction] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[PermissionRestrictionAudit] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[PermissionRestrictionAudit] DROP COLUMN IF EXISTS [Active_MOD]

ALTER TABLE [auth].[Principal] DROP COLUMN IF EXISTS [externalId]
ALTER TABLE [authAudit].[PrincipalAudit] DROP COLUMN IF EXISTS [externalId]
ALTER TABLE [authAudit].[PrincipalAudit] DROP COLUMN IF EXISTS [ExternalId_MOD]

ALTER TABLE [auth].[Principal] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[PrincipalAudit] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[PrincipalAudit] DROP COLUMN IF EXISTS [Active_MOD]

ALTER TABLE [auth].[SecurityContextType] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[SecurityContextTypeAudit] DROP COLUMN IF EXISTS [active]
ALTER TABLE [authAudit].[SecurityContextTypeAudit] DROP COLUMN IF EXISTS [Active_MOD]
