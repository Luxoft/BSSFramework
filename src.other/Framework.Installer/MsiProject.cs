using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Framework.Installer.Behaviour;
using Framework.Installer.Enums;
using Framework.Installer.EventArgs;
using Framework.Installer.Interfaces;

using WixSharp;

namespace Framework.Installer
{
    /// <summary>
    /// Wrapper for WIX Project
    /// </summary>
    public sealed class MsiProject
    {
        private static string installationFolder = @"C:\inetpub\wwwroot";
        private readonly IFileCollector fileCollector;
        private readonly IList<WixSharp.Action> customActions;
        private readonly string msiName;
        private readonly string outputFolder;
        private readonly Guid projectId;
        private readonly string projectVersion;

        /// <param name="projectId">Any guid. Important: This guid don't must change for different version</param>
        /// <param name="msiName">Msi filename</param>
        /// <param name="projectVersion">Msi version</param>
        /// <param name="outputFolder">Msi will be save in this folder</param>
        /// <param name="fileCollector">Class which collect all required files</param>
        /// <param name="customActions">Custom actions collection. If you need to implement custom installation logic.
        /// Create <see cref="WixSharp.Action"/> instance and add it to this collection. This action will be integrated into MSI.
        /// </param>
        public MsiProject(
            Guid projectId,
            string msiName,
            string projectVersion,
            string outputFolder,
            IFileCollector fileCollector,
            IList<WixSharp.Action> customActions = null)
        {
            this.projectId = projectId;
            this.msiName = msiName;
            this.projectVersion = projectVersion;
            this.outputFolder = outputFolder;
            this.fileCollector = fileCollector;
            this.customActions = customActions;
        }

        /// <summary>
        /// Occurs when all files copied into temperary folder.
        /// Use this event if you need to modify any files before it is compiled into MSI
        /// </summary>
        public static event EventHandler<CollectingFilesCompletedEventArgs> CollectingFilesCompleted;

        /// <summary>
        /// Path to installation folder.
        /// By default C:\inetpub\wwwroot
        /// </summary>
        public static string InstallationFolder
        {
            get { return installationFolder; }
            set { installationFolder = value; }
        }

        /// <summary>
        /// Parsing incoming parameters
        /// </summary>
        public static Dictionary<MsiParameterType, string> ParseParameters(string[] parameters)
        {
            var sourcePath = parameters.FirstOrDefault(value => value.StartsWith("-s:"));
            var version = parameters.FirstOrDefault(value => value.StartsWith("-v:"));
            var outputPath = parameters.FirstOrDefault(value => value.StartsWith("-o:"));
            if (sourcePath == null || version == null || outputPath == null)
            {
                return null;
            }

            sourcePath = sourcePath.Replace("-s:", string.Empty).Trim();
            version = version.Replace("-v:", string.Empty).Trim();
            outputPath = outputPath.Replace("-o:", string.Empty).Trim();
            if (sourcePath == string.Empty || version == string.Empty || outputPath == string.Empty)
            {
                return null;
            }

            return new Dictionary<MsiParameterType, string>
                   {
                       { MsiParameterType.PathToSources, sourcePath },
                       { MsiParameterType.Version, version },
                       { MsiParameterType.PathToOutput, outputPath }
                   };
        }

        /// <summary>
        /// Returns documentation
        /// </summary>
        /// <param name="installerName">Name of exe file</param>
        public static string Help(string installerName)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("\n\nUsage: {0} -s:path -v:version -o:path\n\n", installerName);
            stringBuilder.Append("Options:\n");
            stringBuilder.Append("\t-s:\t Absolute path to sources\n");
            stringBuilder.Append("\t-v:\t Version of application\n");
            stringBuilder.Append("\t-o:\t Absolute path to output folder\n");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Build MSI
        /// </summary>
        public void Build()
        {
            var content = this.fileCollector.Collect();
            FileCollectHelper.CopyToCurrentFolder(content);
            this.OnCollectingFilesCompleted();

            var items = new List<WixObject>
                        {
                            new Dir(content.Name, new Files(Path.Combine(FileCollectHelper.TargetPath, "*.*")))
                        };

            if (this.customActions != null)
            {
                items.AddRange(this.customActions);
            }

            var project = new Project(this.msiName, items.ToArray())
                          {
                              GUID = this.projectId,
                              Version = new Version(this.projectVersion),
                              OutDir = this.outputFolder,
                              UI = WUI.WixUI_InstallDir,
                              CustomUI = new DialogCollector().Collect()
                          };

            Compiler.WixSourceGenerated += this.UpdateProjectWxs;
            Compiler.BuildMsi(project);

            Compiler.WixSourceGenerated -= this.UpdateProjectWxs;
            Directory.Delete(FileCollectHelper.TargetPath, true);
        }

        private void OnCollectingFilesCompleted()
        {
            var handler = CollectingFilesCompleted;
            if (handler == null)
            {
                return;
            }

            var args = new CollectingFilesCompletedEventArgs
                       {
                           PathToTempFolder = FileCollectHelper.TargetPath,
                           ApplicationVersion = this.projectVersion
                       };

            handler(this, args);
        }

        private void UpdateProjectWxs(XDocument document)
        {
            document.Root
                .Select("Product")
                .AddElement("MajorUpgrade")
                .AddAttributes(new Dictionary<string, string>
                               {
                                   { "AllowSameVersionUpgrades", "yes" },
                                   { "Schedule", "afterInstallInitialize" },
                                   {
                                       "DowngradeErrorMessage",
                                       "A later version is already installed. Setup will now exit."
                                   }
                               });
            document.Root
                .Select("Product/Package")
                .AddAttributes(new Dictionary<string, string>
                               {
                                   { "InstallScope", "perMachine" }
                               });

            foreach (var element in document.Descendants("File"))
            {
                element.AddAttributes(new Dictionary<string, string> { { "KeyPath", "yes" } });
            }
        }
    }
}