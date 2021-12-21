using System;
using System.IO;

namespace Framework.Core
{
    public static class FileInfoExtensions
    {
        public static void WithWriteAction(this FileInfo fileInfo, Action writeAction)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));
            if (writeAction == null) throw new ArgumentNullException(nameof(writeAction));

            var attr = fileInfo.Attributes;

            if (attr.HasFlag(FileAttributes.ReadOnly))
            {
                fileInfo.Attributes = attr ^ FileAttributes.ReadOnly;
            }

            writeAction();

            fileInfo.Attributes = attr;
        }

        public static string GetPureExtension(this FileInfo fileInfo)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));

            return fileInfo.Extension.Skip(".", false);
        }
    }
}