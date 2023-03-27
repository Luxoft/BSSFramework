using Framework.Core;

using JetBrains.Annotations;

using NHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public class NHibConnectionSettings
{
    private readonly Action<Configuration> _initAction;

    public NHibConnectionSettings() => this._initAction = _ => { };

    public NHibConnectionSettings([NotNull] string serverAddress, [NotNull] string database)
    {
        if (serverAddress == null) throw new ArgumentNullException(nameof(serverAddress));
        if (database == null) throw new ArgumentNullException(nameof(database));

        this._initAction = cfg =>
                           {
                               cfg.Configure();
                               cfg.Properties.Set("connection.connection_string", $"{serverAddress};Initial Catalog={database}");
                           };
    }

    public NHibConnectionSettings([NotNull] Action<Configuration> initAction)
    {
        if (initAction == null) throw new ArgumentNullException(nameof(initAction));

        this._initAction = initAction;
    }

    /// <summary> Use old schema with Event Listener for storing audit fields (create\modify date-time and user).
    /// Default value is False, set True for backward compatibility.
    /// </summary>
    /// <remarks>Backward compatibility will be removed in future version of framework
    /// </remarks>
    [Obsolete("For backward compatibility only, will be removed in future version")]
    public bool UseEventListenerInsteadOfInterceptorForAudit { get; set; } = false;

    public void Init([NotNull] Configuration configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        this._initAction(configuration);
    }

    public static readonly NHibConnectionSettings AppSettings = new NHibConnectionSettings(cfg => cfg.Configure());
}
