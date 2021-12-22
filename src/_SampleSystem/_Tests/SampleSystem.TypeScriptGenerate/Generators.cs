using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript;
using Framework.DomainDriven.Generation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.TypeScriptGenerate.Configurations.Environments;

namespace SampleSystem.TypeScriptGenerate
{
    [TestClass]
    public class Generators
    {
        private string FrameworkPath { get; } = Environment.CurrentDirectory.Replace(@"\",@"/").TakeWhileNot(@"/src", StringComparison.InvariantCultureIgnoreCase);

        private string TargetSystemPath => this.FrameworkPath + @"/src";

        private ICheckOutService CheckOutService { get; } = Framework.DomainDriven.Generation.CheckOutService.Trace;

        private readonly MainGenerationEnvironment mainEnvironment = new MainGenerationEnvironment();

        private readonly AuthorizationGenerationEnvironment authorizationEnvironment = new AuthorizationGenerationEnvironment();

        private readonly ConfigurationGenerationEnvironment configurationEnvironment = new ConfigurationGenerationEnvironment();

        [TestMethod]
        public void GenerateAll()
        {
            this.GenerateMain().ToList();
        }

        public IEnumerable<FileInfo> GenerateMain()
        {
            return this.GenerateMainClientDTO()
                       .Concat(this.GenerateConfigurationDTO())
                       .Concat(this.GenerateAuthorizationDTO())
                       .Concat(this.GenerateServiceFacade())
                       .Concat(this.GenerateQueryFacade())
                       .Concat(this.GenerateConfigurationServiceFacade())
                       .Concat(this.GenerateAuthorizationServiceFacade())
                       .Concat(this.GenerateReportServiceFacade());
        }

        [TestMethod]
        public void GenerateAuthorizationDTOTest()
        {
            this.GenerateAuthorizationDTO().ToList();
        }

        private IEnumerable<FileInfo> GenerateAuthorizationDTO()
        {
            var generator = new TypeScriptDTOFileGenerator(this.authorizationEnvironment.ClientDTO);

            yield return generator.GenerateSingle(
                this.TargetSystemPath + @"/SampleSystem.WebApiCore/js/generated/dto",
                "authorization.generated",
                this.CheckOutService);
        }

        [TestMethod]
        public void GenerateConfigurationDTOTest()
        {
            this.GenerateConfigurationDTO().ToList();
        }

        private IEnumerable<FileInfo> GenerateConfigurationDTO()
        {
            var generator = new TypeScriptDTOFileGenerator(this.configurationEnvironment.ClientDTO);

            yield return generator.GenerateSingle(
                this.TargetSystemPath + @"/SampleSystem.WebApiCore/js/generated/dto",
                "configuration.generated",
                this.CheckOutService);
        }

        [TestMethod]
        public void GenerateClientDTOTest()
        {
            this.GenerateMainClientDTO().ToList();
        }

        private IEnumerable<FileInfo> GenerateMainClientDTO()
        {
            var generator = new TypeScriptDTOFileGenerator(this.mainEnvironment.ClientDTO);

            yield return generator.GenerateSingle(
                this.TargetSystemPath + @"/SampleSystem.WebApiCore/js/generated/dto",
                "entities.generated",
                this.CheckOutService);
        }

        [TestMethod]
        public void GenerateServiceFacadeTest()
        {
            this.GenerateServiceFacade().ToList();
        }

        private IEnumerable<FileInfo> GenerateServiceFacade()
        {
            var generator = new TypeScriptFacadeFileGenerator(this.mainEnvironment.ClientMainFacade);

            yield return generator.GenerateSingle(
                this.TargetSystemPath + @"/SampleSystem.WebApiCore/js/generated/facade",
                "facade.generated",
                this.CheckOutService);
        }

        [TestMethod]
        public void GenerateQueryFacadeTest()
        {
            this.GenerateQueryFacade().ToList();
        }

        private IEnumerable<FileInfo> GenerateQueryFacade()
        {
            var generator = new TypeScriptFacadeFileGenerator(this.mainEnvironment.ClientQueryMainFacade);

            yield return generator.GenerateSingle(
                this.TargetSystemPath + @"/SampleSystem.WebApiCore/js/generated/facade",
                "query.facade.generated",
                this.CheckOutService);
        }

        [TestMethod]
        public void GenerateConfigurationServiceFacadeTest()
        {
            this.GenerateConfigurationServiceFacade().ToList();
        }

        private IEnumerable<FileInfo> GenerateConfigurationServiceFacade()
        {
            var generator = new TypeScriptFacadeFileGenerator(this.configurationEnvironment.ConfigurationFacade);

            yield return generator.GenerateSingle(
                this.TargetSystemPath + @"/SampleSystem.WebApiCore/js/generated/facade",
                "configuration.facade.generated",
                this.CheckOutService);
        }

        [TestMethod]
        public void GenerateAuthorizationServiceFacadeTest()
        {
            this.GenerateAuthorizationServiceFacade().ToList();
        }

        private IEnumerable<FileInfo> GenerateAuthorizationServiceFacade()
        {
            var generator = new TypeScriptFacadeFileGenerator(this.authorizationEnvironment.AuthFacade);

            yield return generator.GenerateSingle(
                this.TargetSystemPath + @"/SampleSystem.WebApiCore/js/generated/facade",
                "authorization.facade.generated",
                this.CheckOutService);
        }

        [TestMethod]
        public void GenerateReportServiceFacadeTest()
        {
            this.GenerateReportServiceFacade().ToList();
        }

        private IEnumerable<FileInfo> GenerateReportServiceFacade()
        {
            var generator = new TypeScriptFacadeFileGenerator(this.configurationEnvironment.ReportFacade);

            yield return generator.GenerateSingle(
                this.TargetSystemPath + @"/SampleSystem.WebApiCore/js/generated/facade",
                "report.facade.generated",
                this.CheckOutService);
        }
    }
}
