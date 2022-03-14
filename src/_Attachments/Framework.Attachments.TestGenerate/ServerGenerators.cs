using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation;

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
            return this.GenerateServerDTO();
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
}
