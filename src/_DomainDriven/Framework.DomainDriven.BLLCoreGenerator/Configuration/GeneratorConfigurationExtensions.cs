﻿using System.CodeDom;
using System.Reflection;

using Framework.Core;
using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator;

public static class GeneratorConfigurationExtensions
{
    public static CodeExpression GetSecurityCodeExpression(
        this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration,
        SecurityRule securityRule)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        if (securityRule is SecurityRule.SpecialSecurityRule)
        {
            return typeof(SecurityRule).ToTypeReferenceExpression().ToPropertyReference(securityRule.ToString());
        }
        else if (securityRule is SecurityRule.NonExpandedRolesSecurityRule)
        {
            return typeof(SecurityRole).ToTypeReferenceExpression().ToPropertyReference(securityRule.ToString());
        }
        else
        {
            var realSecurityRuleTypes = configuration.Environment.SecurityRuleTypeList;

            var request = from realSecurityRuleType in realSecurityRuleTypes

                          from prop in realSecurityRuleType.GetProperties()

                          where GetSecurityRule(prop) == securityRule

                          select realSecurityRuleType.ToTypeReferenceExpression().ToPropertyReference(prop);


            return request.Single(() => new Exception($"Security rule '{securityRule}' not found"));
        }
    }

    private static SecurityRule GetSecurityRule(PropertyInfo property)
    {
        return property.GetValue(null) switch
        {
            SecurityOperation securityOperation => securityOperation,
            SecurityRole securityRole => securityRole,
            SecurityRule.SpecialSecurityRule securityRule => securityRule,
            _ => null
        };
    }

    public static CodeExpression GetDisabledSecurityCodeExpression(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetSecurityCodeExpression(SecurityRule.Disabled);
    }


    public static CodeTypeDeclaration GetBLLContextContainerCodeTypeDeclaration(this IGeneratorConfigurationBase configuration, string typeName, bool asAbstract, CodeTypeReference containerType = null)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (typeName == null) throw new ArgumentNullException(nameof(typeName));

        var contextParameter = configuration.BLLContextInterfaceTypeReference.ToParameterDeclarationExpression("context");
        var contextParameterRefExpr = contextParameter.ToVariableReferenceExpression();

        return new CodeTypeDeclaration
               {
                       Name = typeName,
                       TypeAttributes = asAbstract ? (TypeAttributes.Public | TypeAttributes.Abstract) : TypeAttributes.Public,
                       IsPartial = true,
                       Members =
                       {
                               new CodeConstructor
                               {
                                       Attributes = asAbstract ? MemberAttributes.Family : MemberAttributes.Public,
                                       Parameters = { contextParameter },
                                       BaseConstructorArgs = { contextParameterRefExpr }
                               }
                       },

                       BaseTypes =
                       {
                               containerType ?? typeof(BLLContextContainer<>).ToTypeReference(configuration.BLLContextInterfaceTypeReference)
                       }
               };
    }

    public static CodeTypeDeclaration GetServiceProviderContainerCodeTypeDeclaration(this IGeneratorConfigurationBase configuration, string typeName, bool asAbstract, CodeTypeReference baseType)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (typeName == null) throw new ArgumentNullException(nameof(typeName));

        var parameter = typeof(IServiceProvider).ToTypeReference().ToParameterDeclarationExpression("serviceProvider");
        var parameterRefExpr = parameter.ToVariableReferenceExpression();

        return new CodeTypeDeclaration
               {
                   Name = typeName,
                   TypeAttributes = asAbstract ? (TypeAttributes.Public | TypeAttributes.Abstract) : TypeAttributes.Public,
                   IsPartial = true,
                   Members =
                   {
                       new CodeConstructor
                       {
                           Attributes = asAbstract ? MemberAttributes.Family : MemberAttributes.Public,
                           Parameters = { parameter },
                           BaseConstructorArgs = { parameterRefExpr }
                       }
                   },

                   BaseTypes =
                   {
                       baseType
                   }
               };
    }
}
