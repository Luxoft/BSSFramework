using System.CodeDom;
using System.Reflection;

using Framework.CodeGeneration.Configuration._Container;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security.DomainObjectToSecurity;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security.ExpandMaybe;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security.SecurityToDomainObject.Strict;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security.SecurityToDomainObject.Update;

namespace Framework.CodeGeneration.DTOGenerator.Server.Configuration.PropertyAssigner;

public abstract class PropertyAssignerConfiguratorBase<TConfiguration>(TConfiguration configuration)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), IPropertyAssignerConfigurator
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
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

        protected override CodeExpression GetCondition(PropertyInfo property, bool isEdit)
        {
            return this.propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, isEdit);
        }
    }

    private class UpdateToDomainObjectPropertyAssigner(
        IPropertyAssigner<TConfiguration> innerAssigner,
        PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
        : UpdateToDomainObjectPropertyAssignerBase<TConfiguration>(innerAssigner, propertyAssignerConfigurator.Configuration)
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));

        protected override CodeExpression GetCondition(PropertyInfo property)
        {
            return this.propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, true);
        }
    }


    private class StrictToDomainObjectPropertyAssigner(
        IPropertyAssigner<TConfiguration> innerAssigner,
        PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
        : StrictToDomainObjectPropertyAssignerBase<TConfiguration>(innerAssigner)
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));

        protected override CodeExpression GetCondition(PropertyInfo property)
        {
            return this.propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, true);
        }
    }


    private class ExpandMaybeSecurityToDomainObjectPropertyAssigner(
        IPropertyAssigner<TConfiguration> innerAssigner,
        PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
        : ExpandMaybeSecurityToDomainObjectPropertyAssignerBase<TConfiguration>(innerAssigner)
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));

        protected override CodeExpression GetCondition(PropertyInfo property)
        {
            return this.propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, true);
        }
    }
}
