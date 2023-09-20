using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
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

            this.BaseServiceType = typeof(DependencyDomainSecurityService<,,,>).MakeGenericType(
                    this.Configuration.Environment.PersistentDomainObjectBaseType,
                    this.DomainType,
                    this.dependencySecurityAttr.SourceType,
                    this.Configuration.Environment.GetIdentityType()).ToTypeReference();
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
            yield return (typeof(IDisabledSecurityProviderSource).ToTypeReference(), "disabledSecurityProviderSource", null);
            yield return (typeof(ISecurityOperationResolver<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType), "securityOperationResolver", null);
            yield return (typeof(IDomainSecurityService<>).ToTypeReference(this.dependencySecurityAttr.SourceType), "baseDomainSecurityService", null);
            yield return (typeof(IQueryableSource<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType), "queryableSource", null);

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
