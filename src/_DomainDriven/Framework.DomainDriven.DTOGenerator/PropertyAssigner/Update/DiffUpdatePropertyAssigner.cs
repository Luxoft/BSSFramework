using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Security;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator;

public class DiffUpdatePropertyAssigner<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IDiffUpdatePropertyAssigner
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public DiffUpdatePropertyAssigner(IDTOSource<TConfiguration> source)
            : this(source.FromMaybe(() => new ArgumentOutOfRangeException(nameof(source))).Configuration, source.DomainType)
    {
    }


    public DiffUpdatePropertyAssigner(TConfiguration configuration, Type domainType)
            : base(configuration)
    {
        this.DomainType = domainType ?? throw new ArgumentNullException(nameof(domainType));

        this.UpdateCodeTypeReferenceService = this.Configuration.GetLayerCodeTypeReferenceService(DTOGenerator.FileType.UpdateDTO);

        this.StrictCodeTypeReferenceService = this.Configuration.GetLayerCodeTypeReferenceService(DTOGenerator.FileType.StrictDTO);
    }


    public Type DomainType { get; }

    public DTOFileType FileType { get; } = DTOGenerator.FileType.UpdateDTO;

    protected ILayerCodeTypeReferenceService UpdateCodeTypeReferenceService { get; }

    protected ILayerCodeTypeReferenceService StrictCodeTypeReferenceService { get; }

    public CodeExpression MappingServiceRefExpr => new CodeThisReferenceExpression();

    public CodeStatement GetAssignStatement(
            PropertyInfo property,
            CodeExpression baseSourcePropertyRef,
            CodeExpression currentSourcePropertyRef,
            CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (baseSourcePropertyRef == null) throw new ArgumentNullException(nameof(baseSourcePropertyRef));
        if (currentSourcePropertyRef == null) throw new ArgumentNullException(nameof(currentSourcePropertyRef));
        if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

        if (this.Configuration.IsIdentityOrVersionProperty(property))
        {
            return this.MappingServiceRefExpr.ToMethodInvokeExpression(
                                                                       "GetEqualsValue",
                                                                       currentSourcePropertyRef, 
                                                                       baseSourcePropertyRef,
                                                                       property.Name.ToPrimitiveExpression())
                       .ToAssignStatement(targetPropertyRef);
        }
        else if (this.Configuration.IsCollectionProperty(property))
        {
            //if (property.IsSecurity())
            //{
            //    throw new NotImplementedException();
            //}
            //else
            {
                var elementType = property.PropertyType.GetCollectionElementType();

                var elementSourceFileType = this.StrictCodeTypeReferenceService.GetCollectionFileType(property);

                var elementSourceTypeRef = this.Configuration.GetCodeTypeReference(elementType, elementSourceFileType);

                var elementIdentityTypeRef = this.Configuration.GetCodeTypeReference(elementType, DTOType.IdentityDTO);

                var elementTargetFileType = this.UpdateCodeTypeReferenceService.GetCollectionFileType(property);

                var elementTargetTypeRef = this.Configuration.GetCodeTypeReference(elementType, elementTargetFileType);

                var extractMethod = this.MappingServiceRefExpr
                                        .ToMethodReferenceExpression(property.IsSecurity() ? "ExtractSecurityUpdateDataL" : "ExtractUpdateDataL",
                                                                     elementSourceTypeRef,
                                                                     elementIdentityTypeRef,
                                                                     elementTargetTypeRef);

                var currentSourceItemParam = new CodeParameterDeclarationExpression { Name = "currentSourceItem" };
                var baseSourceItemParam = new CodeParameterDeclarationExpression { Name = "baseSourceItem" };

                var toPairElementLambda = new CodeLambdaExpression
                                          {
                                                  Parameters = { currentSourceItemParam, baseSourceItemParam },
                                                  Statements =
                                                  {
                                                          this.Configuration.GetCreateUpdateDTOExpression(
                                                           elementType,
                                                           currentSourceItemParam.ToVariableReferenceExpression(),
                                                           baseSourceItemParam.ToVariableReferenceExpression(),
                                                           this.MappingServiceRefExpr)
                                                  }
                                          };

                var toSingleElementLambda = new CodeLambdaExpression
                                            {
                                                    Parameters = { currentSourceItemParam },
                                                    Statements =
                                                    {
                                                            this.Configuration.GetCreateUpdateDTOExpression(
                                                             elementType, 
                                                             currentSourceItemParam.ToVariableReferenceExpression(),
                                                             null,
                                                             this.MappingServiceRefExpr)
                                                    }
                                            };

                return extractMethod.ToMethodInvokeExpression(currentSourcePropertyRef, baseSourcePropertyRef, toPairElementLambda, toSingleElementLambda).ToAssignStatement(targetPropertyRef);
            }
        }
        else if (this.Configuration.IsReferenceProperty(property) && property.IsDetail())
        {
            return this.Configuration
                       .GetCreateUpdateDTOExpression(property.PropertyType, currentSourcePropertyRef, baseSourcePropertyRef, this.MappingServiceRefExpr)
                       .ToMaybeReturnExpression()
                       .ToAssignStatement(targetPropertyRef);
        }
        else
        {
            if (property.IsSecurity())
            {
                return new CodeConditionStatement
                       {
                               Condition = new CodeBinaryOperatorExpression(baseSourcePropertyRef, CodeBinaryOperatorType.ValueEquality, currentSourcePropertyRef).ToNegateExpression(),
                               TrueStatements =
                               {
                                       currentSourcePropertyRef.ToAssignStatement(targetPropertyRef)
                               }
                       };
            }
            else
            {
                return new CodeConditionStatement
                       {
                               Condition = new CodeBinaryOperatorExpression(baseSourcePropertyRef, CodeBinaryOperatorType.ValueEquality, currentSourcePropertyRef).ToNegateExpression(),
                               TrueStatements =
                               {
                                       currentSourcePropertyRef.ToMaybeReturnExpression().ToAssignStatement(targetPropertyRef)
                               }
                       };
            }
        }
    }
}
