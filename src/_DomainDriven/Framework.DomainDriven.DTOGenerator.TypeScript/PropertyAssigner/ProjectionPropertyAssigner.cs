using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner
{
    /// <summary>
    /// ProjectionPropertyAssigner property assigner
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class ProjectionPropertyAssigner<TConfiguration> : MainPropertyAssigner<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public ProjectionPropertyAssigner(IDTOSource<TConfiguration> source)
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

            var typeReference = this.Configuration.GetCodeTypeReference(elementType, ClientFileType.ProjectionDTO);

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
                    sourceExpression.ToMethodInvokeExpression(Constants.MapMethodName, callbackExpression).ToAssignStatement(targetPropertyRef)
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
                var type = this.Configuration.GetCodeTypeReference(property.PropertyType, DTOType.ProjectionDTO);
                return type.ToTypeReferenceExpression().ToMethodInvokeExpression(Constants.FromJsMethodName, sourcePropertyRef).ToAssignStatement(targetPropertyRef);
            }

            return sourcePropertyRef.ToAssignStatement(targetPropertyRef);
        }
    }
}
