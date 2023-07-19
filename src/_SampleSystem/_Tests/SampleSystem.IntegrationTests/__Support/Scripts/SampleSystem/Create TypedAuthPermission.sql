CREATE VIEW [app].[TypedAuthPermissionBusinessUnit]
AS
SELECT   pfi.id, pfi.permissionId, pfi.contextEntityId
FROM     [auth].[PermissionFilterItem] pfi
JOIN     [auth].[EntityType] et on pfi.entityTypeId = et.id
where et.name = 'BusinessUnit'

GO

CREATE VIEW [app].[TypedAuthPermissionManagementUnit]
AS
SELECT   pfi.id, pfi.permissionId, pfi.contextEntityId
FROM     [auth].[PermissionFilterItem] pfi
JOIN     [auth].[EntityType] et on pfi.entityTypeId = et.id
where et.name = 'ManagementUnit'

GO

CREATE VIEW [app].[TypedAuthPermissionLocation]
AS
SELECT   pfi.id, pfi.permissionId, pfi.contextEntityId
FROM     [auth].[PermissionFilterItem] pfi
JOIN     [auth].[EntityType] et on pfi.entityTypeId = et.id
where et.name = 'Location'

GO

CREATE VIEW [app].[TypedAuthPermissionEmployee]
AS
SELECT   pfi.id, pfi.permissionId, pfi.contextEntityId
FROM     [auth].[PermissionFilterItem] pfi
JOIN     [auth].[EntityType] et on pfi.entityTypeId = et.id
where et.name = 'Employee'

GO
