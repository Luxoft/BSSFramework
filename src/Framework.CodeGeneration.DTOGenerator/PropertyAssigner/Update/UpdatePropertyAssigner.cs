using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.Application.Domain.Attributes;
using Framework.BLL.Domain.Dto;
using Framework.BLL.Domain.Extensions;
using Framework.CodeDom;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.Core;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner.Update;

public class UpdatePropertyAssigner<TConfiguration>(IDTOSource<TConfiguration> source) : PropertyAssigner<TConfiguration>(source)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public CodeExpression MappingServiceRefExpr => new CodeThisReferenceExpression();

    public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

        var isSecurity = this.Configuration.Environment.ExtendedMetadata.GetProperty(property).IsSecurity();

        if (this.Configuration.IsCollectionProperty(property))
        {
            return this.GetCollectionAssignStatement(
                                                     property,
                                                     sourcePropertyRef,
                                                     targetPropertyRef,
                                                     isSecurity ? "ExtractSecurityUpdateDataFromSingle" : "ExtractUpdateDataFromSingle");
        }
        else if (this.Configuration.IsIdentityOrVersionProperty(property) || isSecurity)
        {
            return sourcePropertyRef.ToAssignStatement(targetPropertyRef);
        }
        else if (this.Configuration.IsReferenceProperty(property) && property.IsDetail())
        {
            return this.Configuration.GetCreateUpdateDTOExpression(property.PropertyType, sourcePropertyRef,  null, this.MappingServiceRefExpr)
                       .ToMaybeReturnExpression()
                       .ToAssignStatement(targetPropertyRef);
        }
        else
        {
            return sourcePropertyRef.ToMaybeReturnExpression().ToAssignStatement(targetPropertyRef);
        }
    }

    private CodeStatement GetCollectionAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef, string extractMethodName)
    {
        var elementType = property.PropertyType.GetCollectionOrArrayElementType();

        var targetElementFileType = this.CodeTypeReferenceService.GetCollectionFileType(property);

        var targetElementIdentityTypeRef = this.Configuration.GetCodeTypeReference(elementType, DTOType.IdentityDTO);

        var targetElementTypeRef = this.Configuration.GetCodeTypeReference(elementType, targetElementFileType);

        var sourceElementFileType = this.Configuration.GetLayerCodeTypeReferenceService(BaseFileType.StrictDTO).GetCollectionFileType(property);

        var sourceElementTypeRef = this.Configuration.GetCodeTypeReference(elementType, sourceElementFileType);


        var lambda = new CodeParameterDeclarationExpression { Name = elementType.Name.ToStartLowerCase() }.Pipe(
         param =>
         {
             var paramRef = param.ToVariableReferenceExpression();

             var body = this.Configuration.GetCreateUpdateDTOExpression(elementType, paramRef, null, this.MappingServiceRefExpr);

             return new CodeLambdaExpression
                    {
                            Parameters = { param },
                            Statements = { body }
                    };
         });

        var extractMethodExpr = this.MappingServiceRefExpr.ToMethodReferenceExpression(extractMethodName, sourceElementTypeRef, targetElementIdentityTypeRef, targetElementTypeRef)
                                    .ToMethodInvokeExpression(sourcePropertyRef, lambda);

        return extractMethodExpr.ToAssignStatement(targetPropertyRef);
    }
}
