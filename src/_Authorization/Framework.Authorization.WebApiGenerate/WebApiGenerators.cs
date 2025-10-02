using Framework.DomainDriven.Generation;
using Framework.DomainDriven.WebApiGenerator.NetCore.SingleController;

using FileInfo = Framework.DomainDriven.Generation.FileInfo;

namespace Framework.Authorization.WebApiGenerate;

[TestClass]
public partial class WebApiGenerators
{
    [TestMethod]
    public void GenerateMainTest()
    {
        this.GenerateMain().ToList();
    }

    public IEnumerable<FileInfo> GenerateMain()
    {
        return this.GenerateMainController();
    }

    [TestMethod]
    public void GenerateMainControllerTest()
    {
        this.GenerateMainController().ToList();
    }

    private IEnumerable<FileInfo> GenerateMainController()
    {
        var generator = new SingleControllerCodeFileGenerator(this.Environment.MainSLController);

        var outputPath = Path.Combine(this.GeneratePath, "Framework.Authorization.WebApi");

        yield return generator.GenerateSingle(outputPath, "Authorization.Generated", this.CheckOutService);
    }
}
