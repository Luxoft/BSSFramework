using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeePhotoMap : SampleSystemBaseMap<EmployeePhoto>
{
    public override void Configure(EntityTypeBuilder<EmployeePhoto> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.ContentType).IsRequired();
        builder.Property(x => x.Data).HasColumnName("Photo").HasColumnType("image").IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.HasOne(x => x.Employee).WithMany(x => x.EmployeePhotos).HasForeignKey("employeeId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex("employeeId", nameof(EmployeePhoto.Type)).IsUnique().HasDatabaseName("UIX_employee_typeEmployeePhoto");
    }
}
