using System;
using System.Linq;

using Framework.Authorization.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;
using Framework.Persistent;

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
            : this(new DatabaseName(typeof(PersistentDomainObjectBase).GetTargetSystemName()))
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
    public IMappingSettings MappingSettings => new MappingSettings<PersistentDomainObjectBase>(this.DAL.GetMappingGenerators().Select(mg => mg.Generate()), this.DatabaseName, true);


    public DatabaseName DatabaseName { get; }


    public IMappingSettings GetMappingSettings(DatabaseName dbName, AuditDatabaseName dbAuditName)
    {
        return new MappingSettings<PersistentDomainObjectBase>(this.DAL.GetMappingGenerators().Select(mg => mg.Generate()), dbName, dbAuditName);
    }

    public IMappingSettings GetMappingSettingsWithoutAudit(DatabaseName dbName)
    {
        return new MappingSettings<PersistentDomainObjectBase>(this.DAL.GetMappingGenerators().Select(mg => mg.Generate()), dbName, false);
    }

    public static readonly ServerGenerationEnvironment Default = new ServerGenerationEnvironment();
}
