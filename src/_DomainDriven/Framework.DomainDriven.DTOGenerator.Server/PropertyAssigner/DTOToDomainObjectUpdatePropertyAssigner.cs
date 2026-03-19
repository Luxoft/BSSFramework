using System.CodeDom;

using Framework.CodeDom;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator.Server;

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
