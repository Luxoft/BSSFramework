using System.CodeDom;

using Framework.BLL.Domain.DirectMode;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.FileFactory;
using Framework.FileGeneration.Configuration;

using SampleSystem.CodeGenerate.Configurations.Services;

namespace SampleSystem.CodeGenerate.Configurations.BLLCore;

/// <summary>
/// Custom factory class for BLL interfaces to domain objects (an example of extension with added ComplexChangeModelType handling)
/// </summary>
public class SampleSystemBLLInterfaceFileFactory(BLLCoreGeneratorConfiguration configuration, Type domainType)
    : BLLInterfaceFileFactory<BLLCoreGeneratorConfiguration>(configuration, domainType)
{
    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        foreach (var complexChangeModelType in this.Configuration.Environment.GetModelTypes(this.DomainType!, this.Configuration.ComplexChangeModelType))
        {
            var methodName = this.DomainType!.GetModelMethodName(complexChangeModelType, SampleSystemModelRole.ComplexChange, false);

            complexChangeModelType.CheckDirectMode(DirectMode.In, true);

            yield return new CodeMemberMethod
            {
                Name = methodName,
                ReturnType = this.DomainType!.ToTypeReference(),
                Parameters =
                                 {
                                         complexChangeModelType.ToTypeReference().ToParameterDeclarationExpression("changeModel")
                                 }
            };
        }
    }
}
