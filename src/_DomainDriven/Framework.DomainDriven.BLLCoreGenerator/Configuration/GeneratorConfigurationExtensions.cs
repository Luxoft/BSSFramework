using System.CodeDom;
using System.Reflection;

using Framework.Core;
using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator;

public static class GeneratorConfigurationExtensions
{
    public static CodeExpression GetSecurityCodeExpression(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, SecurityOperation securityOperation, Type securityOperationType = null)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var realSecurityOperationType = securityOperationType ?? configuration.Environment.SecurityOperationType;

        var prop = realSecurityOperationType.GetProperties()
                                            .Single(p => (SecurityOperation)p.GetValue(null) == securityOperation, () => new Exception($"Type '{realSecurityOperationType}' does not contains operation '{securityOperation}'"));

        return realSecurityOperationType.ToTypeReferenceExpression().ToPropertyReference(prop);
    }

    public static CodeExpression GetDisabledSecurityCodeExpression(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return configuration.GetSecurityCodeExpression(SecurityOperation.Disabled);
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
