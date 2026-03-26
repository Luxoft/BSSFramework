using System.CodeDom;
using System.Reflection;

using Framework.BLL.BLL;
using Framework.CodeDom;

namespace Framework.CodeGeneration.BLLGenerator.Configuration;

public static class GeneratorConfigurationExtensions
{
    public static CodeTypeDeclaration GetBLLContextContainerCodeTypeDeclaration(
        this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration,
        string typeName,
        bool asAbstract,
        CodeTypeReference? containerType = null)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (typeName == null) throw new ArgumentNullException(nameof(typeName));

        var bllContextInterfaceTypeReference = configuration.Environment.BLLCore.BLLContextInterfaceTypeReference;

        var contextParameter = bllContextInterfaceTypeReference.ToParameterDeclarationExpression("context");
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
                   BaseTypes = { containerType ?? typeof(BLLContextContainer<>).ToTypeReference(bllContextInterfaceTypeReference) }
               };
    }
}
