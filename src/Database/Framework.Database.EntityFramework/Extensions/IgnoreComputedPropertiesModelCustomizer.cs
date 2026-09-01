using Framework.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Extensions;

public class IgnoreComputedPropertiesModelCustomizer : IModelCustomizer
{
    public void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes().ToList())
        {
            this.IgnoreComputedProperties(entityType);

            foreach (var navigation in entityType.GetNavigations().ToList())
            {
                if (navigation.PropertyInfo is { } propertyInfo && !propertyInfo.HasPrivateField(true))
                {
                    entityType.AddIgnored(navigation.Name);

                    var foreignKey = navigation.ForeignKey;

                    foreignKey.DeclaringEntityType.RemoveForeignKey(foreignKey);
                }
            }
        }
    }

    private void IgnoreComputedProperties(IMutableTypeBase typeBase)
    {
        foreach (var property in typeBase.GetProperties().ToList())
        {
            if (property.PropertyInfo is { } propertyInfo && !propertyInfo.HasPrivateField(true))
            {
                typeBase.AddIgnored(property.Name);
                typeBase.RemoveProperty(property);
            }
        }

        foreach (var complexProperty in typeBase.GetComplexProperties().ToList())
        {
            this.IgnoreComputedProperties(complexProperty.ComplexType);
        }
    }
}
