using System.Reflection;

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
            foreach (var property in entityType.GetProperties().ToList())
            {
                if (property.PropertyInfo is { } propertyInfo && !propertyInfo.HasPrivateField(true))
                {
                    entityType.AddIgnored(property.Name);
                    entityType.RemoveProperty(property);
                }
            }

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
}
