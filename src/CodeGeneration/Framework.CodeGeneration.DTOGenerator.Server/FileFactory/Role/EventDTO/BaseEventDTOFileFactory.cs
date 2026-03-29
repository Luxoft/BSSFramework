using System.CodeDom;
using System.Reflection;
using Framework.CodeGeneration.DTOGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileType;
using Framework.Projection;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.EventDTO;

public class DefaultBaseEventDTOFileFactory<TConfiguration>(TConfiguration configuration)
    : FileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, DTOFileType>(configuration, configuration.Environment.PersistentDomainObjectBaseType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public override DTOFileType FileType { get; } = ServerFileType.BaseEventDTO;



    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield return this.GetDataContractCodeAttributeDeclaration(this.Configuration.EventDataContractNamespace);

        var knownTypeAttributes =

                from domainType in this.Configuration.DomainTypes

                where !domainType.IsProjection()

                from eventOperationCode in this.Configuration.DomainObjectEventMetadata.GetEventOperations(domainType)

                let fileType = new DomainOperationEventDTOFileType(eventOperationCode)

                where this.Configuration.GeneratePolicy.Used(domainType, fileType)

                select this.Configuration.GetCodeTypeReference(domainType, fileType)
                           .ToKnownTypeCodeAttributeDeclaration();


        foreach (var knownTypeAttribute in knownTypeAttributes)
        {
            yield return knownTypeAttribute;
        }

        yield return this.Configuration.GetDTOFileAttribute(this.DomainType, this.FileType);
    }

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       IsClass = true,
                       IsPartial = true,
                       TypeAttributes = TypeAttributes.Public | TypeAttributes.Abstract
               };
    }
}
