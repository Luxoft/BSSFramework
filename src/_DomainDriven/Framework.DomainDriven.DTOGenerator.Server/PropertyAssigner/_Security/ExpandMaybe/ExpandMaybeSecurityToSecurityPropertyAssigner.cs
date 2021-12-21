using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class ExpandMaybeSecurityToSecurityPropertyAssigner<TConfiguration>  : MaybePropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        private readonly IPropertyCodeTypeReferenceService _sourceTypeReferenceService;

        public ExpandMaybeSecurityToSecurityPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner, IPropertyCodeTypeReferenceService sourceTypeReferenceService)
            : base(innerAssigner)
        {
            if (sourceTypeReferenceService == null) throw new ArgumentNullException(nameof(sourceTypeReferenceService));

            this._sourceTypeReferenceService = sourceTypeReferenceService;
        }


        protected override CodeStatement GetSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
            if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

            var sourcePropertyTypeRef = this._sourceTypeReferenceService.GetCodeTypeReference(property);
            var sourcePropertyTypeJustRef = sourcePropertyTypeRef.ToJustReference();

            var justVarDecl = new CodeVariableDeclarationStatement(sourcePropertyTypeJustRef, "just" + property.Name, sourcePropertyRef.ToAsCastExpression(sourcePropertyTypeJustRef));
            var justVarDeclRef = new CodeVariableReferenceExpression(justVarDecl.Name);

            var targetPropertyTypeRef = this.CodeTypeReferenceService.GetCodeTypeReference(property);

            var resultVarDecl = new CodeVariableDeclarationStatement(targetPropertyTypeRef, "result" + property.Name,

                                                                     property.PropertyType.IsCollection() ? new CodeObjectCreateExpression(targetPropertyTypeRef) : null);

            var resultVarDeclRef = new CodeVariableReferenceExpression(resultVarDecl.Name);

            return new CodeConditionStatement
            {
                Condition = new CodePrimitiveExpression(true),
                TrueStatements =
                {
                    justVarDecl,
                    new CodeNotNullConditionStatement (justVarDeclRef)
                    {
                        TrueStatements =
                        {
                            resultVarDecl,

                            this.InnerAssigner.GetAssignStatement(property, justVarDeclRef.ToPropertyReference("Value"), resultVarDeclRef),

                            resultVarDeclRef.ToAssignStatement(targetPropertyRef)
                        },
                        FalseStatements =
                        {
                            new CodeDefaultValueExpression(targetPropertyTypeRef).ToAssignStatement(targetPropertyRef)
                        }
                    },
                }
            };
        }
    }
}