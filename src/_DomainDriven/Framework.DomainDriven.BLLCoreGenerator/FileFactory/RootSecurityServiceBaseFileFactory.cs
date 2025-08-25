using System.CodeDom;

using Framework.CodeDom;
using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class RootSecurityServiceBaseFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public RootSecurityServiceBaseFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
    }


    public override FileType FileType { get; } = FileType.RootSecurityServiceBase;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var containerType = this.Configuration.RootSecurityServerGenerator.GetGenericRootSecurityServiceType();

        return this.Configuration.GetServiceProviderContainerCodeTypeDeclaration(this.Name, true, containerType);
    }

    private CodeTypeMember GetRegisterDependencyInjectionMethod()
    {
        var methodName = "Register";

        var serviceCollectionParameter = new CodeParameterDeclarationExpression(typeof(IServiceCollection), "services");

        var addScopedStatements = from domainType in this.Configuration.SecurityServiceDomainTypes

                                  from domainTypeRegisterStatement in this.GetDomainTypeRegisterStatements(serviceCollectionParameter.ToVariableReferenceExpression(), domainType)

                                  select domainTypeRegisterStatement;

        return new CodeMemberMethod
               {
                       Name = methodName,
                       Attributes = MemberAttributes.Public | MemberAttributes.Static,
                       Parameters = { serviceCollectionParameter },
               }.WithStatements(addScopedStatements);
    }

    private IEnumerable<CodeExpressionStatement> GetDomainTypeRegisterStatements(CodeExpression serviceCollectionExpr, Type domainType)
    {
        var domainTypeServiceImpl = this.Configuration.GetCodeTypeReference(domainType, FileType.DomainSecurityService);

        var baseDomainSecurityServiceType = typeof(IDomainSecurityService<>).MakeGenericType(domainType);

        var addScopedMethod = typeof(ServiceCollectionServiceExtensions).ToTypeReferenceExpression()
                                                                        .ToMethodReferenceExpression(
                                                                            nameof(ServiceCollectionServiceExtensions.AddScoped),
                                                                            baseDomainSecurityServiceType.ToTypeReference(),
                                                                            domainTypeServiceImpl);

        yield return serviceCollectionExpr.ToStaticMethodInvokeExpression(addScopedMethod).ToExpressionStatement();
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var securityBaseMember in this.Configuration.RootSecurityServerGenerator.GetBaseMembers())
        {
            yield return securityBaseMember;
        }

        yield return this.GetRegisterDependencyInjectionMethod();
    }
}
