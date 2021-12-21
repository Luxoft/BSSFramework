using System;
using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.BLLGenerator
{
    public class BLLFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public BLLFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        private Type EventOperationType => this.DomainType.GetEventOperationType(true);

        public override FileType FileType => FileType.BLL;


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            var baseBLLType = this.Configuration.Environment.BLLCore.GetSecurityDomainBLLBaseTypeReference(this.DomainType)
                .ToTypeReference(this.DomainType.ToTypeReference(), this.EventOperationType.ToTypeReference());


            var initializeMethodName = "Initialize";
            var initializeSnippet = new CodeSnippetTypeMember("\t\t" + $"partial void {initializeMethodName}();");
            var invoikeInitializeStatement = new CodeThisReferenceExpression().ToMethodInvokeExpression(initializeMethodName).ToExpressionStatement();


            var codeTypeDeclaration = new CodeTypeDeclaration
            {
                Name = this.Name,

                Attributes = MemberAttributes.Public,
                IsPartial = true,

                BaseTypes =
                {
                    baseBLLType,

                    this.Configuration.Environment.BLLCore.GetCodeTypeReference(this.DomainType, BLLCoreGenerator.FileType.BLLInterface)
                },
                Members =
                {
                    initializeSnippet
                }
            };

            {
                if (this.Configuration.GenerateBllConstructor(this.DomainType))
                {
                    var contextParameter = new CodeParameterDeclarationExpression
                    {
                        Type = this.Configuration.BLLContextTypeReference,
                        Name = "context"
                    };

                    var specificationEvaluatorParameterTypeRef = typeof(ISpecificationEvaluator).ToTypeReference();
                    var specificationEvaluatorParameter = specificationEvaluatorParameterTypeRef.ToParameterDeclarationExpression("specificationEvaluator = null");
                    var specificationEvaluatorParameterArg = specificationEvaluatorParameterTypeRef.ToParameterDeclarationExpression("specificationEvaluator").ToVariableReferenceExpression();

                    var contextParameterExpr = contextParameter.ToVariableReferenceExpression();

                    var securityProviderParameterTypeRef = typeof(ISecurityProvider<>).ToTypeReference(this.DomainType.ToTypeReference());
                    var securityProviderParameter = securityProviderParameterTypeRef.ToParameterDeclarationExpression("securityProvider");

                    var securityOperationConstructor = new CodeConstructor
                    {
                        Attributes = MemberAttributes.Public,
                        Parameters =
                        {
                            contextParameter,
                            securityProviderParameter,
                            specificationEvaluatorParameter
                        },
                        BaseConstructorArgs =
                        {
                            contextParameterExpr,
                            securityProviderParameter.ToVariableReferenceExpression(),
                            specificationEvaluatorParameterArg
                        },
                        Statements = { invoikeInitializeStatement }
                    };

                    codeTypeDeclaration.Members.Add(securityOperationConstructor);
                }
            }

            return codeTypeDeclaration;
        }
    }
}
