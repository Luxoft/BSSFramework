using Framework.CodeGeneration.Extensions;
using Framework.CodeGeneration.WebApiGenerator.SingleController;
using Framework.FileGeneration;

namespace Framework.Authorization.WebApiGenerate;

[TestClass]
public partial class WebApiGenerators
{
    [TestMethod]
    public void GenerateMainTest()
    {
        this.GenerateMain().ToList();
    }

    public IEnumerable<GeneratedFileInfo> GenerateMain()
    {
        return this.GenerateMainController();
    }

    [TestMethod]
    public void GenerateMainControllerTest()
    {
        this.GenerateMainController().ToList();
    }

    private IEnumerable<GeneratedFileInfo> GenerateMainController()
    {
        var generator = new SingleControllerCodeFileGenerator(this.Environment.MainController);

        var outputPath = Path.Combine(this.GeneratePath, "Framework.Authorization.WebApi");

        yield return generator.GenerateSingle(outputPath, "Authorization.Generated", this.CheckOutService);
    }
}
