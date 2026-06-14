using System.CodeDom;

using Anch.Core;
using Anch.DependencyInjection;
using Anch.OData.Domain.QueryLanguage;
using Anch.SecuritySystem;

using Framework.BLL;
using Framework.CodeDom.Extend;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.FileGeneration.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class BLLFactoryContainerFileFactory<TConfiguration>(TConfiguration configuration) : BLLFactoryContainerFileFactoryBase<TConfiguration>(configuration)
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
{
    protected override CodeTypeDeclaration GetCodeTypeDeclaration() => this.Configuration.GetBLLContextContainerCodeTypeDeclaration(this.Name, false);

    protected override IEnumerable<CodeTypeReference> GetBaseTypes() => [.. base.GetBaseTypes(), typeof(IBLLFactoryInitializer).ToTypeReference()];

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        var contextTypeRef = this.Configuration.BLLContextTypeReference;
        var contextField = contextTypeRef.ToMemberField("Context");
        var contextFieldRefExpr = new CodeThisReferenceExpression().ToFieldReference(contextField);

        foreach (var domainType in this.Configuration.DomainTypes)
        {
            var bllFactoryInterfaceTypeRef = this.Configuration.Environment.BLLCore.GetCodeTypeReference(domainType, BLLCoreGenerator.FileType.BLLFactoryInterface);

            var getRequiredServiceMethod = typeof(Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions).ToTypeReferenceExpression()
                                                                                   .ToMethodReferenceExpression(
                                                                                    nameof(Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService),
                                                                                    bllFactoryInterfaceTypeRef);

            var factoryProperty = new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = domainType.Name + "Factory",
                Type = bllFactoryInterfaceTypeRef,
                GetStatements =
                                          {
                                                  contextFieldRefExpr.ToPropertyReference("ServiceProvider").ToStaticMethodInvokeExpression(getRequiredServiceMethod).ToMethodReturnStatement()
                                          }
            };

            var factoryPropertyRefExpr = new CodeThisReferenceExpression().ToPropertyReference(factoryProperty);

            yield return factoryProperty;


            var bllTypeRef = this.Configuration.Environment.BLLCore.GetCodeTypeReference(domainType, BLLCoreGenerator.FileType.BLLInterface);

            var defaultBLLFieldMember = bllTypeRef.ToMemberField($"{domainType.Name.ToStartLowerCase()}BLL");

            yield return defaultBLLFieldMember;

            yield return new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = domainType.Name,
                Type = bllTypeRef,
                GetStatements =
                                 {
                                         new CodeThisReferenceExpression().ToFieldReference(defaultBLLFieldMember)
                                                                          .ToMethodReturnStatementWithLazyInitialize(factoryPropertyRefExpr.ToMethodInvokeExpression("Create"))
                                 }
            };
        }

        foreach (var member in this.GetGenericFactoryMembers(contextFieldRefExpr))
        {
            yield return member;
        }

        yield return this.GetRegisterBLLFactoryDependencyInjectionMethod();
    }

    private IEnumerable<CodeTypeMember> GetGenericFactoryMembers(CodeExpression contextFieldRefExpr)
    {

        var fileTypes = new[]
                        {
                                new { FileType = FileType.DefaultBLLFactory, Type = this.Configuration.Environment.BLLCore.SecurityBLLFactoryType },
                                new { FileType = FileType.ImplementedBLLFactory, Type = this.Configuration.Environment.BLLCore.SecurityBLLFactoryType }
                        };

        foreach (var pair in fileTypes)
        {
            var bllTypeRef = this.Configuration.GetCodeTypeReference(null, pair.FileType);

            var bllFactoryField = bllTypeRef.ToMemberField(pair.FileType.ToString().ToStartLowerCase());

            yield return bllFactoryField;

            yield return new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = pair.Type.ToTypeReference(),
                Name = pair.FileType.ToString().SkipLast("BLLFactory", true),
                GetStatements =
                                 {
                                         new CodeThisReferenceExpression().ToFieldReference(bllFactoryField)
                                                                          .ToMethodReturnStatementWithLazyInitialize(bllTypeRef.ToObjectCreateExpression(contextFieldRefExpr))
                                 }
            };
        }
    }

    private CodeMemberMethod GetRegisterBLLFactoryDependencyInjectionMethod()
    {
        var methodName = nameof(IBLLFactoryInitializer.RegisterBLLFactory);

        var serviceCollectionParameter = new CodeParameterDeclarationExpression(typeof(Microsoft.Extensions.DependencyInjection.IServiceCollection), "serviceCollection");

        var addScopedStatements = from domainType in this.Configuration.DomainTypes

                                  from statement in this.GetRegisterBLLStatements(serviceCollectionParameter, domainType)

                                  select statement;

        return new CodeMemberMethod
        {
            Name = methodName,
            Attributes = MemberAttributes.Public | MemberAttributes.Static,
            Parameters = { serviceCollectionParameter },
        }.WithStatements(addScopedStatements);
    }



    private IEnumerable<CodeExpressionStatement> GetRegisterBLLStatements(CodeParameterDeclarationExpression serviceCollectionParameter, Type domainType)
    {
        var serviceCollectionRef = serviceCollectionParameter.ToVariableReferenceExpression();

        var bllDecl = this.Configuration.Environment.BLLCore.GetCodeTypeReference(domainType, BLLCoreGenerator.FileType.BLLInterface);

        var bllFactoryDecl = this.Configuration.Environment.BLLCore.GetCodeTypeReference(domainType, BLLCoreGenerator.FileType.BLLFactoryInterface);

        var bllFactoryImpl = this.Configuration.GetCodeTypeReference(domainType, FileType.BLLFactory);

        var addScopedMethod = typeof(Framework.BLL.DependencyInjection.BllServiceCollectionExtensions).ToTypeReferenceExpression()
                                                                        .ToMethodReferenceExpression(
                                                                            nameof(Framework.BLL.DependencyInjection.BllServiceCollectionExtensions.AddBLL),
                                                                            this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                                                                            domainType.ToTypeReference(),
                                                                            this.Configuration.Environment.GetIdentityType().ToTypeReference(),
                                                                            bllFactoryDecl,
                                                                            bllFactoryImpl,
                                                                            bllDecl);

        yield return serviceCollectionRef.ToStaticMethodInvokeExpression(addScopedMethod).ToExpressionStatement();
    }
}

