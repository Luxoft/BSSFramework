using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner
{
    /// <summary>
    /// Observable To strict property assigner
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class ObservableToStrictPropertyAssigner<TConfiguration> : DTOGenerator.MainToStrictPropertyAssigner<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public ObservableToStrictPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
        {
        }

        public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            if (this.Configuration.IsReferenceProperty(property))
            {
                if (this.Configuration.IsPersistentObject(property.PropertyType) && !property.IsDetail())
                {
                    var identityTypeRef = this.Configuration.GetCodeTypeReference(property.PropertyType, DTOGenerator.FileType.IdentityDTO);

                    return new IsDefinedConditionStatement(sourcePropertyRef)
                    {
                        TrueStatements =
                        {
                            sourcePropertyRef.ToPropertyReference(this.Configuration.DTOIdentityPropertyName).ToAssignStatement(targetPropertyRef)
                        },
                        FalseStatements =
                        {
                            this.Configuration.IdentityIsReference
                          ? identityTypeRef.ToDefaultValueExpression().ToAssignStatement(targetPropertyRef)
                          : identityTypeRef.ToTypeReferenceExpression().ToFieldReference(this.Configuration.DTOEmptyPropertyName).ToAssignStatement(targetPropertyRef)
                        }
                    };
                }

                var name = "To" + this.DomainType.Name.SkipLast(Constants.DTOName, true);

                return new IsDefinedConditionStatement(sourcePropertyRef)
                       {
                           TrueStatements =
                           {
                               sourcePropertyRef.ToMethodInvokeExpression(name).ToAssignStatement(targetPropertyRef)
                           },
                           FalseStatements =
                           {
                               new CodePrimitiveExpression(null).ToAssignStatement(targetPropertyRef)
                           }
                       };
            }

            if (this.Configuration.ClassTypes.Contains(property.PropertyType))
            {
                return new IsDefinedConditionStatement(sourcePropertyRef)
                {
                    TrueStatements =
                    {
                            this.Configuration.GetCodeTypeReference(property.PropertyType, ClientFileType.Class)
                                          .ToObjectCreateExpression(sourcePropertyRef)
                                          .ToAssignStatement(targetPropertyRef)
                    }
                };
            }

            if (this.Configuration.IsCollectionProperty(property))
            {
                var elementFileType = this.CodeTypeReferenceService.GetCollectionFileType(property);

                var elementType = property.PropertyType.GetCollectionOrArrayElementType();

                var lambda = new CodeParameterDeclarationExpression { Name = elementType.Name.ToStartLowerCase() }.Pipe(param =>
                {
                    var paramRef = param.ToVariableReferenceExpression();

                    CodeExpression body;
                    if (elementFileType == DTOGenerator.FileType.StrictDTO)
                    {
                        var name = "To" + this.DomainType.Name.SkipLast(Constants.DTOName, true);
                        body = paramRef.ToMethodInvokeExpression(name);
                    }
                    else
                    {
                        body = paramRef.ToPropertyReference(this.Configuration.DTOIdentityPropertyName);
                    }

                    return new CodeLambdaExpression
                           {
                               Parameters = { param },
                               Statements = { body }
                           };
                });

                var selectMethod = typeof(Core.EnumerableExtensions)
                    .ToTypeReferenceExpression()
                    .ToMethodReferenceExpression("To" + this.Configuration.CollectionType.Name.TakeWhileNot("`"));
                var referenceFileType = this.CodeTypeReferenceService.GetReferenceFileType(property);

                CodeTypeReference resultCodeTypeReference;
                CodeTypeReference paramCodeTypeReference;

                if (referenceFileType == DTOGenerator.FileType.StrictDTO)
                {
                    resultCodeTypeReference = this.CodeTypeReferenceService.GetCodeTypeReference(property).TypeArguments[0];
                    paramCodeTypeReference = this.Configuration.GetCodeTypeReference(elementType, DTOGenerator.FileType.RichDTO);
                }
                else
                {
                    resultCodeTypeReference = this.Configuration.GetCodeTypeReference(elementType, DTOGenerator.FileType.IdentityDTO);
                    paramCodeTypeReference = this.Configuration.GetCodeTypeReference(elementType, DTOGenerator.FileType.SimpleDTO);
                }

                selectMethod.TypeArguments.Add(paramCodeTypeReference);
                selectMethod.TypeArguments.Add(resultCodeTypeReference);

                var assignStatement = sourcePropertyRef.ToStaticMethodInvokeExpression(selectMethod, lambda)
                                                       .ToAssignStatement(targetPropertyRef);

                return new IsDefinedConditionStatement(sourcePropertyRef)
                       {
                           TrueStatements =
                           {
                               assignStatement
                           }
                       };
            }

            return base.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
        }
    }
}
