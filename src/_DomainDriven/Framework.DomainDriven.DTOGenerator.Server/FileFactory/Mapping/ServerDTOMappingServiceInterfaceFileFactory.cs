using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class ServerDTOMappingServiceInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration, FileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    private readonly ReadOnlyCollection<IServerMappingServiceExternalMethodGenerator> _externalGenerators;


    public ServerDTOMappingServiceInterfaceFileFactory(TConfiguration configuration, IEnumerable<IServerMappingServiceExternalMethodGenerator> externalGenerators)
            : base(configuration, null)
    {
        if (externalGenerators == null) throw new ArgumentNullException(nameof(externalGenerators));

        this._externalGenerators = externalGenerators.ToReadOnlyCollection();
    }


    public override FileType FileType { get; } = ServerFileType.ServerDTOMappingServiceInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsInterface = true,
               };
    }

    public override CodeTypeReference BaseReference => typeof(IDTOMappingService<,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), this.Configuration.Environment.GetIdentityType().ToTypeReference());


    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        foreach (var fieldFileFactory in this._externalGenerators)
        {
            foreach (var method in fieldFileFactory.GetServerMappingServiceInterfaceMethods())
            {
                yield return method;
            }
        }
    }
}
