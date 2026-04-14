using Framework.Authorization.Domain;
using Framework.Authorization.TestGenerate.Configurations._Base;
using Framework.Authorization.TestGenerate.Configurations.BLL;
using Framework.Authorization.TestGenerate.Configurations.BLLCore;
using Framework.Authorization.TestGenerate.Configurations.DTO;
using Framework.Authorization.TestGenerate.Configurations.Services.Main;
using Framework.Authorization.TestGenerate.Configurations.Services.QueryService;
using Framework.Authorization.TestGenerate.Configurations.Services.WebApi;
using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.BLL.Domain.ServiceRole.Base;
using Framework.Database;
using Framework.Database.NHibernate._MappingSettings;
using Framework.ExtendedMetadata;
using Framework.ExtendedMetadata.Builder;
using Framework.Tracking.Validation;
using Framework.Validation;

namespace Framework.Authorization.TestGenerate.Configurations;

public partial class AuthorizationGenerationEnvironment(DatabaseName databaseName) : GenerationEnvironmentBase
{
    public BLLCoreGeneratorConfiguration BLLCore => field ??= new BLLCoreGeneratorConfiguration(this);

    public BLLGeneratorConfiguration BLL => field ??= new BLLGeneratorConfiguration(this);

    public ServerDTOGeneratorConfiguration ServerDTO => field ??= new ServerDTOGeneratorConfiguration(this);

    public MainServiceGeneratorConfiguration MainService => field ??= new MainServiceGeneratorConfiguration(this);

    public QueryServiceGeneratorConfiguration QueryService => field ??= new QueryServiceGeneratorConfiguration(this);

    public MainControllerConfiguration MainController => field ??= new MainControllerConfiguration(this);

    /// <summary>
    /// Свойства содержащиеся в MappingSettings
    /// DatabaseName - Берётся из namespace'а сборки, которая сдержит тип PersistentDomainObjectBase (метод GetTargetSystemName);
    /// Types - Список доменных объектов. Это все типы наследованные от PersistentDomainObjectBase той сборки, в которой содеержится PersistentDomainObjectBase.
    /// </summary>
    public MappingSettings MappingSettings => field ??= this.GetMappingSettings(databaseName, databaseName.ToDefaultAudit());


    public MappingSettings GetMappingSettings(DatabaseName dbName, AuditDatabaseName dbAuditName) => new MappingSettings<PersistentDomainObjectBase>(dbName, dbAuditName);

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


                     .Add<BusinessRole>(tb =>
                                            tb.AddAttribute(new BLLViewRoleAttribute())
                                              .AddProperty(v => v.Permissions, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.Ignore)))
                                              .AddProperty(v => v.Description, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly)))
                         )

                     .Add<Principal>(tb =>
                                         tb.AddAttribute(new BLLViewRoleAttribute())
                                           .AddAttribute(new BLLSaveRoleAttribute())
                                           .AddAttribute(new BLLRemoveRoleAttribute()))

                     .Add<Permission>(tb =>
                                          tb.AddAttribute(new BLLViewRoleAttribute { MaxCollection = MainDTOType.RichDTO })
                                            .AddAttribute(new BLLRemoveRoleAttribute())
                                            .AddProperty(
                                                v => v.DelegatedTo,
                                                pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.Ignore, DTORole.Event))
                                                        .AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly)))
                                            .AddProperty(v => v.DelegatedFrom, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.Ignore)))
                                            .AddProperty(v => v.Principal, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly)))
                                            .AddProperty(v => v.Role, pb => pb.AddAttribute(new FixedPropertyValidatorAttribute())))

                     .Add<PermissionRestriction>(tb =>
                                                     tb.AddAttribute(new BLLRoleAttribute())
                                                       .AddProperty(v => v.Permission, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly)))
                                                       .AddProperty(
                                                           v => v.SecurityContextType,
                                                           pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.Ignore, DTORole.Integration))
                                                                   .AddAttribute(new FixedPropertyValidatorAttribute()))
                                                       .AddProperty(
                                                           v => v.SecurityContextId,
                                                           pb => pb.AddAttribute(new FixedPropertyValidatorAttribute())))

                     .Add<SecurityContextType>(tb =>
                                                   tb.AddAttribute(new BLLViewRoleAttribute())
                                                     .AddProperty(v => v.Name, pb => pb.AddAttribute(new CustomSerializationAttribute(CustomSerializationMode.ReadOnly))))
                     .Add<DelegateToItemModel>(tb => tb.AddProperty(
                                                   v => v.Permission,
                                                   pb => pb.AddAttribute(new AutoMappingAttribute(false))))
                     .Build();

    }

    public static readonly AuthorizationGenerationEnvironment Default = new(new DatabaseName("", "auth"));
}
