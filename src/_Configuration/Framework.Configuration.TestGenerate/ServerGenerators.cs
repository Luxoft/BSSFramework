using Framework.CodeGeneration.BLLCoreGenerator;
using Framework.CodeGeneration.BLLGenerator;
using Framework.CodeGeneration.DTOGenerator.Server;
using Framework.CodeGeneration.Extensions;
using Framework.CodeGeneration.WebApiGenerator.SingleController;

using Framework.FileGeneration;

namespace Framework.Configuration.TestGenerate;

public partial class ServerGenerators
{
    [Fact]
    public void GenerateMainTest()
    {
        this.GenerateMain().ToList();
    }

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

        yield return generator.GenerateSingle(this.GeneratePath + @"/Framework.Configuration.BLL.Core/_Generated", "Configuration.Generated", this.CheckOutService);
    }

    private IEnumerable<GeneratedFileInfo> GenerateBLL()
    {
        var generator = new BLLFileGenerator(this.Environment.BLL);

        return generator.GenerateGroup(
            this.GeneratePath + @"/Framework.Configuration.BLL/_Generated",
            decl => decl.Name.Contains("FetchRuleExpander") ? "Configuration.FetchRuleExpander.Generated"
                    : decl.Name.Contains("ValidationMap") ? "Configuration.ValidationMap.Generated"
                    : decl.Name.Contains("Validator") ? "Configuration.Validator.Generated"
                    : "Configuration.Generated",
            this.CheckOutService);
    }

    [Fact]
    public void GenerateServerDTOTest() => this.GenerateServerDTO().ToList();

    private IEnumerable<GeneratedFileInfo> GenerateServerDTO()
    {
        var generator = new ServerFileGenerator(this.Environment.ServerDTO);

        yield return generator.GenerateSingle(
                                              this.GeneratePath + @"/Framework.Configuration.Generated.DTO",
                                              "Configuration.Generated",
                                              this.CheckOutService);
    }

    [Fact]
    public void GenerateMainControllerTest() => this.GenerateMainController().ToList();

    private IEnumerable<GeneratedFileInfo> GenerateMainController()
    {
        var generator = new SingleControllerCodeFileGenerator(this.Environment.MainController);

        var outputPath = Path.Combine(this.GeneratePath, "Framework.Configuration.WebApi");

        yield return generator.GenerateSingle(outputPath, "Configuration.Generated", this.CheckOutService);
    }
}
