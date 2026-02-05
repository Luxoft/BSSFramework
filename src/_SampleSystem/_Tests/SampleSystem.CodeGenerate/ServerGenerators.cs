using System.CodeDom;

using CommonFramework;

using Framework.CodeDom;
using Framework.DomainDriven;
using Framework.DomainDriven.BLLGenerator;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Audit;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.NHibernate.DALGenerator;
using Framework.DomainDriven.ProjectionGenerator;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.DomainDriven.WebApiGenerator.NetCore;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.CodeGenerate.ServerDTO;

using FileInfo = Framework.DomainDriven.Generation.FileInfo;
using FileType = Framework.DomainDriven.DTOGenerator.FileType;
using IGenerationEnvironmentBase = Framework.DomainDriven.ServiceModelGenerator.IGenerationEnvironmentBase;

namespace SampleSystem.CodeGenerate;

[TestClass]
public partial class ServerGenerators
{
    [TestMethod]
    public void GenerateMainTest() => this.GenerateMain().ToList();

    public IEnumerable<FileInfo> GenerateMain() =>
        this.GenerateMainProjections()
            .Concat(this.GenerateLegacyProjections())
            .Concat(this.GenerateBLLCore())
            .Concat(this.GenerateBLL())
            .Concat(this.GenerateServerDTO())
            .Concat(this.GenerateAuditDTO())
            .Concat(this.GenerateDAL())
            .Concat(this.GenerateMainWebApiNetCore())
            .Concat(this.GenerateMainQueryWebApiNetCore())
            .Concat(this.GenerateIntegrationWebApiNetCore())
            .Concat(this.GenerateAuthWebApiNetCoreTest())
            .Concat(this.GenerateAuditWebApiNetCore())
            .Concat(this.GenerateConfigurationWebApiNetCoreTest());

    [TestMethod]
    public void GenerateMainWebApiNetCoreTest() => this.GenerateMainWebApiNetCore().ToList();

