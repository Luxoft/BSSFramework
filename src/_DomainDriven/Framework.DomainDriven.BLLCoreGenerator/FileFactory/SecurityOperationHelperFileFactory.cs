﻿using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.Security;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class SecurityRuleHelperFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public SecurityRuleHelperFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
    }

    public override FileType FileType => FileType.SecurityRuleHelper;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Attributes = MemberAttributes.Public,
                       Name = this.Name
               }.MarkAsStatic();
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        yield return this.GetRegisterMethod();
    }

    private CodeMemberMethod GetRegisterMethod()
    {
        var serviceCollectionParameter = new CodeParameterDeclarationExpression(typeof(IServiceCollection), "services");

        var domainObjectSecurityRuleInfoRequest =

            from domainType in this.Configuration.DomainTypes

            let viewSecurityRule = domainType.GetViewSecurityRule()

            let editSecurityRule = domainType.GetEditSecurityRule()

            where viewSecurityRule != null || editSecurityRule != null

            select this.GetRegisterStatement(serviceCollectionParameter.ToVariableReferenceExpression(), domainType, viewSecurityRule, editSecurityRule);

        return new CodeMemberMethod
               {
                   Attributes = MemberAttributes.Public | MemberAttributes.Static,

                   Name = "RegisterDomainObjectSecurityRules",

                   Parameters = { serviceCollectionParameter }
               }.Self(v => v.Statements.AddRange(domainObjectSecurityRuleInfoRequest.ToArray()));
    }
    private CodeExpressionStatement GetRegisterStatement(CodeExpression serviceCollectionExpr, Type domainType, SecurityRule viewSecurityRule, SecurityRule editSecurityRule)
    {
        throw new NotImplementedException();

        //var secPairs = new[] { new { Mode = SecurityRule.View, SecutirtRule = viewSecurityRule }, new { Mode = SecurityRule.Edit, SecutirtRule = editSecurityRule } };

        //var addSingletonMethod = typeof(ServiceCollectionServiceExtensions).ToTypeReferenceExpression()
        //                                                                   .ToMethodReferenceExpression(
        //                                                                       nameof(ServiceCollectionServiceExtensions.AddSingleton));

        //return secPairs.Aggregate(serviceCollectionExpr,
        //                          (pair, state) =>
        //                          {

        //                              var createExpr = typeof(DomainModeSecurityRuleInfo).ToTypeReference().ToObjectCreateExpression(
        //                                  domainType.ToTypeOfExpression(),
        //                                  viewSecurityRule.Maybe(v => this.Configuration.GetSecurityCodeExpression(v)) ?? new CodePrimitiveExpression(),
        //                                  editSecurityRule.Maybe(v => this.Configuration.GetSecurityCodeExpression(v)) ?? new CodePrimitiveExpression());
        //                          })




        //return serviceCollectionExpr.ToStaticMethodInvokeExpression(addSingletonMethod, createExpr).ToExpressionStatement();
    }
}
