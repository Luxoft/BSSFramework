using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Framework.CodeDom;

namespace Framework.DomainDriven.DTOGenerator;

public class DomainObjectSecurityOperationCodeFileFactory<TConfiguration> : FileFactory<TConfiguration, RoleFileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly IReadOnlyCollection<Enum> SecurityOperations;


    public DomainObjectSecurityOperationCodeFileFactory(TConfiguration configuration, Type domainType, IEnumerable<Enum> securityOperations)
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
        yield return Enum.GetUnderlyingType(this.Configuration.Environment.SecurityOperationCodeType).ToTypeReference();
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield return this.GetDataContractCodeAttributeDeclaration();
    }


    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        return from securityOperationCode in this.SecurityOperations

               select new CodeMemberField
                      {
                              Name = securityOperationCode.ToString(),
                              InitExpression = securityOperationCode.ToPrimitiveExpression(),
                              CustomAttributes =
                              {
                                      new CodeAttributeDeclaration (new CodeTypeReference(typeof(EnumMemberAttribute)))
                              }
                      };
    }
}
