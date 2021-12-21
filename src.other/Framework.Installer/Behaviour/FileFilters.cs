using System.IO;

namespace Framework.Installer.Behaviour
{
    /// <summary>
    /// Defines filters to exclude files from msi
    /// </summary>
    public static class FileFilters
    {
        /// <summary></summary>
        public static bool ExcludePackagesConfig(string filename)
        {
            filename = new FileInfo(filename).Name.ToLower();
            return filename != "packages.config";
        }

        /// <summary></summary>
        public static bool ExcludeRedundantConfigFromBinFolder(string filename)
        {
            filename = new FileInfo(filename).Name.ToLower();
            return filename != "appsettings.config" && filename != "connectionstrings.config" &&
                   !filename.EndsWith(".dll.config");
        }

        /// <summary></summary>
        public static bool ExcludeAllConfigs(string filename)
        {
            return !filename.EndsWith(".config");
        }
    }
}