using System.CodeDom;
using System.Reflection;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.Security.DomainObjectToSecurity;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.Security.ExpandMaybe;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.Security.SecurityToDomainObject.Strict;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.Security.SecurityToDomainObject.Update;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner;

public abstract class PropertyAssignerConfiguratorBase<TConfiguration>(TConfiguration configuration)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), IPropertyAssignerConfigurator
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    protected abstract CodeExpression GetPropertyHasAccessCondition(IServerPropertyAssigner propertyAssigner, PropertyInfo property, bool isEdit);


    public virtual IPropertyAssigner GetStrictSecurityToDomainObjectPropertyAssigner(IPropertyAssigner innerAssigner)
    {
        if (innerAssigner == null) throw new ArgumentNullException(nameof(innerAssigner));

        if (this.Configuration.ExpandStrictMaybeToDefault)
        {
            return new ExpandMaybeSecurityToDomainObjectPropertyAssigner(innerAssigner.WithConfiguration(this.Configuration), this);
        }
        else
        {
            return new StrictToDomainObjectPropertyAssigner(innerAssigner.WithConfiguration(this.Configuration), this);
        }
    }

    public virtual IPropertyAssigner GetUpdateSecurityToDomainObjectPropertyAssigner(IPropertyAssigner innerAssigner)
    {
        if (innerAssigner == null) throw new ArgumentNullException(nameof(innerAssigner));

        return new UpdateToDomainObjectPropertyAssigner(innerAssigner.WithConfiguration(this.Configuration), this);
    }

    public virtual IPropertyAssigner GetDomainObjectToSecurityPropertyAssigner(IPropertyAssigner innerAssigner)
    {
        if (innerAssigner == null) throw new ArgumentNullException(nameof(innerAssigner));

        return new DomainObjectToSecurityPropertyAssigner(innerAssigner.WithConfiguration(this.Configuration), this);
    }


    private class DomainObjectToSecurityPropertyAssigner(
        IPropertyAssigner<TConfiguration> innerAssigner,
        PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
        : DomainObjectToSecurityPropertyAssignerBase<TConfiguration>(innerAssigner)
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));

        protected override CodeExpression GetCondition(PropertyInfo property, bool isEdit) => this.propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, isEdit);
    }

    private class UpdateToDomainObjectPropertyAssigner(
        IPropertyAssigner<TConfiguration> innerAssigner,
        PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
        : UpdateToDomainObjectPropertyAssignerBase<TConfiguration>(innerAssigner, propertyAssignerConfigurator.Configuration)
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));

        protected override CodeExpression GetCondition(PropertyInfo property) => this.propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, true);
    }


    private class StrictToDomainObjectPropertyAssigner(
        IPropertyAssigner<TConfiguration> innerAssigner,
        PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
        : StrictToDomainObjectPropertyAssignerBase<TConfiguration>(innerAssigner)
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));

        protected override CodeExpression GetCondition(PropertyInfo property) => this.propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, true);
    }


    private class ExpandMaybeSecurityToDomainObjectPropertyAssigner(
        IPropertyAssigner<TConfiguration> innerAssigner,
        PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
        : ExpandMaybeSecurityToDomainObjectPropertyAssignerBase<TConfiguration>(innerAssigner)
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));

        protected override CodeExpression GetCondition(PropertyInfo property) => this.propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, true);
    }
}
