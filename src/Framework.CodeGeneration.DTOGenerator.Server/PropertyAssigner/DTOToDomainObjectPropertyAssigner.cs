using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DTOToDomainObjectPropertyAssigner<TConfiguration> : ServerPropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DTOToDomainObjectPropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
    {
    }


    public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var isFixReferencePropertyMode = property.IsFixReference(this.FileType.Role);

        var getToDomainObjectExpr =
            (CodeExpression expr, Type type, bool isDetail) =>
            {
                var methodName = "To" + type.Name;

                if (this.Configuration.Environment.ExtendedMetadata.HasAttribute<AutoMappingAttribute>(property, attr => !attr.Enabled))
                {
                    if (this.Configuration.IsPersistentObject(type))
                    {
                        return this.MappingServiceRefExpr.ToMethodInvokeExpression(
                            methodName,
                            expr.ToPropertyReference(this.Configuration.DTOIdentityPropertyName));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                if (isDetail)
                {
                    if (this.Configuration.IsPersistentObject(this.DomainType))
                    {
                        return this.MappingServiceRefExpr.ToMethodInvokeExpression(
                            methodName,
                            expr,
                            this.DomainParameter.ToVariableReferenceExpression());
                    }
                    else if (this.Configuration.CanCreateDomainObject(property, type, this.FileType))
                    {
                        return this.MappingServiceRefExpr.ToMethodInvokeExpression(methodName, expr, true.ToPrimitiveExpression());
                    }
                }

                return this.MappingServiceRefExpr.ToMethodInvokeExpression(methodName, expr);
            };

        if (this.Configuration.IsReferenceProperty(property))
        {
            if (this.Configuration.IsPersistentObject(property.PropertyType))
            {
                var propertyTypeRefExpr = this.CodeTypeReferenceService.GetCodeTypeReference(property);

                if (isFixReferencePropertyMode)
                {
                    throw new NotImplementedException("Not implement generation mapping for reference properties with FixReference attributes");
                }



                return new CodeConditionStatement
                       {
                               Condition = new CodeObjectEqualsExpression(sourcePropertyRef, propertyTypeRefExpr.ToDefaultValueExpression()).ToNegateExpression(),

                               TrueStatements =
                               {
                                       getToDomainObjectExpr(sourcePropertyRef, property.PropertyType, property.IsDetail()).ToAssignStatement(targetPropertyRef)
                               },

                               FalseStatements =
                               {
                                       new CodePrimitiveExpression(null).ToAssignStatement(targetPropertyRef)
                               }
                       };
            }
            else
            {
                return new CodeNotNullConditionStatement(sourcePropertyRef)
                       {
                               TrueStatements =
                               {
                                       getToDomainObjectExpr (sourcePropertyRef, property.PropertyType, property.IsDetail()).ToAssignStatement(targetPropertyRef)
                               },
                               FalseStatements =
                               {
                                       new CodePrimitiveExpression(null).ToAssignStatement(targetPropertyRef)
                               }
                       };
            }
        }
        else if (this.Configuration.IsCollectionProperty(property))
        {
            var elementType = property.PropertyType.GetCollectionElementType();

            if (!this.IsPersistent() && property.HasSetMethod())
            {
                if (isFixReferencePropertyMode)
                {
                    throw new NotImplementedException("Not implement generation mapping for details with FixReference attributes for not persistent root object");
                }

                return new CodeNotNullConditionStatement(sourcePropertyRef)
                       {
                               TrueStatements =
                               {
                                       typeof(CoreEnumerableExtensions).ToTypeReferenceExpression()
                                                                                  .ToMethodInvokeExpression(
                                                                                   "ToList",
                                                                                   sourcePropertyRef,
                                                                                   new CodeParameterDeclarationExpression { Name = "v" }.Pipe(lamdaParam => new CodeLambdaExpression
                                                                                       {
                                                                                               Parameters = { lamdaParam },
                                                                                               Statements = { getToDomainObjectExpr(lamdaParam.ToVariableReferenceExpression(), elementType, property.IsDetail()) }
                                                                                       })).ToAssignStatement(targetPropertyRef)
                               }
                       };
            }
            else
            {
                if (isFixReferencePropertyMode)
                {
                    return this.GetFixReferenceMappingStatement(property, sourcePropertyRef, targetPropertyRef);
                }

                var transferElementTypeRef = this.Configuration.GetCodeTypeReference(elementType, this.CodeTypeReferenceService.GetCollectionFileType(property));

                var createDetailLambda = new CodeParameterDeclarationExpression { Name = "detailDTO" }.Pipe(lambdaParam => new CodeLambdaExpression
                    {
                            Parameters = { lambdaParam },
                            Statements = { getToDomainObjectExpr(lambdaParam.ToVariableReferenceExpression(), elementType, true) }
                    });

                var removeDetailLambda = this.Configuration.UseRemoveMappingExtension

                                                 ? (CodeExpression)new CodeParameterDeclarationExpression { Name = "detail" }.Pipe(lamdaParam => new CodeLambdaExpression
                                                     {
                                                             Parameters = { lamdaParam },
                                                             Statements =  { typeof(AddRemoveDetailHelper).ToTypeReferenceExpression()
                                                                                     .ToMethodReferenceExpression("RemoveDetail", property.DeclaringType.ToTypeReference(), elementType.ToTypeReference())
                                                                                     .ToMethodInvokeExpression(this.DomainParameter.ToVariableReferenceExpression(), lamdaParam.ToVariableReferenceExpression())
                                                                           }
                                                     })

                                                 : this.DomainParameter.ToVariableReferenceExpression().ToPropertyReference("Remove" + elementType.Name);

                return new CodeNotNullConditionStatement(sourcePropertyRef)
                       {
                               TrueStatements =
                               {
                                       this.GetCollectionMappingMethodReferenceExpression(transferElementTypeRef, elementType)
                                           .ToMethodInvokeExpression(createDetailLambda, removeDetailLambda)
                                           .ToMethodInvokeExpression("Map", sourcePropertyRef, targetPropertyRef)
                               }
                       };
            }
        }

        if (this.Configuration.CheckVersion && property.GetCustomAttributes<VersionAttribute>().Any())
        {
            return this.MappingServiceRefExpr.ToPropertyReference("VersionService")
                       .ToMethodInvokeExpression("GetVersion", sourcePropertyRef, this.DomainParameter.ToVariableReferenceExpression())
                       .ToAssignStatement(targetPropertyRef);
        }

        return sourcePropertyRef.ToAssignStatement(targetPropertyRef);
    }

    protected virtual CodeMethodReferenceExpression GetCollectionMappingMethodReferenceExpression(CodeTypeReference transferElementTypeRef, Type elementType)
    {
        return this.MappingServiceRefExpr.ToMethodReferenceExpression(
                                                                      "GetCollectionMappingService",
                                                                      transferElementTypeRef,
                                                                      elementType.ToTypeReference());
    }

    private CodeStatement GetFixReferenceMappingStatement(PropertyInfo property, CodeExpression sourcePropertyRef,
                                                          CodeExpression targetPropertyRef)
    {
        var enumerableExpression = typeof(Enumerable).ToTypeReferenceExpression();

        Func<string, CodeMethodInvokeExpression> getEnumMethodFunc = z => enumerableExpression.ToMethodInvokeExpression(z);
        var identityExpression = new CodeSnippetExpression($"s => s.{this.Configuration.Environment.IdentityProperty.Name}");

        var targetSelector = getEnumMethodFunc("Select");
        targetSelector.Parameters.Add(targetPropertyRef);
        targetSelector.Parameters.Add(identityExpression);

        var sourceSelector = getEnumMethodFunc("Select");
        sourceSelector.Parameters.Add(sourcePropertyRef);
        sourceSelector.Parameters.Add(identityExpression);

        var intersect = getEnumMethodFunc("Intersect");
        intersect.Parameters.Add(targetSelector);
        intersect.Parameters.Add(sourceSelector);

        var actualCountExpression = getEnumMethodFunc("Count");
        actualCountExpression.Parameters.Add(intersect);

        var expectedCountExpression = getEnumMethodFunc("Count");
        expectedCountExpression.Parameters.Add(targetPropertyRef);

        var fixReferenceValidationStatement =
                new CodeConditionStatement(
                                           new CodeNegateExpression(new CodeValueEqualityOperatorExpression(actualCountExpression,
                                                                        expectedCountExpression)));


        var throwStatement = new CodeThrowExceptionStatement(
                                                             new CodeObjectCreateExpression(
                                                              this.Configuration.ExceptionType,
                                                              new CodePrimitiveExpression(
                                                               $"{property.Name} property of {this.DomainType.Name} can not modified")));

        fixReferenceValidationStatement.TrueStatements.Add(throwStatement);



        //target.Children.Join(this.Children, z => z.Id, z => z.Id,
        //         (domainItem, dtoItem) => new { domainItem, dtoItem }).Foreach(z => z.dtoItem.MapToDomainObject(context, z.domainItem));

        var joinStatement = getEnumMethodFunc("Join");
        joinStatement.Parameters.Add(targetPropertyRef);
        joinStatement.Parameters.Add(sourcePropertyRef);
        joinStatement.Parameters.Add(identityExpression);
        joinStatement.Parameters.Add(identityExpression);
        joinStatement.Parameters.Add(new CodeSnippetExpression("(domainItem, dtoItem)=>new{domainItem, dtoItem}"));

        var foreachStatement =
                typeof(CommonFramework.EnumerableExtensions)
                        .ToTypeReferenceExpression()
                        .ToMethodInvokeExpression("Foreach");

        foreachStatement.Parameters.Add(joinStatement);
        foreachStatement.Parameters.Add(
                                        new CodeSnippetExpression(
                                                                  $"item => item.dtoItem.{this.Configuration.MapToDomainObjectMethodName}({"this"}, item.domainItem)"));

        var statement = new CodeNotNullConditionStatement(sourcePropertyRef);

        statement.TrueStatements.Add(fixReferenceValidationStatement);
        statement.TrueStatements.Add(foreachStatement);




        return statement;
    }
}
