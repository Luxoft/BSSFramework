using Framework.CodeGeneration.Extensions;
using Framework.CodeGeneration.WebApiGenerator.SingleController;
using Framework.FileGeneration;

namespace Framework.Configuration.WebApiGenerate;

[TestClass]
public partial class WebApiGenerators
{
    [TestMethod]
    public void GenerateMainTest() => this.GenerateMain().ToList();

    public IEnumerable<GeneratedFileInfo> GenerateMain() => this.GenerateMainController();

    [TestMethod]
    public void GenerateMainControllerTest() => this.GenerateMainController().ToList();

    private IEnumerable<GeneratedFileInfo> GenerateMainController()
    {
        var generator = new SingleControllerCodeFileGenerator(this.Environment.MainController);

        var outputPath = Path.Combine(this.GeneratePath, "Framework.Configuration.WebApi");

        yield return generator.GenerateSingle(outputPath, "Configuration.Generated", this.CheckOutService);
    }
}
