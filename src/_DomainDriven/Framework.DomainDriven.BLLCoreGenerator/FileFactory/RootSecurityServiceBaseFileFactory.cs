using System;
using System.CodeDom;
using System.Linq;

using Framework.CodeDom;
using Framework.SecuritySystem;

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

        return this.Configuration.GetBLLContextContainerCodeTypeDeclaration(this.Name, true, containerType);
    }

    private CodeTypeMember GetRegisterDependencyInjectionMethod()
    {
        var methodName = "Register";

        var serviceCollectionParameter = new CodeParameterDeclarationExpression(typeof(IServiceCollection), "serviceCollection");

        Func<Type, Type> getDomainSecurityServiceTypeRef = domainType => this.Configuration.Environment.SecurityOperationCodeType.IsEnum

                                                                                 ? typeof(IDomainSecurityService<,>).MakeGenericType(domainType, this.Configuration.Environment.SecurityOperationCodeType)

                                                                                 : typeof(IDomainSecurityService<>).MakeGenericType(domainType);


        var addScopedStatements = from domainType in this.Configuration.SecurityServiceDomainTypes

                                  let domainTypeServiceImpl =  this.Configuration.GetCodeTypeReference(domainType, FileType.DomainSecurityService)

                                  let addScopedMethod = typeof(ServiceCollectionServiceExtensions).ToTypeReferenceExpression()
                                          .ToMethodReferenceExpression(
                                                                       nameof(ServiceCollectionServiceExtensions.AddScoped),
                                                                       getDomainSecurityServiceTypeRef(domainType).ToTypeReference(),
                                                                       domainTypeServiceImpl)

                                  select serviceCollectionParameter.ToVariableReferenceExpression().ToStaticMethodInvokeExpression(addScopedMethod).ToExpressionStatement();

        return new CodeMemberMethod
               {
                       Name = methodName,
                       Attributes = MemberAttributes.Public | MemberAttributes.Static,
                       Parameters = { serviceCollectionParameter },
               }.WithStatements(addScopedStatements);
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var securityBaseMember in this.Configuration.RootSecurityServerGenerator.GetBaseMembers())
        {
            yield return securityBaseMember;
        }

        yield return this.GetRegisterDependencyInjectionMethod();
    }
}
