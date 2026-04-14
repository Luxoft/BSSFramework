using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.Configuration;
using Framework.Configuration.Domain;
using Framework.Configuration.TestGenerate.Configurations.BLL;
using Framework.Configuration.TestGenerate.Configurations.BLLCore;
using Framework.Configuration.TestGenerate.Configurations.DTO;
using Framework.Configuration.TestGenerate.Configurations.Services.Main;
using Framework.Configuration.TestGenerate.Configurations.Services.QueryService;
using Framework.Configuration.TestGenerate.Configurations.Services.WebApi;
using Framework.Database;
using Framework.Database.NHibernate.Mapping;
using Framework.ExtendedMetadata;
using Framework.ExtendedMetadata.Builder;
using Framework.Validation.Attributes;
using Framework.Validation.Attributes.Available;
using Framework.Validation.Attributes.Available.Range;

namespace Framework.Configuration.TestGenerate.Configurations;

public partial class ConfigurationGenerationEnvironment(DatabaseName databaseName)
    : CodeGenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
{
    public readonly string DTODataContractNamespace = "Configuration";

    public BLLCoreGeneratorConfiguration BLLCore => field ??= new BLLCoreGeneratorConfiguration(this);

    public BLLGeneratorConfiguration BLL => field ??= new BLLGeneratorConfiguration(this);

    public MainServiceGeneratorConfiguration MainService => field ??= new MainServiceGeneratorConfiguration(this);

    public QueryServiceGeneratorConfiguration QueryService => field ??= new QueryServiceGeneratorConfiguration(this);

    public ServerDTOGeneratorConfiguration ServerDTO => field ??= new ServerDTOGeneratorConfiguration(this);

    public MainControllerConfiguration MainController => field ??= new MainControllerConfiguration(this);

    public MappingSettings MappingSettings => field ??= this.GetMappingSettings(databaseName);

    public MappingSettings GetMappingSettings(DatabaseName dbName) => new MappingSettings<PersistentDomainObjectBase>(dbName);

    protected override IEnumerable<ExtendedAttributeSource> GetExtendedAttributeSources()
    {

        yield return new ExtendedAttributeSourceBuilder()
                     .Add<DomainObjectBase>(tb => tb.AddAttribute<AvailableDecimalValidatorAttribute>()
                                                    .AddAttribute<AvailablePeriodValidatorAttribute>()
                                                    .AddAttribute<AvailableDateTimeValidatorAttribute>()
                                                    .AddAttribute<DefaultStringMaxLengthValidatorAttribute>())

                     .Add<PersistentDomainObjectBase>(tb => tb.AddProperty(v => v.Id, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly)))
                                                              .AddProperty(v => v.IsNew, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.Ignore))))


                     .Add<BaseDirectory>(tb => tb.AddProperty(v => v.Name, pb => pb.AddAttribute(new VisualIdentityAttribute())))


                     .Add<DomainObjectEvent>(tb =>
                                                 tb.AddAttribute(new BLLRoleAttribute()))

                     .Add<DomainObjectModification>(tb =>
                                                        tb.AddAttribute(new BLLRoleAttribute()))

                     .Add<SentMessage>(tb =>
                                           tb.AddAttribute(new BLLRoleAttribute()))

                     .Add<Sequence>(tb =>
                                        tb.AddAttribute(new BLLViewRoleAttribute())
                                          .AddAttribute(new BLLSaveRoleAttribute())
                                          .AddAttribute(new BLLRemoveRoleAttribute())
                                          .AddProperty(v => v.Number, pb => pb.AddAttribute(new Int64ValueValidatorAttribute { Min = 0 })))

                     .Add<SystemConstant>(tb =>
                                              tb.AddAttribute(new BLLViewRoleAttribute())
                                                .AddAttribute(new BLLSaveRoleAttribute { AllowCreate = false })
                                                .AddProperty(v => v.Type, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly)))
                                                .AddProperty(
                                                    v => v.Code,
                                                    pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly))
                                                            .AddAttribute(new VisualIdentityAttribute()))
                                                .AddProperty(
                                                    v => v.IsManual,
                                                    pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly))))


                     .Add<TargetSystem>(tb =>
                                            tb.AddAttribute(new BLLViewRoleAttribute())
                                              .AddAttribute(new BLLSaveRoleAttribute { AllowCreate = false })
                                              .AddProperty(
                                                  v => v.DomainTypes,
                                                  pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly))))

                     .Add<DomainType>(tb =>
                                          tb.AddAttribute(new BLLViewRoleAttribute())
                                            .AddProperty(v => v.EventOperations, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly)))
                                            .AddProperty(v => v.Name, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly)))
                                            .AddProperty(v => v.Namespace, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly))))

                     .Add<DomainObjectNotification>(tb =>
                                                        tb.AddAttribute(new BLLRoleAttribute()))
                     .Build();
    }

    public static readonly ConfigurationGenerationEnvironment Default = new(new DatabaseName("", "configuration"));
}
