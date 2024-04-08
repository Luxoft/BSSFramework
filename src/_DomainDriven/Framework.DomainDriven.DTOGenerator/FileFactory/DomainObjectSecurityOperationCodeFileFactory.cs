using System.CodeDom;
using System.Reflection;
using System.Runtime.Serialization;

using Framework.SecuritySystem;

namespace Framework.DomainDriven.DTOGenerator;

public class DomainObjectSecurityOperationCodeFileFactory<TConfiguration> : FileFactory<TConfiguration, RoleFileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly IReadOnlyCollection<SecurityRule> SecurityOperations;


    public DomainObjectSecurityOperationCodeFileFactory(TConfiguration configuration, Type domainType, IEnumerable<SecurityRule> securityOperations)
            : base(configuration, domainType)
    {
        if (securityOperations == null) throw new ArgumentNullException(nameof(securityOperations));

        this.SecurityOperations = securityOperations.ToArray();
    }


    public override RoleFileType FileType { get; } = DTOGenerator.FileType.DomainObjectSecurityOperationCode;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       TypeAttributes = TypeAttributes.Public,
                       IsEnum = true
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield break;
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield return this.GetDataContractCodeAttributeDeclaration();
    }


    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        return from securityRule in this.SecurityOperations

               select new CodeMemberField
                      {
                              Name = securityRule.Name,
                              CustomAttributes =
                              {
                                      new CodeAttributeDeclaration (new CodeTypeReference(typeof(EnumMemberAttribute)))
                              }
                      };
    }
}
