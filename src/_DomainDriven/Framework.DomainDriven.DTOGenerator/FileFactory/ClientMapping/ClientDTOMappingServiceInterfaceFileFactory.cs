using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator;

public class ClientDTOMappingServiceInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration, FileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly ReadOnlyCollection<IClientMappingServiceExternalMethodGenerator> _externalGenerators;


    public ClientDTOMappingServiceInterfaceFileFactory(TConfiguration configuration, IEnumerable<IClientMappingServiceExternalMethodGenerator> externalGenerators)
            : base(configuration, null)
    {
        if (externalGenerators == null) throw new ArgumentNullException(nameof(externalGenerators));

        this._externalGenerators = externalGenerators.ToReadOnlyCollection();
    }


    public override FileType FileType { get; } = FileType.ClientDTOMappingServiceInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsInterface = true,
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        foreach (var fieldFileFactory in this._externalGenerators)
        {
            foreach (var method in fieldFileFactory.GetClientMappingServiceInterfaceMethods())
            {
                yield return method;
            }
        }
    }
}
