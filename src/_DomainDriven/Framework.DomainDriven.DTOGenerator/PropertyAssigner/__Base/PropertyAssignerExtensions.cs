using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;

namespace Framework.DomainDriven.DTOGenerator
{
    public static class PropertyAssignerExtensions
    {
        public static CodeStatement GetAssignStatementBySource(this IPropertyAssigner propertyAssigner, PropertyInfo property, CodeExpression sourceObjectRef, CodeExpression targetObjectRef)
        {
            if (propertyAssigner == null) throw new ArgumentNullException(nameof(propertyAssigner));
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (sourceObjectRef == null) throw new ArgumentNullException(nameof(sourceObjectRef));
            if (targetObjectRef == null) throw new ArgumentNullException(nameof(targetObjectRef));


            return propertyAssigner.GetAssignStatement(property, sourceObjectRef.ToPropertyReference(property), targetObjectRef.ToPropertyReference(property));
        }

        public static IPropertyAssigner<TConfiguration> WithConfiguration<TConfiguration>(this IPropertyAssigner propertyAssigner, TConfiguration configuration)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
        {
            if (propertyAssigner == null) throw new ArgumentNullException(nameof(propertyAssigner));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var genericAssigner = propertyAssigner as IPropertyAssigner<TConfiguration>;

            if (genericAssigner != null && genericAssigner.Configuration == configuration)
            {
                return genericAssigner;
            }
            else
            {
                return new ImplPropertyAssigner<TConfiguration>(configuration, propertyAssigner);
            }
        }

        private class ImplPropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
        {
            private readonly IPropertyAssigner _innerAssigner;

            public ImplPropertyAssigner(TConfiguration configuration, IPropertyAssigner innerAssigner)
                : base(configuration, innerAssigner.DomainType, innerAssigner.FileType)
            {
                this._innerAssigner = innerAssigner;
            }

            public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
            {
                if (property == null) throw new ArgumentNullException(nameof(property));
                if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
                if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

                return this._innerAssigner.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
            }
        }
    }
}