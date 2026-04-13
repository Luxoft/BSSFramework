using System.CodeDom;
using System.Reflection;

using CommonFramework;
using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Extensions;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.__Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner;

public class PropertyAssignerConfigurator<TConfiguration>(TConfiguration configuration) : PropertyAssignerConfiguratorBase<TConfiguration>(configuration)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    protected override CodeExpression GetPropertyHasAccessCondition(IServerPropertyAssigner propertyAssigner, PropertyInfo property, bool isEdit)
    {
        if (propertyAssigner == null) throw new ArgumentNullException(nameof(propertyAssigner));
        if (property == null) throw new ArgumentNullException(nameof(property));

        var attr = this.GetDomainObjectAttribute(propertyAssigner, property, isEdit);

        return this.Configuration.ToHasAccessMethod(propertyAssigner.ContextRef, attr.SecurityRule, propertyAssigner.DomainType!, propertyAssigner.DomainParameter);
    }

    private DomainObjectAccessAttribute GetDomainObjectAttribute(IServerPropertyAssigner propertyAssigner, PropertyInfo property, bool isEdit)
    {
        if (propertyAssigner == null) throw new ArgumentNullException(nameof(propertyAssigner));
        if (property == null) throw new ArgumentNullException(nameof(property));

        if (isEdit)
        {
            return this.Configuration.Environment.MetadataProxyProvider.Wrap(property).GetEditDomainObjectAttribute().FromMaybe(() => $"Edit operation for property \"{property.Name}\" in domainObject \"{propertyAssigner.DomainType.Name}\" not found");
        }
        else
        {
            return (this.Configuration.Environment.MetadataProxyProvider.Wrap(property).GetViewDomainObjectAttribute()
                 ?? this.Configuration.Environment.MetadataProxyProvider.Wrap(propertyAssigner.DomainType).GetViewDomainObjectAttribute())

                        .FromMaybe(() => $"View operation for property \"{property.Name}\" in domainObject \"{propertyAssigner.DomainType.Name}\" not found");
        }
    }
}
