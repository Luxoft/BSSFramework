using System.CodeDom;
using System.Linq.Expressions;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class DependencyDomainSecurityServiceGenerator<TConfiguration> : DomainSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly DependencySecurityAttribute dependencySecurityAttr;

    public DependencyDomainSecurityServiceGenerator(TConfiguration configuration, Type domainType, DependencySecurityAttribute dependencySecurityAttr)
            : base(configuration, domainType)
    {
        this.dependencySecurityAttr = dependencySecurityAttr ?? throw new ArgumentNullException(nameof(dependencySecurityAttr));

        this.BaseServiceType = typeof(DependencyDomainSecurityService<,,,,>).MakeGenericType(
         this.Configuration.Environment.PersistentDomainObjectBaseType,
         this.DomainType,
         this.dependencySecurityAttr.SourceType,
         this.Configuration.Environment.GetIdentityType(),
         this.Configuration.Environment.SecurityOperationCodeType).ToTypeReference();
    }

    public override CodeTypeReference BaseServiceType { get; }

    public override IEnumerable<CodeTypeMember> GetMembers()
    {
        yield return new CodeMemberProperty
                     {
                             Name = "Selector",
                             Attributes = MemberAttributes.Family | MemberAttributes.Override,
                             Type = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(this.DomainType, this.dependencySecurityAttr.SourceType)).ToTypeReference(),
                             GetStatements =
                             {
                                     new CodeParameterDeclarationExpression{ Name = "domainObject" }.Pipe(param => new CodeLambdaExpression
                                         {
                                                 Parameters = { param },
                                                 Statements = { this.DomainType.GetPropertyPath<DependencySecurityAttribute>().Aggregate((CodeExpression)param.ToVariableReferenceExpression(), (state, prop) => state.ToPropertyReference(prop)) }
                                         }).ToMethodReturnStatement()
                             }
                     };
    }

    public override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield break;
    }

    public override IEnumerable<(CodeTypeReference ParameterType, string Name)> GetBaseTypeConstructorParameters()
    {
        yield return (typeof(IAccessDeniedExceptionService<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType), "accessDeniedExceptionService");
        yield return (typeof(IDisabledSecurityProviderContainer<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType), "disabledSecurityProviderContainer");
        yield return (typeof(IDomainSecurityService<,>).ToTypeReference(this.dependencySecurityAttr.SourceType, this.Configuration.Environment.SecurityOperationCodeType), "baseDomainSecurityService");
        yield return (typeof(IQueryableSource<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType), "queryableSource");
    }
}
