using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class BLLContextFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLContextFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
    }

    public override FileType FileType => FileType.BLLContext;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsClass = true,
               };
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return typeof(SecurityBLLBaseContext<,,,>).ToTypeReference(
            this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
            this.Configuration.Environment.DomainObjectBaseType.ToTypeReference(),
            this.Configuration.Environment.GetIdentityType().ToTypeReference(),
            this.Configuration.GetCodeTypeReference(null, FileType.BLLFactoryContainerInterface));

        yield return typeof(IBLLFactoryContainerContext<>).ToTypeReference(this.Configuration.SecurityBLLFactoryContainerType);

        yield return this.Configuration.BLLContextInterfaceTypeReference;
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        var pairs = new[] { this.Configuration.DefaultBLLFactoryContainerType, this.Configuration.SecurityBLLFactoryContainerType }
                .Select(z => new
                             {
                                     BLLFactoryContextType = z,
                                     ContainerContextType = typeof(IBLLFactoryContainerContext<>).MakeGenericType(z)
                             });

        foreach (var pair in pairs)
        {
            yield return new CodeMemberProperty
                         {
                                 Name = "Logics",
                                 PrivateImplementationType = pair.ContainerContextType.ToTypeReference(),
                                 Type = pair.BLLFactoryContextType.ToTypeReference(),
                                 GetStatements = { new CodeThisReferenceExpression().ToPropertyReference("Logics").ToMethodReturnStatement() }
                         };
        }
    }
}
