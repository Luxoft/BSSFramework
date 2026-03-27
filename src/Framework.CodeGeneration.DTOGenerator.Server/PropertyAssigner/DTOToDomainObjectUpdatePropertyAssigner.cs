using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner;

public class DTOToDomainObjectUpdatePropertyAssigner<TConfiguration>(IDTOSource<TConfiguration> source) : DTOToDomainObjectPropertyAssigner<TConfiguration>(source)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected override CodeMethodReferenceExpression GetCollectionMappingMethodReferenceExpression(CodeTypeReference transferElementTypeRef, Type elementType)
    {
        return this.MappingServiceRefExpr.ToMethodReferenceExpression(
                                                                      "GetUpdateCollectionMappingService",
                                                                      transferElementTypeRef,
                                                                      this.Configuration.GetCodeTypeReference(elementType, DTOType.IdentityDTO),
                                                                      elementType.ToTypeReference());
    }
}
