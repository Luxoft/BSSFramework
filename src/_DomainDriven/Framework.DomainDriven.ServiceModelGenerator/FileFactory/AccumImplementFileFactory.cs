using System.CodeDom;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class AccumImplementFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public AccumImplementFileFactory(TConfiguration configuration)
            : base(configuration, typeof(object))
    {

    }


    public override FileType FileType { get; } = FileType.Implement;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Configuration.ImplementClassName,
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsClass = true
               };
    }



    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        return from methodGenerator in this.Configuration.GetAccumMethodGenerators()

               from method in methodGenerator.GetFacadeMethods()

               select method;
    }
}
