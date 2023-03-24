using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner;

/// <summary>
/// Observable Projection Property Assigner
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class ObservableProjectionPropertyAssigner<TConfiguration> : MainPropertyAssigner<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public ObservableProjectionPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
    {
    }

    public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property.PropertyType.IsCollectionOrArray())
        {
            return this.MapArrayExpression(property, sourcePropertyRef, targetPropertyRef);
        }

        return new CodeDelegateInvokeExpression(
                                                targetPropertyRef,
                                                this.GetSimpleExpression(property.PropertyType, sourcePropertyRef, this.Configuration.GetCodeTypeReference(property.PropertyType, ObservableFileType.ObservableProjectionDTO)))
                .ToExpressionStatement();
    }

    protected override CodeStatement MapArrayExpression(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        var elementType = property.PropertyType.GetCollectionOrArrayElementType();

        var typeReference = this.Configuration
                                .GetCodeTypeReference(elementType, ObservableFileType.ObservableProjectionDTO)
                                .NormalizeTypeReference(elementType);

        CodeExpression invokeExpression;

        if (elementType.IsPrimitiveJsType())
        {
            invokeExpression = this.GetSimpleExpression(elementType, new CodeSnippetExpression(Constants.DefaultVariableName), typeReference);
        }
        else if (elementType.IsCollectionOrArray())
        {
            var internalInvokeExpression = new CodeLambdaExpression
                                           {
                                                   Parameters = new CodeParameterDeclarationExpressionCollection(new[] { new CodeParameterDeclarationExpression(Constants.VoidType, Constants.VarailableName) }),
                                                   Statements = { new CodeSnippetExpression(Constants.VarailableName) }
                                           };
            invokeExpression = new CodeSnippetExpression(Constants.DefaultVariableName).ToMethodInvokeExpression(Constants.MapMethodName, internalInvokeExpression);
        }
        else
        {
            invokeExpression =
                    typeReference.NormalizeTypeReference(elementType)
                                 .ToTypeReferenceExpression()
                                 .ToMethodInvokeExpression(
                                                           Constants.FromJsMethodName,
                                                           new CodeSnippetExpression(Constants.DefaultVariableName));
        }

        var callbackExpression = new CodeLambdaExpression
                                 {
                                         Parameters = new CodeParameterDeclarationExpressionCollection(new[] { new CodeParameterDeclarationExpression(Constants.VoidType, Constants.DefaultVariableName) }),
                                         Statements = { invokeExpression }
                                 };

        CodeExpression sourceExpression = sourcePropertyRef;

        return new IsDefinedConditionStatement(sourceExpression)
               {
                       TrueStatements =
                       {
                               new CodeDelegateInvokeExpression(targetPropertyRef, sourceExpression.ToMethodInvokeExpression(Constants.MapMethodName, callbackExpression))
                       }
               };
    }

    protected override CodeExpression GetSimpleExpression(System.Type propertyType, CodeExpression sourcePropertyRef, CodeTypeReference reference)
    {
        if (propertyType.IsDateTime())
        {
            return sourcePropertyRef.ConvertToDate();
        }

        if (propertyType.IsPeriod())
        {
            return sourcePropertyRef.ConvertToObservablePeriod();
        }

        if (!propertyType.IsPrimitiveJsType())
        {
            return reference.ToTypeReferenceExpression().ToMethodInvokeExpression(Constants.FromJsMethodName, sourcePropertyRef);
        }

        return sourcePropertyRef;
    }
}
