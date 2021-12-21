using System.Collections.Generic;
using System.IO;
using System.Linq;

using WixSharp;

using SystemFile = System.IO.File;
using WixSharpFile = WixSharp.File;

namespace Framework.Installer.Behaviour
{
    /// <summary>
    /// Defines default behaviour for collect files
    /// </summary>
    public static class FileCollectHelper
    {
        internal static string TargetPath
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "dp"); }
        }

        /// <param name="name">Output folder name</param>
        /// <param name="sourceFolder">Path to service project</param>
        public static Dir ServiceCollect(string name, string sourceFolder)
        {
            return new Dir(
                name,
                new Dir(
                    "Bin",
                    new Files(Path.Combine(sourceFolder, "bin", "*.*"), FileFilters.ExcludeRedundantConfigFromBinFolder)),
                new DirFiles(Path.Combine(sourceFolder, "*.svc")),
                new DirFiles(Path.Combine(sourceFolder, "*.asax")),
                new DirFiles(Path.Combine(sourceFolder, "*.config"), FileFilters.ExcludePackagesConfig));
        }

        /// <param name="sourceFolder">Path to client project</param>
        public static Dir SilverlightClientCollect(string sourceFolder)
        {
            return new Dir(
                "Client",
                new Dir(
                    "Bin",
                    new Files(Path.Combine(sourceFolder, "bin", "*.*"), FileFilters.ExcludeRedundantConfigFromBinFolder)),
                new Dir("ClientBin", new Files(Path.Combine(sourceFolder, "ClientBin", "*.*"))),
                new DirFiles(Path.Combine(sourceFolder, "*.aspx")));
        }

        internal static void CopyToCurrentFolder(Dir folder)
        {
            if (Directory.Exists(TargetPath))
            {
                Directory.Delete(TargetPath, true);
            }

            ProcessFolder(folder, TargetPath);
        }

        private static void CopyFiles(IEnumerable<WixSharpFile> files, string currentPath)
        {
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file.Name);
                var path = Path.Combine(currentPath, fileInfo.Name);
                SystemFile.Copy(file.Name, path);
                SystemFile.SetAttributes(path, FileAttributes.Normal);
            }
        }

        private static void CopyFiles(DirFiles dirFile, string currentPath)
        {
            CopyFiles(dirFile.GetFiles(dirFile.Name), currentPath);
        }

        private static void CopyFolder(Dir folder, string currentPath)
        {
            var path = Path.Combine(currentPath, folder.Name);
            Directory.CreateDirectory(path);
            ProcessFolder(folder, path);
        }

        private static void ProcessFolder(Dir currentFolder, string currentPath)
        {
            foreach (var dir in currentFolder.Dirs)
            {
                CopyFolder(dir, currentPath);
            }

            foreach (var item in currentFolder.FileCollections.SelectMany(files => files.GetAllItems(files.Name)))
            {
                var dir = item as Dir;
                if (dir != null)
                {
                    CopyFolder(dir, currentPath);
                }

                var dirFile = item as DirFiles;
                if (dirFile != null)
                {
                    CopyFiles(dirFile, currentPath);
                }
            }

            foreach (var dirFile in currentFolder.DirFileCollections)
            {
                CopyFiles(dirFile, currentPath);
            }

            CopyFiles(currentFolder.Files, currentPath);
        }
    }
}