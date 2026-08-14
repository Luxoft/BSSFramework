using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Authorization.Generated.DAL.EntityFramework.Mapping;

public class SecurityContextTypeMap : AuthBaseMap<SecurityContextType>
{
    public override void Configure(EntityTypeBuilder<SecurityContextType> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();
    }
}
