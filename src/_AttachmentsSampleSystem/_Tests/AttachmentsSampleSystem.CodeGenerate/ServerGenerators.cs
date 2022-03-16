using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Framework.CodeDom;
using Framework.Core;
using Framework.CustomReports.Generation.BLL;
using Framework.DomainDriven;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.BLLGenerator;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Audit;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.NHibernate.DALGenerator;
using Framework.DomainDriven.ProjectionGenerator;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.DomainDriven.WebApiGenerator.NetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FileInfo = Framework.DomainDriven.Generation.FileInfo;
using FileType = Framework.DomainDriven.DTOGenerator.FileType;

namespace AttachmentsSampleSystem.CodeGenerate
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
                       .Concat(this.GenerateDAL())
                       .Concat(this.GenerateMainWebApiNetCore())
                       .Concat(this.GenerateMainQueryWebApiNetCore())
                       .Concat(this.GenerateAuthWebApiNetCoreTest())
                       .Concat(this.GenerateConfigurationWebApiNetCoreTest());
        }

        [TestMethod]
        public void GenerateMainWebApiNetCoreTest()
        {
            this.GenerateMainWebApiNetCore().ToList();
        }

        public IEnumerable<FileInfo> GenerateMainWebApiNetCore()
        {
            var configurators = new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<Framework.DomainDriven.ServiceModelGenerator.IGenerationEnvironmentBase>[]
                                {
                                    this.environment.MainService
                                };

            var generator = new WebApiNetCoreFileGenerator(this.environment.ToWebApiNetCore(configurators, nameSpace: "AttachmentsSampleSystem.WebApiCore.Controllers.Main"));

            return generator.DecorateProjectionsToRootControllerNetCore()
                            .WithAutoRequestMethods()
                            .Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "Main"));
        }

        public IEnumerable<FileInfo> GenerateMainQueryWebApiNetCore()
        {
            var configurators = new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<Framework.DomainDriven.ServiceModelGenerator.IGenerationEnvironmentBase>[]
                                {
                                    this.environment.QueryService
                                };


            var attr = new CodeAttributeDeclaration(
                typeof(RouteAttribute).ToTypeReference(),
                new CodeAttributeArgument(new CodePrimitiveExpression("mainQueryApi/v{version:apiVersion}/[controller]")));

            var generator = new WebApiNetCoreFileGenerator(this.environment.ToWebApiNetCore(configurators, nameSpace: "AttachmentsSampleSystem.WebApiCore.Controllers.MainQuery"), additionalControllerAttributes: new[] { attr }.ToList());

            return generator.DecorateProjectionsToRootControllerNetCore("Query").WithAutoRequestMethods().Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "MainQuery"));
        }

        public IEnumerable<FileInfo> GenerateAuthWebApiNetCoreTest()
        {
            var e = new Framework.Authorization.TestGenerate.ServerGenerationEnvironment(new DatabaseName("$", "$"));

            var configurators = new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<Framework.DomainDriven.ServiceModelGenerator.IGenerationEnvironmentBase>[]
                                {
                                    e.MainService,
                                };

            var attr = new CodeAttributeDeclaration(
                typeof(RouteAttribute).ToTypeReference(),
                new CodeAttributeArgument(new CodePrimitiveExpression("authApi/v{version:apiVersion}/[controller]")));

            var generator = new WebApiNetCoreFileGenerator(e.ToWebApiNetCore(configurators), additionalControllerAttributes: new[] { attr }.ToList());

            return generator.DecorateProjectionsToRootControllerNetCore().WithAutoRequestMethods().Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "Auth"));
        }

        public IEnumerable<FileInfo> GenerateConfigurationWebApiNetCoreTest()
        {
            var e = new Framework.Configuration.TestGenerate.ServerGenerationEnvironment(new DatabaseName("$", "$"));

            var configurators = new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<Framework.DomainDriven.ServiceModelGenerator.IGenerationEnvironmentBase>[]
                                {
                                    e.MainService,
                                };

            var attr = new CodeAttributeDeclaration(
                typeof(RouteAttribute).ToTypeReference(),
                new CodeAttributeArgument(new CodePrimitiveExpression("configApi/v{version:apiVersion}/[controller]")));

            var generator = new WebApiNetCoreFileGenerator(e.ToWebApiNetCore(configurators), additionalControllerAttributes: new[] { attr }.ToList());

            return generator.DecorateProjectionsToRootControllerNetCore().WithAutoRequestMethods().Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "Configuration"));
        }

        [TestMethod]
        public void GenerateBLLCoreTest()
        {
            this.GenerateBLLCore().ToList();
        }

        /// <summary>
        /// Кастомная генерация BLL-слоя
        /// </summary>
        /// <returns></returns>
        private IEnumerable<FileInfo> GenerateBLLCore()
        {
            var generator = new BLLCoreFileGenerator(this.environment.BLLCore);

            return generator.GenerateGroup(
                TargetSystemPath + @"/AttachmentsSampleSystem.BLL.Core/_Generated",
                decl => decl.Name.Contains("FetchService") ? "AttachmentsSampleSystem.FetchService.Generated"
                    : decl.Name.Contains("ValidationMap") ? "AttachmentsSampleSystem.ValidationMap.Generated"
                    : decl.Name.Contains("Validator") ? "AttachmentsSampleSystem.Validator.Generated"
                    : "AttachmentsSampleSystem.Generated",
                this.CheckOutService);
        }

        [TestMethod]
        public void GenerateBLLTest()
        {
            this.GenerateBLL().ToList();
        }

        private IEnumerable<FileInfo> GenerateBLL()
        {
            var generator = new BLLFileGenerator(this.environment.BLL);

            yield return generator.GenerateSingle(TargetSystemPath + @"/AttachmentsSampleSystem.BLL/_Generated", "AttachmentsSampleSystem.Generated", this.CheckOutService);
        }

        [TestMethod]
        public void GenerateServerDTOTest()
        {
            this.GenerateServerDTO().ToList();
        }

        private IEnumerable<FileInfo> GenerateServerDTO()
        {
            var generator = new ServerFileGenerator(this.environment.ServerDTO);

            return generator.GenerateGroup(
                TargetSystemPath + @"/AttachmentsSampleSystem.Generated.DTO",
                decl => decl.Name.Contains("Client") && decl.Name.Contains("DTOMappingService") ? "AttachmentsSampleSystemClientMappingService.Generated"
                    : decl.Name.Contains("DTOMappingService") ? "AttachmentsSampleSystemMappingService.Generated"
                    : (decl.UserData["FileType"] as DTOFileType).Maybe(type => type.Role == DTORole.Event) ? "AttachmentsSampleSystem.Event.Generated"
                    : (decl.UserData["FileType"] as DTOFileType).Maybe(type => type.Role == DTORole.Integration) ? "AttachmentsSampleSystem.Integration.Generated"
                    : (decl.UserData["FileType"] as DTOFileType).Maybe(type => type == FileType.ProjectionDTO) ? "AttachmentsSampleSystem.Projections.Generated"
                    : decl.Name.EndsWith("Helper") ? "AttachmentsSampleSystem.Helpers.Generated"
                    : "AttachmentsSampleSystem.Generated",
              this.CheckOutService);
        }

        [TestMethod]
        public void GenerateDALTest()
        {
            this.GenerateDAL().ToList();
        }

        private IEnumerable<FileInfo> GenerateDAL()
        {
            var generator = new DALFileGenerator(this.environment.DAL);

            return generator.Generate(TargetSystemPath + @"/AttachmentsSampleSystem.Generated.DAL.NHibernate/Mapping", this.CheckOutService);
        }
    }
}
