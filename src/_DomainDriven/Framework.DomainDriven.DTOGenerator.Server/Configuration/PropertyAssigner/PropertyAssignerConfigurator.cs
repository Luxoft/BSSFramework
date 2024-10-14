using System.CodeDom;
using System.Reflection;

using Framework.Core;
using Framework.Security;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class PropertyAssignerConfigurator<TConfiguration> : PropertyAssignerConfiguratorBase<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public PropertyAssignerConfigurator(TConfiguration configuration)
            : base(configuration)
    {
    }


    protected override CodeExpression GetPropertyHasAccessCondition(IServerPropertyAssigner propertyAssigner, PropertyInfo property, bool isEdit)
    {
        if (propertyAssigner == null) throw new ArgumentNullException(nameof(propertyAssigner));
        if (property == null) throw new ArgumentNullException(nameof(property));

        var attr = this.GetDomainObjectAttribute(propertyAssigner, property, isEdit);

        return this.Configuration.ToHasAccessMethod(propertyAssigner.ContextRef, attr.SecurityRule, propertyAssigner.DomainType, propertyAssigner.DomainParameter);
    }

    private DomainObjectAccessAttribute GetDomainObjectAttribute(IServerPropertyAssigner propertyAssigner, PropertyInfo property, bool isEdit)
    {
        if (propertyAssigner == null) throw new ArgumentNullException(nameof(propertyAssigner));
        if (property == null) throw new ArgumentNullException(nameof(property));

        if (isEdit)
        {
            return this.Configuration.Environment.ExtendedMetadata.GetProperty(property).GetEditDomainObjectAttribute().FromMaybe(() => $"Edit operation for property \"{property.Name}\" in domainObject \"{propertyAssigner.DomainType.Name}\" not found");
        }
        else
        {
            return (this.Configuration.Environment.ExtendedMetadata.GetProperty(property).GetViewDomainObjectAttribute()
                 ?? this.Configuration.Environment.ExtendedMetadata.GetType(propertyAssigner.DomainType).GetViewDomainObjectAttribute())

                        .FromMaybe(() => $"View operation for property \"{property.Name}\" in domainObject \"{propertyAssigner.DomainType.Name}\" not found");
        }
    }
}
