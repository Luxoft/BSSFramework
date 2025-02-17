using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;
using Framework.Projection.Environment;

namespace Framework.Configuration.TestGenerate;

public partial class ServerGenerationEnvironment : GenerationEnvironmentBase
{
    public readonly BLLCoreGeneratorConfiguration BLLCore;

    public readonly BLLGeneratorConfiguration BLL;


    public readonly AuditDTOGeneratorConfiguration AuditDTO;

    public readonly MainServiceGeneratorConfiguration MainService;

    public readonly QueryServiceGeneratorConfiguration QueryService;

    public readonly DALGeneratorConfiguration DAL;

    public readonly AuditServiceGeneratorConfiguration AuditService;

    public readonly ServerDTOGeneratorConfiguration ServerDTO;

    public ServerGenerationEnvironment()
            :this(new DatabaseName("", "configuration"))
    {
    }

    public ServerGenerationEnvironment(DatabaseName databaseName)
    {
        this.BLLCore = new BLLCoreGeneratorConfiguration(this);

        this.BLL = new BLLGeneratorConfiguration(this);

        this.ServerDTO = new ServerDTOGeneratorConfiguration(this);

        this.MainService = new MainServiceGeneratorConfiguration(this);

        this.QueryService = new QueryServiceGeneratorConfiguration(this);

        this.DAL = new DALGeneratorConfiguration(this);

        this.AuditService = new AuditServiceGeneratorConfiguration(this);

        this.AuditDTO = new AuditDTOGeneratorConfiguration(this);

        this.DatabaseName = databaseName;
    }

    public DatabaseName DatabaseName { get; }

    public MappingSettings MappingSettings => this.GetMappingSettings(this.DatabaseName);

    public MappingSettings GetMappingSettings(DatabaseName dbName)
    {
        return new MappingSettings<PersistentDomainObjectBase>(dbName);
    }

    public override IDomainTypeRootExtendedMetadata ExtendedMetadata { get; } =

        new DomainTypeRootExtendedMetadataBuilder()

            .Add<CodeFirstSubscription>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute())
                      .AddAttribute(new BLLSaveRoleAttribute { AllowCreate = false }))

            .Add<DomainObjectEvent>(
                tb =>
                    tb.AddAttribute(new BLLRoleAttribute()))

            .Add<DomainObjectModification>(
                tb =>
                    tb.AddAttribute(new BLLRoleAttribute()))

            .Add<ExceptionMessage>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute())
                      .AddAttribute(new BLLSaveRoleAttribute { CustomImplementation = true }))

            .Add<SentMessage>(
                tb =>
                    tb.AddAttribute(new BLLRoleAttribute()))

            .Add<Sequence>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute())
                      .AddAttribute(new BLLSaveRoleAttribute())
                      .AddAttribute(new BLLRemoveRoleAttribute()))

            .Add<SystemConstant>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute())
                      .AddAttribute(new BLLSaveRoleAttribute { AllowCreate = false }))


            .Add<TargetSystem>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute())
                      .AddAttribute(new BLLSaveRoleAttribute { AllowCreate = false }))

            .Add<DomainType>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute()))

            .Add<DomainObjectNotification>(
                tb =>
                    tb.AddAttribute(new BLLRoleAttribute()))

            .Add<ControlSettings>(
                tb =>
                    tb.AddAttribute(new BLLRoleAttribute()));

    public static readonly ServerGenerationEnvironment Default = new ServerGenerationEnvironment();
}
