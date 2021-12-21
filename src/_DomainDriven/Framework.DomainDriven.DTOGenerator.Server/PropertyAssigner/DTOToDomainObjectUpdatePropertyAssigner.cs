using System;
using System.CodeDom;

using Framework.CodeDom;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class DTOToDomainObjectUpdatePropertyAssigner<TConfiguration> : DTOToDomainObjectPropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        public DTOToDomainObjectUpdatePropertyAssigner(IDTOSource<TConfiguration> source)
            : base(source)
        {
        }

        protected override CodeMethodReferenceExpression GetCollectionMappingMethodReferenceExpression(CodeTypeReference transferElementTypeRef, Type elementType)
        {
            return this.MappingServiceRefExpr.ToMethodReferenceExpression(
                "GetUpdateCollectionMappingService",
                transferElementTypeRef,
                this.Configuration.GetCodeTypeReference(elementType, DTOType.IdentityDTO),
                elementType.ToTypeReference());
        }
    }
}