    public IEnumerable<FileInfo> GenerateMainWebApiNetCore()
    {
        var configurators =
            new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<
            IGenerationEnvironmentBase>[] { this.environment.MainService };

        var generator = new WebApiNetCoreFileGenerator(
            this.environment.ToWebApiNetCore(
                configurators,
                "SampleSystem.WebApiCore.Controllers.Main"));

        return generator.DecorateProjectionsToRootControllerNetCore()
                        .WithAutoRequestMethods()
                        .Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "Main"));
    }

    public IEnumerable<FileInfo> GenerateMainQueryWebApiNetCore()
    {
        var configurators =
            new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<
            IGenerationEnvironmentBase>[] { this.environment.QueryService };

        var attr = new CodeAttributeDeclaration(
            typeof(RouteAttribute).ToTypeReference(),
            new CodeAttributeArgument(new CodePrimitiveExpression("mainQueryApi/[controller]/[action]")));

        var generator = new WebApiNetCoreFileGenerator(
            this.environment.ToWebApiNetCore(
                configurators,
                "SampleSystem.WebApiCore.Controllers.MainQuery"),
            additionalControllerAttributes: new[] { attr }.ToList());

        return generator.DecorateProjectionsToRootControllerNetCore("Query")
                        .WithAutoRequestMethods()
                        .Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "MainQuery"));
    }

    public IEnumerable<FileInfo> GenerateIntegrationWebApiNetCore()
    {
        var configurators =
            new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<
            IGenerationEnvironmentBase>[] { this.environment.IntegrationService };

        var attr = new CodeAttributeDeclaration(
            typeof(RouteAttribute).ToTypeReference(),
            new CodeAttributeArgument(new CodePrimitiveExpression("integrationApi/[controller]/[action]")));

        var generator = new WebApiNetCoreFileGenerator(
            this.environment.ToWebApiNetCore(
                configurators,
                "SampleSystem.WebApiCore.Controllers.Integration"),
            additionalControllerAttributes: new[] { attr }.ToList());

        return generator.DecorateProjectionsToRootControllerNetCore()
                        .WithAutoRequestMethods()
                        .Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "Integration"));
    }

    public IEnumerable<FileInfo> GenerateAuditWebApiNetCore()
    {
        var configurators =
            new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<
            IGenerationEnvironmentBase>[] { this.environment.AuditService };

        var attr = new CodeAttributeDeclaration(
            typeof(RouteAttribute).ToTypeReference(),
            new CodeAttributeArgument(new CodePrimitiveExpression("mainAuditApi/[controller]/[action]")));

        var generator = new WebApiNetCoreFileGenerator(
            this.environment.ToWebApiNetCore(
                configurators,
                "SampleSystem.WebApiCore.Controllers.Audit"),
            additionalControllerAttributes: new[] { attr }.ToList());

        return generator.DecorateProjectionsToRootControllerNetCore()
                        .WithAutoRequestMethods()
                        .Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "Audit"));
    }

    public IEnumerable<FileInfo> GenerateAuthWebApiNetCoreTest()
    {
        var e = new Framework.Authorization.TestGenerate.ServerGenerationEnvironment(new DatabaseName("$", "$"));

        var configurators =
            new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<
            IGenerationEnvironmentBase>[] { e.MainService };

        var attr = new CodeAttributeDeclaration(
            typeof(RouteAttribute).ToTypeReference(),
            new CodeAttributeArgument(new CodePrimitiveExpression("authApi/[controller]/[action]")));

        var generator = new WebApiNetCoreFileGenerator(
            e.ToWebApiNetCore(configurators),
            additionalControllerAttributes: new[] { attr }.ToList());

        return generator.DecorateProjectionsToRootControllerNetCore()
                        .WithAutoRequestMethods()
                        .Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "Auth"));
    }

    public IEnumerable<FileInfo> GenerateConfigurationWebApiNetCoreTest()
    {
        var e = new Framework.Configuration.TestGenerate.ServerGenerationEnvironment(new DatabaseName("$", "$"));

        var configurators =
            new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<
            IGenerationEnvironmentBase>[] { e.MainService };

        var attr = new CodeAttributeDeclaration(
            typeof(RouteAttribute).ToTypeReference(),
            new CodeAttributeArgument(new CodePrimitiveExpression("configApi/[controller]/[action]")));

        var generator = new WebApiNetCoreFileGenerator(
            e.ToWebApiNetCore(configurators),
            additionalControllerAttributes: new[] { attr }.ToList());

        return generator.DecorateProjectionsToRootControllerNetCore()
                        .WithAutoRequestMethods()
                        .Generate<CodeNamespace>(Path.Combine(this.webApiNetCorePath, "Configuration"));
    }

    [TestMethod]
    public void GenerateMainProjectionsTest() => this.GenerateMainProjections().ToList();

    private IEnumerable<FileInfo> GenerateMainProjections()
    {
        var generator = new ProjectionFileGenerator(this.environment.MainProjection);

        yield return generator.GenerateSingle(
            TargetSystemPath + @"/SampleSystem.Domain.Projections",
            "SampleSystem.Generated",
            this.CheckOutService,
            false);
    }

    [TestMethod]
    public void GenerateLegacyProjectionsTest() => this.GenerateLegacyProjections().ToList();

    private IEnumerable<FileInfo> GenerateLegacyProjections()
    {
        var generator = new ProjectionFileGenerator(this.environment.LegacyProjection);

        yield return generator.GenerateSingle(
            TargetSystemPath + @"/SampleSystem.Domain.LegacyProjections",
            "SampleSystem.Generated",
            this.CheckOutService,
            false);
    }

    [TestMethod]
    public void GenerateBLLCoreTest() => this.GenerateBLLCore().ToList();

    /// <summary>
    ///     Кастомная генерация BLL-слоя
    /// </summary>
    /// <returns></returns>
    private IEnumerable<FileInfo> GenerateBLLCore()
    {
        var generator = new SampleSystemBLLCoreFileGenerator(this.environment.BLLCore);


        yield return generator.GenerateSingle(
            TargetSystemPath + @"/SampleSystem.BLL.Core/_Generated",
            "SampleSystem.Generated",
            this.CheckOutService);
    }

    [TestMethod]
    public void GenerateBLLTest() => this.GenerateBLL().ToList();

    private IEnumerable<FileInfo> GenerateBLL()
    {
        var generator = new BLLFileGenerator(this.environment.BLL);

        return generator.GenerateGroup(
            TargetSystemPath + @"/SampleSystem.BLL/_Generated",
            decl => decl.Name.Contains("FetchRuleExpander") ? "SampleSystem.FetchRuleExpander.Generated"
                    : decl.Name.Contains("ValidationMap") ? "SampleSystem.ValidationMap.Generated"
                    : decl.Name.Contains("Validator") ? "SampleSystem.Validator.Generated"
                    : "SampleSystem.Generated",
            this.CheckOutService);
    }

    [TestMethod]
    public void GenerateServerDTOTest() => this.GenerateServerDTO().ToList();

    private IEnumerable<FileInfo> GenerateServerDTO()
    {
        var generator =
            new SampleSystemServerFileGenerator<ServerDTOGeneratorConfiguration>(
                this.environment.ServerDTO);

        return generator.GenerateGroup(
            TargetSystemPath + @"/SampleSystem.Generated.DTO",
            decl => decl.Name.Contains("Client") && decl.Name.Contains("DTOMappingService")
                        ? "SampleSystemClientMappingService.Generated"
                        : decl.Name.Contains("DTOMappingService")
                            ? "SampleSystemMappingService.Generated"
                            : (decl.UserData["FileType"] as DTOFileType).Maybe(
                                type => type.Role == DTORole.Event)
                                ? "SampleSystem.Event.Generated"
                                : (decl.UserData["FileType"] as DTOFileType).Maybe(
                                    type => type.Role == DTORole.Integration)
                                    ? "SampleSystem.Integration.Generated"
                                    : (decl.UserData["FileType"] as DTOFileType)
                                    .Maybe(type => type == FileType.ProjectionDTO)
                                        ? "SampleSystem.Projections.Generated"
                                        : decl.Name.EndsWith("Helper")
                                            ? "SampleSystem.Helpers.Generated"
                                            : "SampleSystem.Generated",
            this.CheckOutService);
    }

    [TestMethod]
    public void GenerateAuditDTOTest() => this.GenerateAuditDTO().ToList();

    private IEnumerable<FileInfo> GenerateAuditDTO()
    {
        var dtoGenerator = new AuditDTOModelFileGenerator(this.environment.AuditDTO);

        yield return dtoGenerator.GenerateSingle(
            TargetSystemPath + @"/SampleSystem.Generated.DTO",
            "SampleSystem.Audit.Generated");

        var generator =
            new SampleSystemServerFileGenerator<ServerDTOGeneratorConfiguration>(
                this.environment.ServerDTO);
    }

    [TestMethod]
    public void GenerateDALTest() => this.GenerateDAL().ToList();

    private IEnumerable<FileInfo> GenerateDAL()
    {
        var generator = new DALFileGenerator(this.environment.DAL);

        return generator.Generate(TargetSystemPath + @"/SampleSystem.Generated.DAL.NHibernate/Mapping", this.CheckOutService);
    }
}
