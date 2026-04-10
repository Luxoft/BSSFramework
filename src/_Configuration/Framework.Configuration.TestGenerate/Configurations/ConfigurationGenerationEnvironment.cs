using Framework.BLL.Domain.ServiceRole;
using Framework.BLL.Domain.ServiceRole.Base;
using Framework.Configuration.Domain;
using Framework.Configuration.TestGenerate.Configurations._Base;
using Framework.Configuration.TestGenerate.Configurations.BLL;
using Framework.Configuration.TestGenerate.Configurations.BLLCore;
using Framework.Configuration.TestGenerate.Configurations.DTO;
using Framework.Configuration.TestGenerate.Configurations.Services.Main;
using Framework.Configuration.TestGenerate.Configurations.Services.QueryService;
using Framework.Configuration.TestGenerate.Configurations.Services.WebApi;
using Framework.Database;
using Framework.Database.NHibernate._MappingSettings;
using Framework.ExtendedMetadata;

namespace Framework.Configuration.TestGenerate.Configurations;

public partial class ConfigurationGenerationEnvironment : GenerationEnvironmentBase
{
    public readonly BLLCoreGeneratorConfiguration BLLCore;

    public readonly BLLGeneratorConfiguration BLL;

    public readonly MainServiceGeneratorConfiguration MainService;

    public readonly QueryServiceGeneratorConfiguration QueryService;

    public readonly ServerDTOGeneratorConfiguration ServerDTO;

    public readonly MainControllerConfiguration MainController;

    public ConfigurationGenerationEnvironment()
        : this(new DatabaseName("", "configuration"))
    {
    }

    public ConfigurationGenerationEnvironment(DatabaseName databaseName)
    {
        this.BLLCore = new BLLCoreGeneratorConfiguration(this);

        this.BLL = new BLLGeneratorConfiguration(this);

        this.ServerDTO = new ServerDTOGeneratorConfiguration(this);

        this.MainService = new MainServiceGeneratorConfiguration(this);

        this.QueryService = new QueryServiceGeneratorConfiguration(this);

        this.MainController = new MainControllerConfiguration(this);

        this.DatabaseName = databaseName;
    }

    public DatabaseName DatabaseName { get; }

    public MappingSettings MappingSettings => this.GetMappingSettings(this.DatabaseName);

    public MappingSettings GetMappingSettings(DatabaseName dbName) => new MappingSettings<PersistentDomainObjectBase>(dbName);

    public override IMetadataProxyProvider MetadataProxyProvider  { get; } =

        new DomainTypeRootExtendedMetadataBuilder()

            .Add<DomainObjectEvent>(tb =>
                                        tb.AddAttribute(new BLLRoleAttribute()))

            .Add<DomainObjectModification>(tb =>
                                               tb.AddAttribute(new BLLRoleAttribute()))

            .Add<SentMessage>(tb =>
                                  tb.AddAttribute(new BLLRoleAttribute()))

            .Add<Sequence>(tb =>
                               tb.AddAttribute(new BLLViewRoleAttribute())
                                 .AddAttribute(new BLLSaveRoleAttribute())
                                 .AddAttribute(new BLLRemoveRoleAttribute()))

            .Add<SystemConstant>(tb =>
                                     tb.AddAttribute(new BLLViewRoleAttribute())
                                       .AddAttribute(new BLLSaveRoleAttribute { AllowCreate = false }))


            .Add<TargetSystem>(tb =>
                                   tb.AddAttribute(new BLLViewRoleAttribute())
                                     .AddAttribute(new BLLSaveRoleAttribute { AllowCreate = false }))

            .Add<DomainType>(tb =>
                                 tb.AddAttribute(new BLLViewRoleAttribute()))

            .Add<DomainObjectNotification>(tb =>
                                               tb.AddAttribute(new BLLRoleAttribute()));

    public static readonly ConfigurationGenerationEnvironment Default = new();
}
