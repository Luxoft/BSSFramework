using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator;

public class MainToStrictPropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public MainToStrictPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
    {
    }

    public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

        if (this.Configuration.IsReferenceProperty(property))
        {
            if (this.Configuration.IsPersistentObject(property.PropertyType) && !property.IsDetail())
            {
                var identityTypeRef = this.Configuration.GetCodeTypeReference(property.PropertyType, DTOGenerator.FileType.IdentityDTO);

                return new CodeNotNullConditionStatement(sourcePropertyRef)
                       {
                               TrueStatements =
                               {
                                       sourcePropertyRef.ToPropertyReference(this.Configuration.DTOIdentityPropertyName)
                                                        .ToAssignStatement(targetPropertyRef)
                               },
                               FalseStatements =
                               {
                                       this.Configuration.IdentityIsReference

                                               ? identityTypeRef.ToDefaultValueExpression()
                                                                .ToAssignStatement(targetPropertyRef)

                                               : identityTypeRef.ToTypeReferenceExpression()
                                                                .ToFieldReference(this.Configuration.DTOEmptyPropertyName)
                                                                .ToAssignStatement(targetPropertyRef)
                               }
                       };
            }
            else
            {
                var strictRefType = this.CodeTypeReferenceService.GetCodeTypeReference(property);

                return new CodeNotNullConditionStatement(sourcePropertyRef)
                       {
                               TrueStatements =
                               {
                                       strictRefType.ToObjectCreateExpression(sourcePropertyRef)
                                                    .ToAssignStatement(targetPropertyRef)
                               },
                               FalseStatements =
                               {
                                       new CodePrimitiveExpression(null).ToAssignStatement(targetPropertyRef)
                               }
                       };
            }
        }
        if (property.PropertyType.IsArray)
        {
            return new CodeNotNullConditionStatement(sourcePropertyRef)
                   {
                           TrueStatements =
                           {
                                   typeof(Enumerable).ToTypeReferenceExpression()
                                                     .ToMethodInvokeExpression("ToArray", sourcePropertyRef)
                                                     .ToAssignStatement(targetPropertyRef)
                           },
                           FalseStatements =
                           {
                                   new CodePrimitiveExpression(null).ToAssignStatement(targetPropertyRef)
                           }
                   };
        }
        else if (this.Configuration.IsCollectionProperty(property))
        {
            var elementFileType = this.CodeTypeReferenceService.GetCollectionFileType(property);

            var elementType = property.PropertyType.GetCollectionOrArrayElementType();

            var lambda = new CodeParameterDeclarationExpression { Name = elementType.Name.ToStartLowerCase() }.Pipe(param =>
            {
                var paramRef = param.ToVariableReferenceExpression();

                var body = elementFileType == DTOGenerator.FileType.StrictDTO

                                   ? (CodeExpression)this.Configuration
                                                         .GetCodeTypeReference(elementType, DTOGenerator.FileType.StrictDTO)
                                                         .ToObjectCreateExpression(paramRef)

                                   : paramRef.ToPropertyReference(this.Configuration.DTOIdentityPropertyName);

                return new CodeLambdaExpression
                       {
                               Parameters = { param },
                               Statements = { body }
                       };
            });

            var selectMethod = typeof(Core.EnumerableExtensions)
                               .ToTypeReferenceExpression()
                               .ToMethodReferenceExpression("To" + this.Configuration.CollectionType.Name.TakeWhileNot("`"));

            var assignStatement = sourcePropertyRef.ToStaticMethodInvokeExpression(selectMethod, lambda)
                                                   .ToAssignStatement(targetPropertyRef);


            return new CodeNotNullConditionStatement(sourcePropertyRef)
                   {
                           TrueStatements =
                           {
                                   assignStatement
                           }
                   };
        }


        return sourcePropertyRef.ToAssignStatement(targetPropertyRef);
    }
}
