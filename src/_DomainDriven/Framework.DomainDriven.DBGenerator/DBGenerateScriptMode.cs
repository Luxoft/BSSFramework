using System;

namespace Framework.DomainDriven.DBGenerator
{
    public enum DBGenerateScriptMode
    {
        /// <summary>
        /// All changes are applyed on db
        /// </summary>
        AppliedOnTargetDatabase,
        /// <summary>
        /// All changes are received from copy schema db (not applyed on target db)
        /// </summary>
        [Obsolete("This mode is unstable, use AppliedOnTargetDatabase instead")]
        AppliedOnCopySchemeDatabase,
        /// <summary>
        /// All changes are received from copy schema and data db (not applyed on target db)
        /// </summary>
        [Obsolete("This mode is unstable, use AppliedOnTargetDatabase instead")]
        AppliedOnCopySchemeAndDataDatabase,
    }
}
