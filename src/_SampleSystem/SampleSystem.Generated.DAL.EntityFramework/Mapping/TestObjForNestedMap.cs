using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Directories;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class TestObjForNestedMap : IEntityTypeConfiguration<TestObjForNested>
{
    public void Configure(EntityTypeBuilder<TestObjForNested> builder)
    {
        builder.ToTable("TestObjForNested");
    }
}
