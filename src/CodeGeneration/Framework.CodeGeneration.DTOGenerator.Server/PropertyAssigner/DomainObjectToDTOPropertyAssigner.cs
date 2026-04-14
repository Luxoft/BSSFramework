using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileType;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner;

public class DomainObjectToDTOPropertyAssigner<TConfiguration>(IDTOSource<TConfiguration> source) : ServerPropertyAssigner<TConfiguration>(source)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
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
