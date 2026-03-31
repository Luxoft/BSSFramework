using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main.Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main;

public class DefaultBaseAbstractDTOFileFactory<TConfiguration>(TConfiguration configuration)
    : MainDTOFileFactory<TConfiguration>(configuration, configuration.Environment.DomainObjectBaseType)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public override MainDTOFileType FileType { get; } = BaseFileType.BaseAbstractDTO;


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
