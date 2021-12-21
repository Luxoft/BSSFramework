using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public abstract class MaybeSecurityToDomainObjectPropertyAssigner<TConfiguration> : SecurityServerPropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        protected MaybeSecurityToDomainObjectPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner)
            : base(innerAssigner)
        {
        }

        protected abstract CodeStatement GetSecurityAssignStatementInternal(PropertyInfo property, CodeExpression justValueRefExpr, CodeStatement innerAssignStatement);

        protected sealed override CodeStatement GetSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            if (property.HasAttribute<VersionAttribute>())
            {
                return this.InnerAssigner.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
            }
            else
            {
                var sourcePropertyTypeRef = this.CodeTypeReferenceService.GetCodeTypeReference(property);
                var sourcePropertyTypeJustRef = sourcePropertyTypeRef.ToJustReference();

                var justVarDecl = sourcePropertyTypeJustRef.ToVariableDeclarationStatement("just" + property.Name, sourcePropertyRef.ToAsCastExpression(sourcePropertyTypeJustRef));
                var justVarDeclRef = justVarDecl.ToVariableReferenceExpression();

                return new[]
                       {
                           justVarDecl,
                           this.GetSecurityAssignStatementInternal(property, justVarDeclRef, this.InnerAssigner.GetAssignStatement(property, justVarDeclRef.ToPropertyReference("Value"), targetPropertyRef))
                       }.Composite();
            }
        }
    }
}
