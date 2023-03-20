using System;
using System.CodeDom;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

public abstract class PropertyAssignerConfiguratorBase<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IPropertyAssignerConfigurator
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected PropertyAssignerConfiguratorBase(TConfiguration configuration)
            : base(configuration)
    {
    }


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


    private class DomainObjectToSecurityPropertyAssigner : DomainObjectToSecurityPropertyAssignerBase<TConfiguration>
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> _propertyAssignerConfigurator;


        public DomainObjectToSecurityPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner, PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
                : base(innerAssigner)
        {
            this._propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));
        }


        protected override CodeExpression GetCondition(PropertyInfo property, bool isEdit)
        {
            return this._propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, isEdit);
        }
    }

    private class UpdateToDomainObjectPropertyAssigner : UpdateToDomainObjectPropertyAssignerBase<TConfiguration>
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> _propertyAssignerConfigurator;


        public UpdateToDomainObjectPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner, PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
                : base(innerAssigner)
        {
            this._propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));
        }


        protected override CodeExpression GetCondition(PropertyInfo property)
        {
            return this._propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, true);
        }
    }


    private class StrictToDomainObjectPropertyAssigner : StrictToDomainObjectPropertyAssignerBase<TConfiguration>
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> _propertyAssignerConfigurator;


        public StrictToDomainObjectPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner, PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
                : base(innerAssigner)
        {
            this._propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));
        }


        protected override CodeExpression GetCondition(PropertyInfo property)
        {
            return this._propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, true);
        }
    }


    private class ExpandMaybeSecurityToDomainObjectPropertyAssigner : ExpandMaybeSecurityToDomainObjectPropertyAssignerBase<TConfiguration>
    {
        private readonly PropertyAssignerConfiguratorBase<TConfiguration> _propertyAssignerConfigurator;


        public ExpandMaybeSecurityToDomainObjectPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner, PropertyAssignerConfiguratorBase<TConfiguration> propertyAssignerConfigurator)
                : base(innerAssigner)
        {
            this._propertyAssignerConfigurator = propertyAssignerConfigurator ?? throw new ArgumentNullException(nameof(propertyAssignerConfigurator));
        }


        protected override CodeExpression GetCondition(PropertyInfo property)
        {
            return this._propertyAssignerConfigurator.GetPropertyHasAccessCondition(this, property, true);
        }
    }
}
