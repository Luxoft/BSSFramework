using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class BLLFactoryContainerInterfaceFileFactory<TConfiguration> : BLLFactoryContainerInterfaceFileFactoryBase<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLFactoryContainerInterfaceFileFactory(TConfiguration configuration)
            : base(configuration)
    {

    }


    public override FileType FileType => FileType.BLLFactoryContainerInterface;


    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        foreach (var domainType in this.Configuration.BLLDomainTypes)
        {
            yield return new CodeMemberProperty
                         {
                                 Name = domainType.Name + "Factory",
                                 Type = this.Configuration.GetCodeTypeReference(domainType, FileType.BLLFactoryInterface),
                                 HasGet = true
                         };
        }
    }
}
