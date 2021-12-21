using System;
using System.Collections.Generic;
using System.IO;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Report
{
    public abstract class FileTemplateEvaluator : ITemplateEvaluator<FileInfo>, ITemplateEvaluator<byte[]>
    {
        private readonly string _extension;


        protected FileTemplateEvaluator([NotNull] string extension)
        {
            this._extension = extension;
        }

        protected bool WithExt
        {
            get { return !string.IsNullOrEmpty(this._extension); }
        }

        public byte[] Evaluate(byte[] template, object rootObject = null, IReadOnlyDictionary<string, object> variables = null, bool throwEvaluateException = false)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));

            return FileHelper.TempProcess(fileName =>
            {
                var result = this.Evaluate(new FileInfo(fileName), rootObject, variables);

                try
                {
                    return File.ReadAllBytes(result.FullName);
                }
                finally
                {
                    File.Delete(result.FullName);
                }
            }, this._extension, template);
        }

        public FileInfo Evaluate(FileInfo template, object rootObject = null, IReadOnlyDictionary<string, object> variables = null, bool throwEvaluateException = false)
        {
            if (this.WithExt && !template.GetPureExtension().Equals(this._extension, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception($"Unexpected Extension Format: {template.GetPureExtension()} | Expected: {this._extension}");
            }

            return this.EvaluateInternal(template, rootObject, variables, throwEvaluateException);
        }

        protected abstract FileInfo EvaluateInternal(FileInfo template, object rootObject = null, IReadOnlyDictionary<string, object> variables = null, bool throwEvaluateException = false);
    }
}
