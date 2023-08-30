using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;
using Framework.Security;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator;

public class UpdatePropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public UpdatePropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
    {
    }

    public CodeExpression MappingServiceRefExpr => new CodeThisReferenceExpression();

    public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

        if (this.Configuration.IsCollectionProperty(property))
        {
            return this.GetCollectionAssignStatement(
                                                     property,
                                                     sourcePropertyRef,
                                                     targetPropertyRef,
                                                     property.IsSecurity() ? "ExtractSecurityUpdateDataFromSingle" : "ExtractUpdateDataFromSingle");
        }
        else if (this.Configuration.IsIdentityOrVersionProperty(property) || property.IsSecurity())
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

        var sourceElementFileType = this.Configuration.GetLayerCodeTypeReferenceService(DTOGenerator.FileType.StrictDTO).GetCollectionFileType(property);

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
