using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.BLLGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation;

using FileInfo = Framework.DomainDriven.Generation.FileInfo;

namespace Framework.Configuration.TestGenerate;

[TestClass]
public partial class ServerGenerators
{
    [TestMethod]
    public void GenerateMainTest()
    {
        this.GenerateMain().ToList();
    }

    public IEnumerable<FileInfo> GenerateMain()
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

    private IEnumerable<FileInfo> GenerateBLLCore()
    {
        var generator = new BLLCoreFileGenerator(this.Environment.BLLCore);

        yield return generator.GenerateSingle(this.GeneratePath + @"/Framework.Configuration.BLL.Core/_Generated", "Configuration.Generated", this.CheckOutService);
    }

    private IEnumerable<FileInfo> GenerateBLL()
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

    [TestMethod]
    public void GenerateServerDTOTest()
    {
        this.GenerateServerDTO().ToList();
    }

    private IEnumerable<FileInfo> GenerateServerDTO()
    {
        var generator = new ServerFileGenerator(this.Environment.ServerDTO);

        yield return generator.GenerateSingle(
                                              this.GeneratePath + @"/Framework.Configuration.Generated.DTO",
                                              "Configuration.Generated",
                                              this.CheckOutService);
    }
}
