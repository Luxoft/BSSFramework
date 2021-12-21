using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Ajax.Utilities;

namespace Framework.Installer.Behaviour
{
    /// <summary>
    /// Defines methods for modify files before it is compiled into MSI
    /// </summary>
    public static class FileEditor
    {
        /// <summary>
        /// Method reduces the size of the file (remove all newlines, spaces and etc.)
        /// </summary>
        /// <param name="files">List of absolute paths to files for modifying</param>
        /// <param name="scriptVersion"></param>
        /// <returns>Warnings</returns>
        public static string JsMinify(IEnumerable<string> files, ScriptVersion scriptVersion)
        {
            var minifier = new Minifier();
            var warnings = new StringBuilder();

            foreach (var file in files)
            {
                var content = minifier.MinifyJavaScript(
                    File.ReadAllText(file),
                    new CodeSettings { ScriptVersion = scriptVersion });

                if (minifier.ErrorList.Count > 0)
                {
                    warnings.Append(BuildJsMinifyWarning(file, minifier.ErrorList));
                }

                File.WriteAllText(file, content);
            }

            return warnings.ToString();
        }

        private static string BuildJsMinifyWarning(string filename, IEnumerable<ContextError> errors)
        {
            var error = new StringBuilder();
            error.AppendFormat("\n\nError occured while minimizing file '{0}'\n\n", filename);

            foreach (var contextError in errors)
            {
                error.AppendFormat(
                    "Line: {0}\nError code: {1}\nMessage: {2}\n\n",
                    contextError.StartLine,
                    contextError.ErrorCode,
                    contextError.Message);
            }

            return error.ToString();
        }
    }
}