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
    /// Class property assigner
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class ClassPropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public ClassPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
        {
        }

        public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            if (property.PropertyType.IsCollectionOrArray())
            {
                return this.ArrayMapExpression(property, sourcePropertyRef, targetPropertyRef);
            }

            return this.GetSimpleAssignStatement(property, sourcePropertyRef, targetPropertyRef);
        }

        protected CodeStatement ArrayMapExpression(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            var elementType = property.PropertyType.GetCollectionOrArrayElementType();
            var typeReference = this.Configuration.IsCollectionProperty(property)
                ? this.Configuration.GetCodeTypeReference(property.PropertyType, ClientFileType.Class).TypeArguments[0]
                : new CodeTypeReference(elementType.FullName);

            var invokeExpression = elementType.IsPrimitiveJsType() ? new CodeSnippetExpression(Constants.DefaultVariableName) : typeReference.NormalizeTypeReference(elementType).ToTypeReferenceExpression().ToMethodInvokeExpression(Constants.FromJsMethodName, new CodeSnippetExpression(Constants.DefaultVariableName)) as CodeExpression;

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
                    sourceExpression.ToMethodInvokeExpression("map", callbackExpression).ToAssignStatement(targetPropertyRef)
                }
            };
        }

        private CodeStatement GetSimpleAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            if (property.IsDateTime())
            {
                return sourcePropertyRef.ConvertToDate().ToAssignStatement(targetPropertyRef);
            }

            if (property.IsPeriod())
            {
                return sourcePropertyRef.ConvertToPeriod().ToAssignStatement(targetPropertyRef);
            }

            if (!property.IsPrimitiveJsType())
            {
                var type = this.Configuration.GetCodeTypeReference(property.PropertyType, ClientFileType.Class);
                return type.ToTypeReferenceExpression().ToMethodInvokeExpression(Constants.FromJsMethodName, sourcePropertyRef).ToAssignStatement(targetPropertyRef);
            }

            return sourcePropertyRef.ToAssignStatement(targetPropertyRef);
        }
    }
}
