using Framework.CodeGeneration.BLLCoreGenerator;
using Framework.CodeGeneration.BLLGenerator;
using Framework.CodeGeneration.DTOGenerator.Server;
using Framework.CodeGeneration.Extensions;
using Framework.FileGeneration;

namespace Framework.Authorization.TestGenerate;

[TestClass]
public partial class ServerGenerators
{
    [TestMethod]
    public void GenerateMainTest()
    {
        this.GenerateMain().ToList();
    }

    public IEnumerable<GeneratedFileInfo> GenerateMain()
    {
        return this.GenerateBLLCore()
                   .Concat(this.GenerateBLL())
                   .Concat(this.GenerateServerDTO());
    }

    [TestMethod]
    public void GenerateBLLCoreTest()
    {
        this.GenerateBLLCore().ToList();
    }

    private IEnumerable<GeneratedFileInfo> GenerateBLLCore()
    {
        var generator = new BLLCoreFileGenerator(this.Environment.BLLCore);

        yield return generator.GenerateSingle(this.GeneratePath + @"/Framework.Authorization.BLL.Core/_Generated", "Authorization.Generated", this.CheckOutService);
    }

    [TestMethod]
    public void GenerateBLLTest()
    {
        this.GenerateBLL().ToList();
    }

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

    [TestMethod]
    public void GenerateServerDTOTest()
    {
        this.GenerateServerDTO().ToList();
    }

    private IEnumerable<GeneratedFileInfo> GenerateServerDTO()
    {
        var generator = new ServerFileGenerator(this.Environment.ServerDTO);

        yield return generator.GenerateSingle(
                                              this.GeneratePath + @"/Framework.Authorization.Generated.DTO",
                                              "Authorization.Generated",
                                              this.CheckOutService);
    }
}
