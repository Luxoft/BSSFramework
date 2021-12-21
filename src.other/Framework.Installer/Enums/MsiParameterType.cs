namespace Framework.Installer.Enums
{
    /// <summary>
    /// Parameters required for msi build
    /// </summary>
    public enum MsiParameterType
    {
        /// <summary>
        /// Absolute path to sources
        /// </summary>
        PathToSources,

        /// <summary>
        /// Version of application
        /// </summary>
        Version,

        /// <summary>
        /// Absolute path to output folder
        /// </summary>
        PathToOutput
    }
}