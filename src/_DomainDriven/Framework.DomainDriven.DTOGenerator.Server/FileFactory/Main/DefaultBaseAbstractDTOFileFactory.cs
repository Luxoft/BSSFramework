using System.CodeDom;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultBaseAbstractDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultBaseAbstractDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.DomainObjectBaseType)
    {
    }


    public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.BaseAbstractDTO;


    //protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    //{
    //    foreach (var baseMember in base.GetMembers())
    //    {
    //        yield return baseMember;
    //    }

    //    yield return this.GenerateDefaultConstructor();

    //    yield return this.GenerateFromDomainObjectConstructor(this.MapToMappingObjectPropertyAssigner);
    //}
}
