using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class ClonePropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        private readonly CodeParameterDeclarationExpression _copyIdParameter;


        public ClonePropertyAssigner(IDTOSource<TConfiguration> source, CodeParameterDeclarationExpression copyIdParameter)
            : base(source)
        {
            this._copyIdParameter = copyIdParameter;
        }


        public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            if (this._copyIdParameter != null && this.Configuration.IsIdentityProperty(property))
            {
                return new CodeConditionStatement
                {
                    Condition = this._copyIdParameter.ToVariableReferenceExpression(),
                    TrueStatements =
                    {
                        sourcePropertyRef.ToAssignStatement(targetPropertyRef)
                    }
                };
            }

            if (this.Configuration.ClassTypes.Contains(property.PropertyType))
            {
                return new CodeNotNullConditionStatement(sourcePropertyRef)
                {
                    TrueStatements =
                    {
                        this.Configuration
                            .GetCodeTypeReference(property.PropertyType, ClientFileType.Class)
                            .ToObjectCreateExpression(sourcePropertyRef)
                            .ToAssignStatement(targetPropertyRef)
                    }
                };
            }

            if (property.PropertyType.IsArray)
            {
                return new CodeNotNullConditionStatement(sourcePropertyRef)
                {
                    TrueStatements =
                    {
                        typeof(ArrayExtensions).ToTypeReferenceExpression()
                            .ToMethodInvokeExpression("CloneA", sourcePropertyRef)
                            .ToAssignStatement(targetPropertyRef)
                    }
                };
            }

            if (this.Configuration.IsCollectionProperty(property))
            {
                var lambdaMethodRef = typeof(Framework.Core.EnumerableExtensions)
                                      .ToTypeReferenceExpression()
                                      .ToMethodReferenceExpression("To" + this.CodeTypeReferenceService.CollectionType.Name.TakeWhileNot("`"));


                var elementType = property.PropertyType.GetCollectionOrArrayElementType();

                var copyLambda = new CodeParameterDeclarationExpression { Name = elementType.Name.ToStartLowerCase() }.Pipe(param =>
                {
                    var paramRef = param.ToVariableReferenceExpression();


                    return new CodeLambdaExpression
                    {
                        Parameters = { param },
                        Statements =
                        {
                            elementType == typeof(string)                ? (CodeExpression)paramRef
                                         : this._copyIdParameter == null ? paramRef.ToMethodInvokeExpression("Clone")
                                                                         : paramRef.ToMethodInvokeExpression("Clone", this._copyIdParameter.ToVariableReferenceExpression())
                        }
                    };
                });

                var copyExpr = sourcePropertyRef.ToStaticMethodInvokeExpression(lambdaMethodRef, copyLambda)
                                                .ToAssignStatement(targetPropertyRef);


                return new CodeNotNullConditionStatement(sourcePropertyRef)
                {
                    TrueStatements =
                    {
                       copyExpr
                    }
                };
            }

            return sourcePropertyRef.ToAssignStatement(targetPropertyRef);
        }
    }
}
