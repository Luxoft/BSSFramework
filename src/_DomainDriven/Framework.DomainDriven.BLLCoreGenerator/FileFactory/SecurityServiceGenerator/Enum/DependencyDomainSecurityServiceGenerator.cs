using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class DependencyDomainSecurityServiceGenerator<TConfiguration> : DomainSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        private readonly DependencySecurityAttribute dependencySecurityAttr;

        public DependencyDomainSecurityServiceGenerator(TConfiguration configuration, Type domainType, DependencySecurityAttribute dependencySecurityAttr)
            : base(configuration, domainType)
        {
            this.dependencySecurityAttr = dependencySecurityAttr ?? throw new ArgumentNullException(nameof(dependencySecurityAttr));

            this.BaseServiceType = typeof(DependencyDomainSecurityService<,>).MakeGenericType(
                    this.DomainType,
                    this.dependencySecurityAttr.SourceType).ToTypeReference();
        }

        public override CodeTypeReference BaseServiceType { get; }

        public override IEnumerable<CodeTypeMember> GetMembers()
        {
            yield break;
        }

        public override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            yield break;
        }

        public override IEnumerable<(CodeTypeReference ParameterType, string Name, CodeExpression CustomBaseInvoke)> GetBaseTypeConstructorParameters()
        {
            yield return (typeof(ISecurityProvider<>).ToTypeReference(this.DomainType), "disabledSecurityProvider", null);
            yield return (typeof(IEnumerable<ISecurityRuleExpander>).ToTypeReference(), "securityRuleExpanders", null);
            yield return (typeof(IDomainSecurityService<>).ToTypeReference(this.dependencySecurityAttr.SourceType), "baseDomainSecurityService", null);
            yield return (typeof(IQueryableSource).ToTypeReference(), "queryableSource", null);

            yield return (null, null, this.BuildDependencyExpressionPath());
        }

        private CodeExpression BuildDependencyExpressionPath()
        {
            var lambdaExpr = new CodeParameterDeclarationExpression { Name = "domainObject" }.Pipe(param => new CodeLambdaExpression
                    {
                        Parameters = { param },
                        Statements = { this.DomainType.GetPropertyPath<DependencySecurityAttribute>().Aggregate((CodeExpression)param.ToVariableReferenceExpression(), (state, prop) => state.ToPropertyReference(prop)) }
                    });


            return typeof(DependencyDomainSecurityServicePath<,>).MakeGenericType(this.DomainType, this.dependencySecurityAttr.SourceType)
                                                                 .ToTypeReference()
                                                                 .ToObjectCreateExpression(lambdaExpr);
        }
    }
}
