using System;
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
/// Main property assigner
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class MainPropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    private readonly bool unwrapObservable;

    public MainPropertyAssigner(IDTOSource<TConfiguration> source, bool unwrap = false)
            : base(source)
    {
        this.unwrapObservable = unwrap;
    }

    public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property.PropertyType.IsCollectionOrArray())
        {
            return this.MapArrayExpression(property, sourcePropertyRef, targetPropertyRef);
        }

        return this.GetSimpleAssignStatement(property, sourcePropertyRef, targetPropertyRef);
    }

    protected virtual CodeStatement MapArrayExpression(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        var type = property.PropertyType.GetCollectionOrArrayElementType();
        var reference = this.CodeTypeReferenceService.GetCodeTypeReference(property);

        var typeReference = reference?.TypeArguments.Count > 0
                                    ? reference.TypeArguments[0].NormalizeTypeReference(type)
                                    : this.CodeTypeReferenceService.GetCodeTypeReferenceByType(type).NormalizeTypeReference(type);

        var variableExpression = new CodeSnippetExpression(Constants.DefaultVariableName);

        CodeExpression lambdaExpression;

        if (this.unwrapObservable)
        {
            if (type.IsPrimitiveJsType())
            {
                lambdaExpression = this.GetSimpleExpression(type, variableExpression, typeReference);
            }
            else if (type.IsCollectionOrArray())
            {
                var internalInvokeExpression = new CodeLambdaExpression
                                               {
                                                       Parameters = new CodeParameterDeclarationExpressionCollection(new[] { new CodeParameterDeclarationExpression(Constants.VoidType, Constants.VarailableName) }),
                                                       Statements = { new CodeSnippetExpression(Constants.VarailableName) }
                                               };
                lambdaExpression = new CodeSnippetExpression(Constants.DefaultVariableName)
                        .ToMethodInvokeExpression(Constants.MapMethodName, internalInvokeExpression);
            }
            else
            {
                lambdaExpression = new CodeMethodInvokeExpression(variableExpression, Constants.ToJsMethodName);
            }
        }
        else
        {
            lambdaExpression = this.GetSimpleExpression(type, variableExpression, typeReference);
        }

        var callbackExpression = new CodeLambdaExpression
                                 {
                                         Parameters =
                                                 new CodeParameterDeclarationExpressionCollection(
                                                  new[] { new CodeParameterDeclarationExpression(Constants.VoidType, Constants.DefaultVariableName) }),
                                         Statements = { lambdaExpression }
                                 };

        CodeExpression sourceExpression = this.unwrapObservable ? sourcePropertyRef.UnwrapObservableProperty() : sourcePropertyRef;

        return new IsDefinedConditionStatement(sourceExpression)
               {
                       TrueStatements =
                       {
                               this.unwrapObservable
                                       ? new CodeDelegateInvokeExpression(sourcePropertyRef).ToMethodInvokeExpression(Constants.MapMethodName, callbackExpression)
                                               .ToAssignStatement(targetPropertyRef)
                                       : sourcePropertyRef.ToMethodInvokeExpression(Constants.MapMethodName, callbackExpression).ToAssignStatement(targetPropertyRef)
                       }
               };
    }

    protected virtual CodeExpression GetSimpleExpression(Type propertyType, CodeExpression sourcePropertyRef, CodeTypeReference reference)
    {
        if (this.unwrapObservable)
        {
            if (this.Configuration.DomainTypes.Contains(propertyType))
            {
                var expression = sourcePropertyRef.UnwrapObservableProperty();

                return new CodeMethodInvokeExpression(expression, Constants.ToJsMethodName);
            }

            if (propertyType.IsPeriod())
            {
                return sourcePropertyRef.UnwrapObservablePeriodPropertyToPeriod();
            }

            return sourcePropertyRef.UnwrapObservableProperty();
        }

        if (propertyType.IsDateTime())
        {
            return sourcePropertyRef.ConvertToDate();
        }

        if (propertyType.IsPeriod())
        {
            return sourcePropertyRef.ConvertToPeriod();
        }

        if (!propertyType.IsPrimitiveJsType())
        {
            return reference.ToTypeReferenceExpression().ToMethodInvokeExpression(Constants.FromJsMethodName, sourcePropertyRef);
        }

        return sourcePropertyRef;
    }

    private CodeStatement GetSimpleAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        var expression = this.GetSimpleExpression(property.PropertyType, sourcePropertyRef, this.CodeTypeReferenceService.GetCodeTypeReference(property));

        if (!this.unwrapObservable)
        {
            return expression.ToAssignStatement(targetPropertyRef);
        }

        if (!this.Configuration.DomainTypes.Contains(property.PropertyType))
        {
            return expression.ToAssignStatement(targetPropertyRef);
        }

        var unwrapExpression = sourcePropertyRef.UnwrapObservableProperty();
        return new IsDefinedConditionStatement(unwrapExpression)
               {
                       TrueStatements =
                       {
                               expression.ToAssignStatement(targetPropertyRef)
                       }
               };
    }
}
