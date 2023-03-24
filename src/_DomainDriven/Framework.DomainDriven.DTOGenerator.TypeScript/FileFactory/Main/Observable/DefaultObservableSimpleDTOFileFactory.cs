using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable;

/// <summary>
/// Default observable simpleDTO file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class DefaultObservableSimpleDTOFileFactory<TConfiguration> : ObservableDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public DefaultObservableSimpleDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public override MainDTOFileType FileType => ObservableFileType.ObservableSimpleDTO;

    public override CodeTypeReference BaseReference
        => this.Configuration.GetCodeTypeReference(
                                                   this.DomainType,
                                                   this.IsPersistent() ? ObservableFileType.BaseObservableAuditPersistentDTO : ObservableFileType.BaseObservableAbstractDTO);

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        if (this.IsPersistent())
        {
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerImplementation();
            }
        }
    }
}
