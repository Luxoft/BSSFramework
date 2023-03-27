using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DomainObjectToDTOPropertyAssigner<TConfiguration> : ServerPropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DomainObjectToDTOPropertyAssigner(IDTOSource<TConfiguration> source)
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
            var referenceFileType = this.CodeTypeReferenceService.GetReferenceFileType(property);

            var convertArguments = referenceFileType.NeedMappingServiceForConvert() ? new[] { sourcePropertyRef, this.MappingServiceRefExpr }
                                           : new[] { sourcePropertyRef };

            return new CodeNotNullConditionStatement(sourcePropertyRef)
                   {
                           TrueStatements =
                           {
                                   this.Configuration.GetConvertToDTOMethod(property.PropertyType, referenceFileType)
                                       .ToMethodInvokeExpression(convertArguments)
                                       .ToAssignStatement(targetPropertyRef)
                           },
                           FalseStatements =
                           {
                                   new CodePrimitiveExpression(null).ToAssignStatement(targetPropertyRef)
                           }
                   };
        }

        if (this.Configuration.IsCollectionProperty(property))
        {
            var collectionFileType = this.CodeTypeReferenceService.GetCollectionFileType(property);

            var convertArguments = collectionFileType.NeedMappingServiceForConvert() ? new[] { sourcePropertyRef, this.MappingServiceRefExpr }
                                           : new[] { sourcePropertyRef };

            var elementType = property.PropertyType.GetCollectionElementType();

            return this.Configuration.GetConvertToDTOListMethod(elementType, collectionFileType)
                       .ToMethodInvokeExpression(convertArguments)
                       .ToAssignStatement(targetPropertyRef);
        }

        return sourcePropertyRef.ToAssignStatement(targetPropertyRef);
    }
}
