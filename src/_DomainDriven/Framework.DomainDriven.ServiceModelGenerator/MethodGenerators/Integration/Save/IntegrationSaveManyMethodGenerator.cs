using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.Transfering;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public class IntegrationSaveManyMethodGenerator<TConfiguration> : IntegrationBaseSaveMethodGenerator<TConfiguration>
        where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        private readonly IntegrationSaveMethodGenerator<TConfiguration> _singleSaveGenerator;

        public IntegrationSaveManyMethodGenerator([NotNull] IntegrationSaveMethodGenerator<TConfiguration> singleSaveGenerator)
            : base(singleSaveGenerator.Configuration, singleSaveGenerator.DomainType)
        {
            if (singleSaveGenerator == null) throw new ArgumentNullException(nameof(singleSaveGenerator));

            this._singleSaveGenerator = singleSaveGenerator;
        }


        public override MethodIdentity Identity { get; } = MethodIdentityType.IntegrationSaveMany;

        protected override string Name => "Save" + this.DomainType.GetPluralizedDomainName();

        protected override CodeTypeReference ReturnType => this.IdentTypeRef.ToEnumerableReference();


        protected override string GetComment()
        {
            return $"Save {this.DomainType.GetPluralizedDomainName()}";
        }

        protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
        {
            yield return this.RichIntegrationTypeRef
                             .ToArrayReference()
                             .ToParameterDeclarationExpression(this.DomainType.GetPluralizedDomainName().ToStartLowerCase());
        }

        protected override IEnumerable<CodeMemberMethod> GetFacadeMethods(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr)
        {
            foreach (var method in base.GetFacadeMethods(evaluateDataParameterExpr, bllParameterExpr))
            {
                yield return method;
            }

            if (!this.Attribute.CountType.HasFlag(CountType.Single))
            {
                yield return this._singleSaveGenerator.GetFacadeMethodWithBLL(evaluateDataParameterExpr, bllParameterExpr);
            }
        }

        protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
        {
            if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));
            if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));

            foreach (var baseStatement in base.GetFacadeMethodInternalStatements(evaluateDataExpr, bllRefExpr))
            {
                yield return baseStatement;
            }

            var convertLambda = new CodeParameterDeclarationExpression { Name = this.DomainType.Name.ToStartLowerCase() }.Pipe(lambdaParam =>

                new CodeLambdaExpression
                {
                    Parameters =
                    {
                        lambdaParam
                    },
                    Statements =
                    {
                        new CodeThisReferenceExpression().ToMethodInvokeExpression(this._singleSaveGenerator.InternalName, lambdaParam.ToVariableReferenceExpression(), evaluateDataExpr, bllRefExpr )
                    }
                });

            yield return this.Parameter
                             .ToVariableReferenceExpression()
                             .ToStaticMethodInvokeExpression(

                                    typeof(Framework.Core.EnumerableExtensions)
                                   .ToTypeReferenceExpression()
                                   .ToMethodReferenceExpression("ToList"), convertLambda)

                             .ToMethodReturnStatement();
        }
    }
}
