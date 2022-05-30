--delete FROM [auth].[DenormalizedPermissionItem] 

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
insert into [auth].[DenormalizedPermissionItem] (id, permissionId, entityId, entityTypeId, active, createdBy, createDate)
select newid(), p.id, pfe.entityId, pfe.entityTypeId, 1, @author, @now
from auth.permission p
join auth.PermissionFilterItem pfi on pfi.permissionId = p.id
join auth.PermissionFilterEntity pfe on pfe.id = pfi.entityId
------------------------------------------------------------------------------------------
insert into [auth].[DenormalizedPermissionItem] (id, permissionId, entityId, entityTypeId, active, createdBy, createDate)
select newid(), p.id, '77777777-7777-7777-7777-777777777777', et.id, 1, @author, @now
from auth.EntityType et
join auth.permission p on 1 = 1
where et.id not in (select pfe.entityTypeId
                    from auth.PermissionFilterItem pfi
					join auth.PermissionFilterEntity pfe on pfe.id = pfi.entityId
					where pfi.permissionId = p.id)
------------------------------------------------------------------------------------------
INSERT INTO [authAudit].[DenormalizedPermissionItemAudit]
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
           ,[entityTypeId]
           ,[EntityType_MOD]
           ,[permissionId]
           ,[Permission_MOD]
           ,[id]
           ,[REV]
           ,[REVTYPE]
           ,[entityId]
           ,[EntityId_MOD])
     select active,
            1,
            createDate,
            1,
            createdBy,
            1,
            modifiedBy,
            1,
            modifyDate,
            1,
            entityTypeId,
            1,
            permissionId,
            1,
            id,
            @newRevision,
            0,
            entityId,
            1
	from auth.DenormalizedPermissionItem
GO
------------------------------------------------------------------------------------------
commit tran