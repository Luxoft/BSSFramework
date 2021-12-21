using System;
using System.IO;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class FileHelper
    {
        public static string GetTempFileName(string extension)
        {
            var tempFileName = Path.GetTempFileName();

            if (string.IsNullOrEmpty(extension))
            {
                return tempFileName;
            }
            else
            {
                var newTempFileName = Path.ChangeExtension(tempFileName, extension);

                if (File.Exists(newTempFileName))
                {
                    return GetTempFileName(extension);
                }
                else
                {
                    File.Create(newTempFileName).Dispose();

                    return newTempFileName;
                }
            }
        }

        public static TResult TempProcess<TResult>([NotNull] Func<string, TResult> getResult, string pureExtension = null, byte[] content = null)
        {
            if (getResult == null) throw new ArgumentNullException(nameof(getResult));

            var tempFileName = GetTempFileName(pureExtension);

            try
            {
                if (content != null)
                {
                    File.WriteAllBytes(tempFileName, content);
                }

                return getResult(tempFileName);
            }
            finally
            {
                File.Delete(tempFileName);
            }
        }

        public static byte[] TempProcessBinary([NotNull] Action<string> action, string pureExtension = null, byte[] content = null)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return TempProcess(fileName =>
            {
                action(fileName);

                return File.ReadAllBytes(fileName);
            }, pureExtension, content);
        }

        ///<summary>
        ///Remove file
        ///</summary>
        public static void SafeRemove([NotNull] string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
