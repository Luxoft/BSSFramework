using System.CodeDom;
using System.Reflection;

using Framework.Core;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class DomainSecurityServiceFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly IDomainSecurityServiceGenerator domainSecurityServiceGenerator;

    public DomainSecurityServiceFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.domainSecurityServiceGenerator = LazyInterfaceImplementHelper.CreateProxy(() => this.Configuration.RootSecurityServerGenerator.GetDomainSecurityServiceGenerator(this.DomainType));
    }


    public override FileType FileType => FileType.DomainSecurityService;

    public override CodeTypeReference BaseReference => this.domainSecurityServiceGenerator.BaseServiceType;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var codeTypeDeclaration = new CodeTypeDeclaration
                                  {
                                          Name = this.Name,
                                          TypeAttributes = TypeAttributes.Public,
                                          IsPartial = true,
                                          IsClass = true,
                                  };

        codeTypeDeclaration.TypeParameters.AddRange(this.Configuration.GetDomainTypeSecurityParameters(this.DomainType).ToArray());

        return codeTypeDeclaration;
    }


    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        return base.GetBaseTypes().Concat(this.domainSecurityServiceGenerator.GetBaseTypes());
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        return base.GetMembers().Concat(this.domainSecurityServiceGenerator.GetMembers());
    }

    protected override IEnumerable<CodeConstructor> GetConstructors()
    {
        foreach (var baseCtor in base.GetConstructors())
        {
            yield return baseCtor;
        }

        if (this.Configuration.GenerateDomainServiceConstructor(this.DomainType))
        {
            yield return this.domainSecurityServiceGenerator.GetConstructor();
        }
    }
}
