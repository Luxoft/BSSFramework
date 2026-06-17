namespace Framework.Database.NHibernate.DBGenerator;

public enum DBGenerateScriptMode
{
    /// <summary>
    /// All changes are applied on db
    /// </summary>
    AppliedOnTargetDatabase,
    /// <summary>
    /// All changes are received from copy schema db (not applied on target db)
    /// </summary>
    [Obsolete("This mode is unstable, use AppliedOnTargetDatabase instead")]
    AppliedOnCopySchemeDatabase,
    /// <summary>
    /// All changes are received from copy schema and data db (not applied on target db)
    /// </summary>
    [Obsolete("This mode is unstable, use AppliedOnTargetDatabase instead")]
    AppliedOnCopySchemeAndDataDatabase,
}
