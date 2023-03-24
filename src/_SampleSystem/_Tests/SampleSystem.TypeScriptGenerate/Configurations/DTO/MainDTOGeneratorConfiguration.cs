using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.TypeScript;
using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;
using Framework.DomainDriven.Generation.Domain;

using SampleSystem.Domain;
using SampleSystem.TypeScriptGenerate.Configurations.Environments;
using SampleSystem.TypeScriptGenerate.Configurations.Services;

namespace SampleSystem.TypeScriptGenerate.Configurations.DTO;

public class MainDTOGeneratorConfiguration : TypeScriptDTOGeneratorConfiguration<MainGenerationEnvironment>
{
    public MainDTOGeneratorConfiguration(MainGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override bool GenerateClientMappingService { get; } = true;


    //public override IEnumerable<RequireJsModule> GetModules()
    //{
    //    var data = new List<RequireJsModule>
    //               {
    //                   new RequireJsModule("{ Guid, Convert }", "luxite/system", string.Empty),
    //                   new RequireJsModule("* as Framework", "luxite/framework/framework", string.Empty),
    //                   new RequireJsModule("{ Core }", "luxite/framework/framework")
    //               };
    //    return data;
    //}


    //protected override IEnumerable<Type> GetDomainTypes()
    //{
    //    yield return typeof(TestBusinessUnit);
    //    yield return typeof(TestBusinessUnitType);
    //}

    protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
    {
        var facadePolicy = new DTOTypeScriptServiceGeneratePolicy<MainFacadeConfiguration>(this.Environment.ClientMainFacade)
                        .Or(new DTOTypeScriptServiceGeneratePolicy<MainQueryFacadeConfiguration>(this.Environment.ClientQueryMainFacade))
                //.Or(new DTORoleGeneratePolicy(DTORole.Client, ClientDTORole.Update))
                ;

        var tempPolicy = new TypeScriptDependencyGeneratePolicy(facadePolicy, this.GetTypeMaps());


        //var tempRes0 = tempPolicy.Used(typeof(Employee), FileType.RichDTO);
        //var tempRes1 = tempPolicy.Used(typeof(SampleStruct), ClientFileType.Struct);
        var tempRes2 = tempPolicy.Used(typeof(SampleEnumForStructDependency), ClientFileType.Enum);
        //var tempRes2 = tempPolicy.Used(typeof(Employee), ObservableFileType.ObservableRichDTO);

        //var tempRes1 = tempPolicy.Used(typeof(TestBusinessUnitType), FileType.ProjectionDTO);
        //var tempRes2 = tempPolicy.Used(typeof(TestBusinessUnitType), ObservableFileType.ObservableProjectionDTO);

        return new TypeScriptDependencyGeneratePolicy(

                                                      facadePolicy,
                                                      this.GetTypeMaps())//.Except((RoleFileType identity) => identity.Name.Contains("vable"))
                ;
    }


    public override IEnumerable<RequireJsModule> GetModules()
    {
        foreach (var baseModule in base.GetModules())
        {
            yield return baseModule;
        }

        yield return new RequireJsModule("* as FrameworkCore", "../../FrameworkCore");
    }
}
