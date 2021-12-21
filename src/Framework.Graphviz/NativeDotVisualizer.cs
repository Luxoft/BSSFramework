using System;
using System.Diagnostics;
using System.IO;

using Framework.Core;
using Framework.Graphviz.Dot;

using JetBrains.Annotations;

namespace Framework.Graphviz
{
    public class NativeDotVisualizer : DotVisualizer<string>
    {
        private readonly string _binaryFileName;


        public NativeDotVisualizer([NotNull] string binaryFileName)
        {
            if (binaryFileName == null) throw new ArgumentNullException(nameof(binaryFileName));

            if (!File.Exists(binaryFileName))
            {
                throw new Exception($"File \"{binaryFileName}\" not found");
            }

            this._binaryFileName = binaryFileName;
        }


        public override byte[] Render([NotNull]string baseDot, GraphvizFormat format)
        {
            if (baseDot == null) throw new ArgumentNullException(nameof(baseDot));

            return FileHelper.TempProcessBinary(fileName =>
            {
                var dot = baseDot.OptmizeDot();

                var startInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    FileName = this._binaryFileName,
                    Arguments = $@" -T{format.ToString().ToLower()} -o {fileName}"
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();

                    process.StandardInput.Write(dot);
                    process.StandardInput.WriteLine();
                    process.StandardInput.Close();
                    process.WaitForExit();

                    var error = process.StandardError.ReadToEnd();

                    if (!error.IsNullOrWhiteSpace())
                    {
                        throw new Exception($"Dot parser : {error}");
                    }
                }
            });
        }


        private static readonly Lazy<NativeDotVisualizer> LazyConfiguration = LazyHelper.Create(() =>
        {
            var path = ConfigurationManagerHelper.GetAppSettings("NativeDotVisualizerBinaryPath", true);

            return new NativeDotVisualizer(path);
        });

        private static readonly Lazy<NativeDotVisualizer> LazyDefault = LazyHelper.Create(() =>
            new NativeDotVisualizer(@"C:\Utils\GraphVIZ\dot.exe"));


        public static NativeDotVisualizer Configuration
        {
            get { return LazyConfiguration.Value; }
        }

        public static NativeDotVisualizer Default
        {
            get { return LazyDefault.Value; }
        }
    }
}
