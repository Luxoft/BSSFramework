using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.BLLGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.NHibernate.DALGenerator;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Attachments.TestGenerate
{
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
                       .Concat(this.GenerateServerDTO())
                       .Concat(this.GenerateDAL());
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
                this.GeneratePath + @"/Framework.Attachments.BLL.Core/_Generated",
                decl => decl.Name.Contains("FetchService") ? "Attachments.FetchService.Generated"
                    : decl.Name.Contains("ValidationMap") ? "Attachments.ValidationMap.Generated"
                    : decl.Name.Contains("Validator") ? "Attachments.Validator.Generated"
                    : "Attachments.Generated",
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
                this.GeneratePath + @"/Framework.Attachments.BLL/_Generated",
                "Attachments.Generated",
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
                this.GeneratePath + @"/Framework.Attachments.Generated.DTO",
                "Attachments.Generated",
                this.CheckOutService);
        }

        [TestMethod]
        public void GenerateDALTest()
        {
            this.GenerateDAL().ToList();
        }

        private IEnumerable<FileInfo> GenerateDAL()
        {
            var generator = new DALFileGenerator(this.Environment.DAL);

            return generator.Generate(this.GeneratePath + @"/Framework.Attachments.Generated.DAL.NHibernate/Mapping", this.CheckOutService);
        }
    }
}
