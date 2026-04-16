using Framework.CodeGeneration.BLLCoreGenerator;
using Framework.CodeGeneration.BLLGenerator;
using Framework.CodeGeneration.DTOGenerator.Server;
using Framework.CodeGeneration.Extensions;
using Framework.CodeGeneration.WebApiGenerator.SingleController;
using Framework.FileGeneration;

using Xunit;

namespace Framework.Authorization.TestGenerate;

public partial class ServerGenerators
{
    [Fact]
    public void GenerateMainTest() => this.GenerateMain().ToList();

    public IEnumerable<GeneratedFileInfo> GenerateMain() =>
        this.GenerateBLLCore()
            .Concat(this.GenerateBLL())
            .Concat(this.GenerateServerDTO())
            .Concat(this.GenerateMainController());

    [Fact]
    public void GenerateBLLCoreTest() => this.GenerateBLLCore().ToList();

    private IEnumerable<GeneratedFileInfo> GenerateBLLCore()
    {
        var generator = new BLLCoreFileGenerator(this.Environment.BLLCore);

        yield return generator.GenerateSingle(this.GeneratePath + @"/Framework.Authorization.BLL.Core/_Generated", "Authorization.Generated", this.CheckOutService);
    }

    [Fact]
    public void GenerateBLLTest() => this.GenerateBLL().ToList();

    private IEnumerable<GeneratedFileInfo> GenerateBLL()
    {
        var generator = new BLLFileGenerator(this.Environment.BLL);

        return generator.GenerateGroup(
            this.GeneratePath + @"/Framework.Authorization.BLL/_Generated",
            decl => decl.Name.Contains("FetchRuleExpander") ? "Authorization.FetchRuleExpander.Generated"
                    : decl.Name.Contains("ValidationMap") ? "Authorization.ValidationMap.Generated"
                    : decl.Name.Contains("Validator") ? "Authorization.Validator.Generated"
                    : "Authorization.Generated",
            this.CheckOutService);
    }

    [Fact]
    public void GenerateServerDTOTest() => this.GenerateServerDTO().ToList();

    private IEnumerable<GeneratedFileInfo> GenerateServerDTO()
    {
        var generator = new ServerFileGenerator(this.Environment.ServerDTO);

        yield return generator.GenerateSingle(
                                              this.GeneratePath + @"/Framework.Authorization.Generated.DTO",
                                              "Authorization.Generated",
                                              this.CheckOutService);
    }

    [Fact]
    public void GenerateMainControllerTest() => this.GenerateMainController().ToList();


    private IEnumerable<GeneratedFileInfo> GenerateMainController()
    {
        var generator = new SingleControllerCodeFileGenerator(this.Environment.MainController);

        var outputPath = Path.Combine(this.GeneratePath, "Framework.Authorization.WebApi");

        yield return generator.GenerateSingle(outputPath, "Authorization.Generated", this.CheckOutService);
    }
}
