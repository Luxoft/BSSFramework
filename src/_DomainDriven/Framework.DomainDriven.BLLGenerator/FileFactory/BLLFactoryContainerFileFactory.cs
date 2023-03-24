using System;
using System.Linq;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Framework.Core;
using Framework.CodeDom;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLLGenerator;

public class BLLFactoryContainerFileFactory<TConfiguration> : BLLFactoryContainerFileFactoryBase<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLFactoryContainerFileFactory(TConfiguration configuration)
            : base(configuration)
    {
    }


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return this.Configuration.Environment.BLLCore.GetBLLContextContainerCodeTypeDeclaration(this.Name, false);
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        var contextTypeRef = this.Configuration.BLLContextTypeReference;
        var contextField = contextTypeRef.ToMemberField("Context");
        var contextFieldRefExpr = new CodeThisReferenceExpression().ToFieldReference(contextField);

        foreach (var domainType in this.Configuration.DomainTypes)
        {
            var bllFactoryInterfaceTypeRef = this.Configuration.Environment.BLLCore.GetCodeTypeReference(domainType, BLLCoreGenerator.FileType.BLLFactoryInterface);

            var getRequiredServiceMethod = typeof(ServiceProviderServiceExtensions).ToTypeReferenceExpression()
                                                                                   .ToMethodReferenceExpression(
                                                                                    nameof(ServiceProviderServiceExtensions.GetRequiredService),
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
        var methodName = "RegisterBLLFactory";

        var serviceCollectionParameter = new CodeParameterDeclarationExpression(typeof(IServiceCollection), "serviceCollection");

        var addScopedStatements = from domainType in this.Configuration.DomainTypes

                                  let factoryDecl = this.Configuration.Environment.BLLCore.GetCodeTypeReference(domainType, BLLCoreGenerator.FileType.BLLFactoryInterface)

                                  let factoryImpl = this.Configuration.GetCodeTypeReference(domainType, FileType.BLLFactory)

                                  let addScopedMethod = typeof(ServiceCollectionServiceExtensions).ToTypeReferenceExpression()
                                          .ToMethodReferenceExpression(
                                                                       nameof(ServiceCollectionServiceExtensions.AddScoped),
                                                                       factoryDecl,
                                                                       factoryImpl)

                                  select serviceCollectionParameter.ToVariableReferenceExpression().ToStaticMethodInvokeExpression(addScopedMethod).ToExpressionStatement();

        return new CodeMemberMethod
               {
                       Name = methodName,
                       Attributes = MemberAttributes.Public | MemberAttributes.Static,
                       Parameters = { serviceCollectionParameter },
               }.WithStatements(addScopedStatements);
    }
}
