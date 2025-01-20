using Framework.Authorization.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.Projection.Environment;
using Framework.Transfering;

namespace Framework.Authorization.TestGenerate;

public partial class ServerGenerationEnvironment : GenerationEnvironmentBase
{
    public readonly BLLCoreGeneratorConfiguration BLLCore;

    public readonly BLLGeneratorConfiguration BLL;

    public readonly ServerDTOGeneratorConfiguration ServerDTO;

    public readonly MainServiceGeneratorConfiguration MainService;

    public readonly QueryServiceGeneratorConfiguration QueryService;

    public readonly DALGeneratorConfiguration DAL;

    public ServerGenerationEnvironment()
            : this(new DatabaseName("", "auth"))
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

        this.DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
    }

    /// <summary>
    /// Свойства содержащиеся в MappingSettings
    /// DatabaseName - Берётся из namespace'а сборки, которая сдержит тип PersistentDomainObjectBase (метод GetTargetSystemName);
    /// Types - Список доменных объектов. Это все типы наследованные от PersistentDomainObjectBase той сборки, в которой содеержится PersistentDomainObjectBase.
    /// </summary>
    public MappingSettings MappingSettings => new MappingSettings<PersistentDomainObjectBase>(this.DatabaseName, this.DatabaseName.ToDefaultAudit());


    public DatabaseName DatabaseName { get; }


    public MappingSettings GetMappingSettings(DatabaseName dbName, AuditDatabaseName dbAuditName)
    {
        return new MappingSettings<PersistentDomainObjectBase>(dbName, dbAuditName);
    }

    public MappingSettings GetMappingSettingsWithoutAudit(DatabaseName dbName)
    {
        return new MappingSettings<PersistentDomainObjectBase>(this.DAL.GetMappingGenerators().Select(mg => mg.Generate()), dbName);
    }

    public override IDomainTypeRootExtendedMetadata ExtendedMetadata { get; } =

        new DomainTypeRootExtendedMetadataBuilder()

            .Add<BusinessRole>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute()))

            .Add<Principal>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute())
                      .AddAttribute(new BLLSaveRoleAttribute())
                      .AddAttribute(new BLLRemoveRoleAttribute()))

            .Add<Permission>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute { MaxCollection = MainDTOType.RichDTO })
                      .AddAttribute(new BLLRemoveRoleAttribute()))

            .Add<PermissionRestriction>(
                tb =>
                    tb.AddAttribute(new BLLRoleAttribute()))

            .Add<SecurityContextType>(
                tb =>
                    tb.AddAttribute(new BLLViewRoleAttribute()));

    public static readonly ServerGenerationEnvironment Default = new ServerGenerationEnvironment();
}
