using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultBasePersistentDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultBasePersistentDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.PersistentDomainObjectBaseType)
    {
    }


    public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.BasePersistentDTO;


    protected override bool ConvertToStrict { get; } = false;


    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        yield return this.GetIdentityObjectTypeRef();
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }


        yield return new CodeMemberProperty
                     {
                             Attributes = MemberAttributes.Public | MemberAttributes.Final,
                             Name = "IsNew",
                             Type = new CodeTypeReference(typeof(bool)),
                             GetStatements =
                             {
                                     new CodeValueEqualityOperatorExpression(
                                                                             this.Configuration.Environment.GetIdentityType().ToTypeReference().ToDefaultValueExpression(),
                                                                             this.Configuration.GetIdentityPropertyCodeExpression()).ToMethodReturnStatement()
                             }
                     };

        yield return this.GetIdentityObjectImplementation(true);
    }
}
