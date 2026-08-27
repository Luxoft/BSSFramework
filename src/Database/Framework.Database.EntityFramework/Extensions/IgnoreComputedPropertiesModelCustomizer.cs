using System.Reflection;

using Framework.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.Extensions;

public class IgnoreComputedPropertiesModelCustomizer : IModelCustomizer
{
    public void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties().ToList())
            {
                if (property.PropertyInfo is { } propertyInfo && !propertyInfo.HasPrivateField(true))
                {
                    entityType.AddIgnored(property.Name);
                    entityType.RemoveProperty(property);
                }
            }
        }
    }
}
