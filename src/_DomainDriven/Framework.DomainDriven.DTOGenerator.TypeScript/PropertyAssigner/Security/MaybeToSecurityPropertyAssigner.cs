using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner.Security
{
    /// <summary>
    /// Maybe To security property Assigner
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class MaybeToSecurityPropertyAssigner<TConfiguration> : MaybeSecurityPropertyAssigner<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public MaybeToSecurityPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner, IPropertyCodeTypeReferenceService sourceTypeReferenceService, SecurityDirection direction)
            : base(innerAssigner, direction)
        {
            if (sourceTypeReferenceService == null)
            {
                throw new ArgumentNullException(nameof(sourceTypeReferenceService));
            }
        }

        protected override CodeStatement GetUnwrapSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            return this.Inner.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
        }

        protected override CodeStatement GetWrapSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            var targetPropertyTypeRef = this.CodeTypeReferenceService.GetCodeTypeReference(property);

            var resultVarDecl = new CodeVariableDeclarationStatement(
                targetPropertyTypeRef,
                "result" + property.Name,
                property.PropertyType.IsCollection() ? new CodeObjectCreateExpression(targetPropertyTypeRef) : null);

            var resultVarDeclRef = new CodeVariableReferenceExpression(resultVarDecl.Name);

            return new IsDefinedConditionStatement(sourcePropertyRef)
            {
                TrueStatements =
                {
                    resultVarDecl,

                    this.Inner.GetAssignStatement(property, sourcePropertyRef, resultVarDeclRef),

                    new CodeObjectCreateExpression(targetPropertyTypeRef.ToJustReference(), resultVarDeclRef).ToAssignStatement(targetPropertyRef)
                },
                FalseStatements =
                {
                    targetPropertyTypeRef.ToNothingValueExpression().ToAssignStatement(targetPropertyRef)
                }
            };
        }
    }
}
