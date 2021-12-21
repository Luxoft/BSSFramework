using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner
{
    /// <summary>
    /// Visual observable property assigner
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class VisualObservablePropertyAssigner<TConfiguration> : MainPropertyAssigner<TConfiguration>
          where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
      {
          public VisualObservablePropertyAssigner(IDTOSource<TConfiguration> source)
              : base(source)
          {
          }

          protected override CodeStatement MapArrayExpression(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
          {
              var type = property.PropertyType.GetCollectionOrArrayElementType();
              var reference = this.CodeTypeReferenceService.GetCodeTypeReference(property);

              var typeReference = reference?.TypeArguments.Count > 0
                                      ? reference.TypeArguments[0].CheckForModuleReference(this.Configuration).NormalizeTypeReference(type)
                                      : base.CodeTypeReferenceService.GetCodeTypeReferenceByType(type).CheckForModuleReference(this.Configuration).NormalizeTypeReference(type);

              var invokeExpression = this.GetSimpleExpression(type, new CodeSnippetExpression(Constants.DefaultVariableName), typeReference);

              var lambdaExpression = new CodeLambdaExpression
                                     {
                                         Parameters =
                                             new CodeParameterDeclarationExpressionCollection(
                                                 new[] { new CodeParameterDeclarationExpression(Constants.VoidType, Constants.DefaultVariableName) }),
                                         Statements = { invokeExpression }
                                     };

              CodeExpression sourceExpression = sourcePropertyRef;

              return new IsDefinedConditionStatement(sourceExpression)
                     {
                         TrueStatements =
                         {
                             new CodeDelegateInvokeExpression(targetPropertyRef, sourceExpression.ToMethodInvokeExpression("map", lambdaExpression))
                         }
                     };
          }

          public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
          {
              if (property.PropertyType.IsCollectionOrArray())
              {
                  return this.MapArrayExpression(property, sourcePropertyRef, targetPropertyRef);
              }

              if (!property.IsPrimitiveJsType())
              {
                  var typeReferenceExpression = base.CodeTypeReferenceService.GetCodeTypeReference(property).ToTypeReferenceExpression();

                  return new CodeDelegateInvokeExpression(
                      targetPropertyRef,
                      typeReferenceExpression.ToMethodInvokeExpression(Constants.FromJsMethodName, sourcePropertyRef)).ToExpressionStatement();
              }

              return new CodeDelegateInvokeExpression(targetPropertyRef, this.GetReferenceExpression(property.PropertyType, sourcePropertyRef))
                      .ToExpressionStatement();
          }

          private CodeExpression GetReferenceExpression(System.Type property, CodeExpression expression)
          {
              if (property.IsDateTime())
              {
                  expression = expression.ConvertToDate();
              }

              if (property.IsPeriod())
              {
                  expression = expression.ConvertToObservablePeriod();
              }

              return expression;
          }

          private CodeExpression GetSimpleExpression(System.Type propertyType, CodeExpression sourcePropertyRef, CodeTypeReference reference)
          {
              if (propertyType.IsPrimitiveJsType())
              {
                  return this.GetReferenceExpression(propertyType, sourcePropertyRef);
              }

              return !propertyType.IsPrimitiveJsType()
                         ? reference.ToTypeReferenceExpression().ToMethodInvokeExpression(Constants.FromJsMethodName, sourcePropertyRef)
                         : sourcePropertyRef;
          }
      }
}
