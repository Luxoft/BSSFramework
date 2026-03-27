using System.CodeDom;

using Framework.BLL;
using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.BLLGenerator.FileFactory.__Base;
using Framework.CodeGeneration.DomainMetadata;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class BLLContextFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
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

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return typeof(SecurityBLLBaseContext<,,>).ToTypeReference(
            this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
            this.Configuration.Environment.GetIdentityType().ToTypeReference(),
            this.Configuration.Environment.BLLCore.GetCodeTypeReference(null, BLLCoreGenerator.FileType.BLLFactoryContainerInterface));

        yield return typeof(IBLLFactoryContainerContext<>).ToTypeReference(this.Configuration.Environment.BLLCore.SecurityBLLFactoryContainerType);

        yield return this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference;
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
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
