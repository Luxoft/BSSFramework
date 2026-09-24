using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ExtendedInlineAuditObjMap : SampleSystemBaseMap<ExtendedInlineAuditObj>
{
    public override void Configure(EntityTypeBuilder<ExtendedInlineAuditObj> builder)
    {
        base.Configure(builder);

        builder.Property<System.Guid?>("createdByEmployeeId").HasColumnName("createdByEmployeeId");
        builder.HasOne(x => x.CreatedByEmployee).WithMany().HasForeignKey("createdByEmployeeId")
               .HasConstraintName("FK_ExtendedInlineAuditObj_createdByEmployeeId_Employee").OnDelete(DeleteBehavior.Restrict);

        builder.Property<System.Guid?>("modifiedByEmployeeId").HasColumnName("modifiedByEmployeeId");
        builder.HasOne(x => x.ModifiedByEmployee).WithMany().HasForeignKey("modifiedByEmployeeId")
               .HasConstraintName("FK_ExtendedInlineAuditObj_modifiedByEmployeeId_Employee").OnDelete(DeleteBehavior.Restrict);

    }
}
