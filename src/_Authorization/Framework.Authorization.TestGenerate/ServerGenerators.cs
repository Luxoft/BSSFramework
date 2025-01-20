using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.BLLGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.NHibernate.DALGenerator;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FileInfo = Framework.DomainDriven.Generation.FileInfo;

namespace Framework.Authorization.TestGenerate;

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

        return generator.GenerateGroup(
                                       this.GeneratePath + @"/Framework.Authorization.BLL.Core/_Generated",
                                       decl => decl.Name.Contains("FetchService") ? "Authorization.FetchService.Generated"
                                               : decl.Name.Contains("ValidationMap") ? "Authorization.ValidationMap.Generated"
                                               : decl.Name.Contains("Validator") ? "Authorization.Validator.Generated"
                                               : "Authorization.Generated",
                                       this.CheckOutService);
    }

    [TestMethod]
    public void GenerateBLLTest()
    {
        this.GenerateBLL().ToList();
    }

    private IEnumerable<FileInfo> GenerateBLL()
    {
        var generator = new BLLFileGenerator(this.Environment.BLL);

        yield return generator.GenerateSingle(
                                              this.GeneratePath + @"/Framework.Authorization.BLL/_Generated",
                                              "Authorization.Generated",
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
                                              this.GeneratePath + @"/Framework.Authorization.Generated.DTO",
                                              "Authorization.Generated",
                                              this.CheckOutService);
    }
}
