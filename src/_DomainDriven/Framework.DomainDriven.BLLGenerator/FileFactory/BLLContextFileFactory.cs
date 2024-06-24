using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLGenerator;

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
        yield return typeof(SecurityBLLBaseContext<,,>).ToTypeReference(
            this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
            this.Configuration.Environment.GetIdentityType().ToTypeReference(),
            this.Configuration.Environment.BLLCore.GetCodeTypeReference(null, BLLCoreGenerator.FileType.BLLFactoryContainerInterface));

        yield return typeof(IBLLFactoryContainerContext<>).ToTypeReference(this.Configuration.Environment.BLLCore.SecurityBLLFactoryContainerType);

        yield return this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference;
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        var pairs = new[] { this.Configuration.Environment.BLLCore.DefaultBLLFactoryContainerType, this.Configuration.Environment.BLLCore.SecurityBLLFactoryContainerType }
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
