using System.CodeDom;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner;

/// <summary>
/// Main To strict property assigner
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class MainToStrictPropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public MainToStrictPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
    {
    }

    public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property.PropertyType.IsCollectionOrArray())
        {
            CodeExpression invokeExpression = new CodeSnippetExpression(Constants.DefaultVariableName);

            if (this.Configuration.IsCollectionProperty(property))
            {
                var fileType = this.CodeTypeReferenceService.GetCollectionFileType(property);
                invokeExpression = this.GetSimpleExpression(property, invokeExpression, fileType, true);
            }
            else
            {
                invokeExpression = this.GetSimpleExpression(property, invokeExpression, isCollection: true);
            }

            var callbackExpression = new CodeLambdaExpression
                                     {
                                             Parameters = new CodeParameterDeclarationExpressionCollection(new[] { new CodeParameterDeclarationExpression(Constants.VoidType, Constants.DefaultVariableName) }),
                                             Statements = { invokeExpression }
                                     };

            return new IsDefinedConditionStatement(sourcePropertyRef)
                   {
                           TrueStatements =
                           {
                                   sourcePropertyRef
                                           .ToMethodInvokeExpression("map", callbackExpression)
                                           .ToAssignStatement(targetPropertyRef)
                           }
                   };
        }

        if (!property.IsPrimitiveJsType())
        {
            return new IsDefinedConditionStatement(sourcePropertyRef)
                   {
                           TrueStatements =
                           {
                                   this.GetSimpleExpression(property, sourcePropertyRef, this.CodeTypeReferenceService.GetReferenceFileType(property)).ToAssignStatement(targetPropertyRef)
                           }
                   };
        }

        return this.GetSimpleExpression(property, sourcePropertyRef).ToAssignStatement(targetPropertyRef);
    }

    private CodeExpression GetSimpleExpression(PropertyInfo property, CodeExpression sourcePropertyRef, RoleFileType fileType = null, bool isCollection = false)
    {
        if (fileType == null)
        {
            return sourcePropertyRef;
        }

        if (fileType.IsIdentity())
        {
            return sourcePropertyRef.ToPropertyReference(this.Configuration.DTOIdentityPropertyName);
        }

        if (!fileType.IsStrict())
        {
            return sourcePropertyRef;
        }

        if (isCollection || this.Configuration.DomainTypes.Contains(property.PropertyType))
        {
            return sourcePropertyRef.ToMethodInvokeExpression(Constants.ToStrictMethodName);
        }

        return sourcePropertyRef;
    }
}
