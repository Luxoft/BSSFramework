using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner;

/// <summary>
/// Main To update property assigner
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class MainToUpdatePropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public MainToUpdatePropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
    {
    }

    public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        return this.GetSimpleAssignStatement(property, sourcePropertyRef, targetPropertyRef);
    }

    private CodeStatement GetSimpleAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property.PropertyType.IsCollectionOrArray())
        {
            var elementType = property.PropertyType.GetCollectionOrArrayElementType();

            bool isIdentity = false;

            if (this.Configuration.IsCollectionProperty(property))
            {
                isIdentity = this.CodeTypeReferenceService.GetCollectionFileType(property) == DTOGenerator.FileType.IdentityDTO;
            }

            CodeExpression expression = new CodeSnippetExpression(Constants.DefaultVariableName);
            expression = this.GetExpression(elementType, expression, isIdentity);

            var callbackExpression = new CodeLambdaExpression
                                     {
                                             Parameters = new CodeParameterDeclarationExpressionCollection(new[] { new CodeParameterDeclarationExpression(Constants.VoidType, Constants.DefaultVariableName) }),
                                             Statements = { expression }
                                     };

            return new IsDefinedConditionStatement(sourcePropertyRef)
                   {
                           TrueStatements =
                           {
                                   sourcePropertyRef.ToMethodInvokeExpression("map", callbackExpression).ToAssignStatement(targetPropertyRef)
                           }
                   };
        }

        if (!property.IsPrimitiveJsType())
        {
            var isIdentity = this.CodeTypeReferenceService.GetFileType(property) == DTOGenerator.FileType.IdentityDTO;
            return new IsDefinedConditionStatement(sourcePropertyRef)
                   {
                           TrueStatements =
                           {
                                   this.GetExpression(property.PropertyType, sourcePropertyRef, isIdentity)
                                       .ToAssignStatement(targetPropertyRef)
                           }
                   };
        }

        return this.GetPrimitiveExpression(sourcePropertyRef).ToAssignStatement(targetPropertyRef);
    }

    private CodeExpression GetExpression(Type type, CodeExpression sourcePropertyRef, bool isIdentity = false)
    {
        if (type.IsPrimitiveJsType())
        {
            return this.GetPrimitiveExpression(sourcePropertyRef);
        }

        if (!isIdentity)
        {
            if (this.Configuration.DomainTypes.Contains(type))
            {
                return sourcePropertyRef.ToMethodInvokeExpression(Constants.ToUpdateMethodName);
            }

            // For class and structs
            return sourcePropertyRef;
        }

        return sourcePropertyRef.ToPropertyReference(this.Configuration.DTOIdentityPropertyName);
    }

    private CodeExpression GetPrimitiveExpression(CodeExpression sourcePropertyRef)
    {
        return sourcePropertyRef;
    }
}
