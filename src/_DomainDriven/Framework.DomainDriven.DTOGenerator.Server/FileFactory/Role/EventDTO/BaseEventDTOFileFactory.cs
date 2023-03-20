using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;

using Framework.DomainDriven.BLL;
using Framework.Projection;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultBaseEventDTOFileFactory<TConfiguration> : FileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, DTOFileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultBaseEventDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.PersistentDomainObjectBaseType)
    {
    }


    public override DTOFileType FileType { get; } = ServerFileType.BaseEventDTO;



    protected override System.Collections.Generic.IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield return this.GetDataContractCodeAttributeDeclaration(this.Configuration.EventDataContractNamespace);

        var knownTypeAttributes =

                from domainType in this.Configuration.DomainTypes

                where !domainType.IsProjection()

                from eventOperationCode in domainType.GetEventOperations(true)

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
