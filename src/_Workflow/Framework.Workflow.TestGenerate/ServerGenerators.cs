using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.BLLGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.NHibernate.DALGenerator;
using Framework.DomainDriven.ProjectionGenerator;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Workflow.TestGenerate
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
            return this.GenerateProjections()
                       .Concat(this.GenerateBLLCore())
                       .Concat(this.GenerateBLL())
                       .Concat(this.GenerateServerDTO())
                       .Concat(this.GenerateDAL());
        }

        [TestMethod]
        public void GenerateProjectionsTest()
        {
            this.GenerateProjections().ToList();
        }

        private IEnumerable<FileInfo> GenerateProjections()
        {
            var generator = new ProjectionFileGenerator(this.Environment.Projection);

            yield return generator.GenerateSingle(this.GeneratePath + @"\Framework.Workflow.Domain.Projections", "Workflow.Generated", this.CheckOutService, parallel: this.genParallel);
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
                this.GeneratePath + @"\Framework.Workflow.BLL.Core\_Generated",
                decl => decl.Name.Contains("FetchService") ? "Workflow.FetchService.Generated"
                    : decl.Name.Contains("ValidationMap") ? "Workflow.ValidationMap.Generated"
                    : decl.Name.Contains("Validator") ? "Workflow.Validator.Generated"
                    : "Workflow.Generated",
                this.CheckOutService,
                parallel: this.genParallel);
        }

        [TestMethod]
        public void GenerateBLLTest()
        {
            this.GenerateBLL().ToList();
        }

        private IEnumerable<FileInfo> GenerateBLL()
        {
            var generator = new BLLFileGenerator(this.Environment.BLL);

            yield return generator.GenerateSingle(this.GeneratePath + @"\Framework.Workflow.BLL\_Generated", "Workflow.Generated", this.CheckOutService, parallel: this.genParallel);
        }

        [TestMethod]
        public void GenerateServerDTOTest()
        {
            this.GenerateServerDTO().ToList();
        }

        private IEnumerable<FileInfo> GenerateServerDTO()
        {
            var clientGenerator = new ServerFileGenerator(this.Environment.ServerDTO);

            yield return clientGenerator.GenerateSingle(this.GeneratePath + @"\Framework.Workflow.Generated.DTO", "Workflow.Generated", this.CheckOutService, parallel: this.genParallel);
        }

        [TestMethod]
        public void GenerateDALTest()
        {
            this.GenerateDAL().ToList();
        }

        private IEnumerable<FileInfo> GenerateDAL()
        {
            var generator = new DALFileGenerator(this.Environment.DAL);

            return generator.Generate(this.GeneratePath + @"\Framework.Workflow.Generated.DAL.NHibernate\Mapping", this.CheckOutService);
        }
    }
}
