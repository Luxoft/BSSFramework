using System.Collections.Immutable;
using System.Reflection;

using Framework.BLL.Domain.Attributes;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.ExtendedMetadata;
using Framework.ExtendedMetadata.Builder;
using Framework.Projection;
using Framework.Validation;

using SampleSystem.CodeGenerate.Configurations._ProjectionSources;
using SampleSystem.CodeGenerate.Configurations.BLL;
using SampleSystem.CodeGenerate.Configurations.BLLCore;
using SampleSystem.CodeGenerate.Configurations.DAL;
using SampleSystem.CodeGenerate.Configurations.DTO.Server;
using SampleSystem.CodeGenerate.Configurations.Projection;
using SampleSystem.CodeGenerate.Configurations.Services.Audit;
using SampleSystem.CodeGenerate.Configurations.Services.Integration;
using SampleSystem.CodeGenerate.Configurations.Services.Main;
using SampleSystem.CodeGenerate.Configurations.Services.QueryService;
using SampleSystem.Domain;
using SampleSystem.Domain.BU;
using SampleSystem.Domain.Employee;
using SampleSystem.Domain.ForUpdate;
using SampleSystem.Domain.HRDepartment;
using SampleSystem.Domain.ManualProjections;
using SampleSystem.Security;
using SampleSystem.Validation;

using SecuritySystem;

namespace SampleSystem.CodeGenerate.Configurations;

public partial class ServerGenerationEnvironment() : CodeGenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase,
    AuditPersistentDomainObjectBase, Guid>(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
{
    public BLLCoreGeneratorConfiguration BLLCore => field ??= new BLLCoreGeneratorConfiguration(this);

    public BLLGeneratorConfiguration BLL => field ??= new BLLGeneratorConfiguration(this);

    public ServerDTOGeneratorConfiguration ServerDTO => field ??= new ServerDTOGeneratorConfiguration(this);

    public MainServiceGeneratorConfiguration MainService => field ??= new MainServiceGeneratorConfiguration(this);

    public QueryServiceGeneratorConfiguration QueryService => field ??= new QueryServiceGeneratorConfiguration(this);

    public IntegrationGeneratorConfiguration IntegrationService => field ??= new IntegrationGeneratorConfiguration(this);

    public DALGeneratorConfiguration DAL => field ??= new DALGeneratorConfiguration(this);

    public MainProjectionGeneratorConfiguration MainProjection => field ??= new MainProjectionGeneratorConfiguration(this);

    public LegacyProjectionGeneratorConfiguration LegacyProjection => field ??= new LegacyProjectionGeneratorConfiguration(this);

    public AuditServiceGeneratorConfiguration AuditService => field ??= new AuditServiceGeneratorConfiguration(this);

    public IAuditDTOGeneratorConfiguration<IAuditDTOGenerationEnvironment> AuditDTO => field ??= new AuditDTOGeneratorConfiguration(this);

    public IProjectionEnvironment MainProjectionEnvironment => field ??= this.CreateDefaultProjectionLambdaEnvironment(new SampleSystemProjectionSource());

    public IProjectionEnvironment LegacyProjectionEnvironment =>
        field ??= this.CreateDefaultProjectionLambdaEnvironment(
            new LegacySampleSystemProjectionSource(),
            this.GetCreateProjectionLambdaSetupParams(
                projectionSubNamespace: "LegacyProjections",
                useDependencySecurity: false));

    public override bool IsHierarchical(Type type) => new[] { typeof(BusinessUnit) }.Contains(type);

    public override ImmutableArray<Type> SecurityRuleTypeList { get; } = [typeof(SampleSystemSecurityOperation), typeof(SecurityRule)];

    protected override IEnumerable<Assembly> GetDomainObjectAssemblies() => base.GetDomainObjectAssemblies().Concat([typeof(Employee).Assembly]);

    protected override IEnumerable<IProjectionEnvironment> GetProjectionEnvironments()
    {
        yield return this.MainProjectionEnvironment;

        yield return this.LegacyProjectionEnvironment;

        yield return this.CreateManualProjectionLambdaEnvironment(typeof(TestManualEmployeeProjection).Assembly);
    }

    protected override IEnumerable<ExtendedAttributeSource> GetExtendedAttributeSources()
    {
        yield return new ExtendedAttributeSourceBuilder()

                     .Add<DomainObjectBase>(tb => tb.AddAttribute<AvailableDecimalValidatorAttribute>()
                                                    .AddAttribute<AvailablePeriodValidatorAttribute>()
                                                    .AddAttribute<AvailableDateTimeValidatorAttribute>()
                                                    .AddAttribute<DefaultStringMaxLengthValidatorAttribute>())

                     .Add<Employee>(b => b.AddAttribute<EmployeeValidatorAttribute>()
                                         .AddProperty(
                                              e => e.PersonalCellPhones,
                                              pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePersonalCellPhoneView))
                                                      .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePersonalCellPhoneEdit)))
                                          .AddProperty(
                                              e => e.Login,
                                              pb => pb.AddAttribute(new ViewDomainObjectAttribute(SecurityRule.View))
                                                      .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.EmployeeEdit)))
                                          .AddProperty(
                                              e => e.PersonalCellPhone,
                                              pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePersonalCellPhoneView)))
                                          .AddProperty(
                                              e => e.Position,
                                              pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePositionView))
                                                      .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.EmployeePositionEdit))))

                     .Add<Example1>(b => b.AddProperty(
                                        e => e.Field3,
                                        pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.LocationView))
                                                .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.LocationEdit))))

                     .Add<HRDepartment>(b => b.AddProperty(
                                            e => e.CompanyLegalEntity,
                                            pb => pb.AddAttribute(new ViewDomainObjectAttribute(SampleSystemSecurityOperation.CompanyLegalEntityView))
                                                    .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.CompanyLegalEntityEdit))))

                     .Build();
    }
}
