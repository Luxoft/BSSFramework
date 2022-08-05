--RunAlways = false
--FileVersion = 1.0
--Scheme = TRMSys
--ApplyMode = PostAddOrUpdate

use [$Database]
go

begin tran
------------------------------------------------------------------------------------------
declare @now datetime = getdate(),
        @author nvarchar(max) = 'denormalize-auth'

INSERT INTO [appAudit].[AuditRevisionEntity]
           ([Author]
           ,[RevisionDate])
     VALUES
           (@author,
		    @now)

declare @newRevision bigint = @@IDENTITY
------------------------------------------------------------------------------------------
update pfi
set contextEntityId = pfe.entityId,
    entityTypeId = pfe.entityTypeId
from [auth].[PermissionFilterItem] pfi
join auth.PermissionFilterEntity pfe on pfe.id = pfi.entityId

------------------------------------------------------------------------------------------
INSERT INTO [authAudit].[PermissionFilterItemAudit]
           ([active]
           ,[Active_MOD]
           ,[createDate]
           ,[CreateDate_MOD]
           ,[createdBy]
           ,[CreatedBy_MOD]
           ,[modifiedBy]
           ,[ModifiedBy_MOD]
           ,[modifyDate]
           ,[ModifyDate_MOD]
           ,[entityId]
           ,[Entity_MOD]
           ,[entityTypeId]
           ,[EntityType_MOD]
           ,[permissionId]
           ,[Permission_MOD]
           ,[id]
           ,[REV]
           ,[REVTYPE]
           ,[contextEntityId]
           ,[ContextEntityId_MOD])
  select active
           ,0
           ,createDate
           ,0
           ,createdBy
           ,0
           ,modifiedBy
           ,0
           ,modifyDate
           ,0
           ,entityId
           ,0
           ,entityTypeId
           ,1
           ,permissionId
           ,0
           ,id
           ,@newRevision
           ,1
           ,contextEntityId
           ,1
  from auth.PermissionFilterItem
GO
------------------------------------------------------------------------------------------
commit tran
