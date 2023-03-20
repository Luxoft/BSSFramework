using System;
using System.CodeDom;
using Framework.CodeDom;
using Framework.CodeDom.TypeScript;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;

/// <summary>
/// Identity codedom helper extensions
/// </summary>
public static class IdentityCodeDomHelper
{
    public static CodeTypeMember GenerateIdentityFromStaticInitializeMethodJs(
            this IFileFactory<ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>> fileFactory)
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var codeTypeReference = new CodeTypeReference(Constants.UnknownTypeName);
        var sourceParameter = new CodeParameterDeclarationExpression(codeTypeReference, Constants.SourceVariableName);

        var targetRef = fileFactory.CurrentReference;

        var returnStatement = targetRef.ToObjectCreateExpression(sourceParameter.ToVariableReferenceExpression()
                                                                                .ToPropertyReference(fileFactory.Configuration.Environment.IdentityProperty.Name)).ToMethodReturnStatement();

        var notNullStatement = new ToIsNullOrUndefinedConditionStatement(sourceParameter.ToVariableReferenceExpression())
                               {
                                       TrueStatements =
                                       {
                                               new CodePrimitiveExpression(null).ToMethodReturnStatement()
                                       }
                               };

        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Public | MemberAttributes.Static,
                       Parameters = { sourceParameter },
                       Name = Constants.FromJsMethodName,
                       ReturnType = targetRef,
                       Statements = { notNullStatement, returnStatement }
               };
    }

    public static CodeTypeMember GenerateToNativeJsonMethod(this IFileFactory<ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>> fileFactory)
    {
        return GenerateSelfAsMethod(fileFactory, Constants.ToNativeJsonMethodName);
    }

    public static CodeTypeMember GenerateSelfToJson(this IFileFactory<ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>> fileFactory)
    {
        return GenerateSelfAsMethod(fileFactory, Constants.ToJsMethodName);
    }

    private static CodeTypeMember GenerateSelfAsMethod(this IFileFactory<ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>> fileFactory, string methodName)
    {
        if (fileFactory == null)
        {
            throw new ArgumentNullException(nameof(fileFactory));
        }

        var sourceParameterRef = new CodeThisReferenceExpression();

        var codeMemberMethod = new CodeMemberMethod
                               {
                                       Name = methodName,
                                       Attributes = MemberAttributes.Family,
                                       ReturnType = fileFactory.CurrentReference
                               };

        codeMemberMethod.Statements.Add(sourceParameterRef.ToMethodReturnStatement());
        return codeMemberMethod;
    }
}
