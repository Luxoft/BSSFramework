using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SampleSystem.CodeGenerate;
using SampleSystem.TypeScriptGenerate.Configurations.DTO;
using SampleSystem.TypeScriptGenerate.Configurations.Services;

namespace SampleSystem.TypeScriptGenerate.Configurations.Environments;

public class MainGenerationEnvironment : GenerationEnvironmentBase,
                                         Framework.DomainDriven.DTOGenerator.TypeScript.ITypeScriptGenerationEnvironmentBase
{
    public MainGenerationEnvironment()
    {
        this.ClientDTO = new MainDTOGeneratorConfiguration(this);

        this.ClientMainFacade = new MainFacadeConfiguration(this);

        this.ClientQueryMainFacade = new MainQueryFacadeConfiguration(this);
    }

    public MainDTOGeneratorConfiguration ClientDTO { get; }

    public MainFacadeConfiguration ClientMainFacade { get; }

    public MainQueryFacadeConfiguration ClientQueryMainFacade { get; }

    protected override IEnumerable<Assembly> GetDomainObjectAssemblies()
    {
        return base.GetDomainObjectAssemblies().Concat(new[] { typeof(SampleSystemSecurityOperationCode).Assembly });
    }
}
