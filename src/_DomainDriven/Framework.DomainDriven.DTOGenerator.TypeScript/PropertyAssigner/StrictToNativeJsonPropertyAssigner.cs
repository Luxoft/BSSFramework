using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner
{
    /// <summary>
    /// Strict To nativeJson property assigner
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class StrictToNativeJsonPropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public StrictToNativeJsonPropertyAssigner(IDTOSource<TConfiguration> source)
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

                CodeExpression expression = new CodeSnippetExpression(Constants.DefaultVariableName);

                expression = !elementType.IsPrimitiveJsType() ? expression.ToMethodInvokeExpression(Constants.ToNativeJsonMethodName) : this.GetPrimitiveExpression(elementType, expression);

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
                return new IsDefinedConditionStatement(sourcePropertyRef)
                {
                    TrueStatements =
                           {
                               new CodeMethodInvokeExpression(sourcePropertyRef, Constants.ToNativeJsonMethodName).ToAssignStatement(targetPropertyRef)
                           }
                };
            }

            return this.GetPrimitiveExpression(property.PropertyType, sourcePropertyRef).ToAssignStatement(targetPropertyRef);
        }

        private CodeExpression GetPrimitiveExpression(Type propertyType, CodeExpression sourcePropertyRef)
        {
            if (propertyType.IsDateTime())
            {
                return sourcePropertyRef.ConvertDateToOData();
            }

            if (propertyType.IsPeriod())
            {
                return sourcePropertyRef.ConvertPeriodToOData();
            }

            return sourcePropertyRef;
        }
    }
}
