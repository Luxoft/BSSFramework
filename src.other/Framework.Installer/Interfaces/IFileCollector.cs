using WixSharp;

namespace Framework.Installer.Interfaces
{
    /// <summary>
    /// Defines method for collect all required files into msi
    /// </summary>
    public interface IFileCollector
    {
        /// <summary></summary>
        Dir Collect();
    }
}