using Framework.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.Extensions;

public class IgnoreComputedPropertiesModelCustomizer(ModelCustomizerDependencies dependencies) : ModelCustomizer(dependencies)
{
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties().ToList())
            {
                if (property.PropertyInfo is { } propertyInfo && !propertyInfo.HasPrivateField(true))
                {
                    entityType.RemoveProperty(property);
                }
            }
        }
    }
}
