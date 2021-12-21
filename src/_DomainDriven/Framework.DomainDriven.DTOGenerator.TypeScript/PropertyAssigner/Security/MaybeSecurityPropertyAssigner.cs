using System;
using System.CodeDom;
using System.Reflection;

using Framework.Security;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner.Security
{
    /// <summary>
    /// Maybe security property assigner
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public abstract class MaybeSecurityPropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        private readonly SecurityDirection direction;

        protected MaybeSecurityPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner, SecurityDirection direction)
            : base(innerAssigner)
        {
            if (innerAssigner == null)
            {
                throw new ArgumentNullException(nameof(innerAssigner));
            }

            this.direction = direction;
            this.Inner = innerAssigner;
        }

        public IPropertyAssigner Inner { get; private set; }

        public sealed override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            if (this.IsSecurityProperty(property))
            {
                if (this.direction == SecurityDirection.FromPlainJs)
                {
                    return this.GetUnwrapSecurityAssignStatement(property, sourcePropertyRef, targetPropertyRef);
                }

                if (this.direction == SecurityDirection.FromObservable)
                {
                    return this.Inner.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
                }

                if (this.direction == SecurityDirection.ToStrict)
                {
                    return this.GetWrapSecurityAssignStatement(property, sourcePropertyRef, targetPropertyRef);
                }

                throw new NotImplementedException("Can't proceed such direction: " + this.direction);
            }

            return this.Inner.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
        }

        protected virtual bool IsSecurityProperty(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return property.IsSecurity();
        }

        protected abstract CodeStatement GetUnwrapSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef);

        protected abstract CodeStatement GetWrapSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef);
    }
}
