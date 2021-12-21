using System;
using System.IO;
using System.Text;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Generation
{
    public class FileInfo
    {
        public FileInfo(string relativePath, string content)
        {
            if (relativePath == null) throw new ArgumentNullException(nameof(relativePath));
            if (content == null) throw new ArgumentNullException(nameof(content));

            this.RelativePath = relativePath;
            this.Content = content;
        }

        public string RelativePath { get; }

        public string Content { get; }

        public string AbsolutePath { get; private set; } = null;

        public State FileState { get; private set; } = State.Unknown;

        public FileInfo Save(string path, ICheckOutService checkOutService = null, Encoding encoding = null)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            Encoding usingEncoding = encoding ?? Encoding.UTF8;

            var absolutePath = Path.Combine(path, this.RelativePath);

            var directoryPath = Path.GetDirectoryName(absolutePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            State state;

            if (File.Exists(absolutePath))
            {
                var prevContent = File.ReadAllText(absolutePath, usingEncoding);

                if (prevContent != this.Content)
                {
                    state = State.Modified;
                    this.InternalSave(absolutePath, checkOutService, usingEncoding);
                }
                else
                {
                    state = State.NotModified;
                }
            }
            else
            {
                state = State.New;
                this.InternalSave(absolutePath, checkOutService, usingEncoding);
            }

            this.AbsolutePath = absolutePath;
            this.FileState = state;

            return this;
        }

        private void InternalSave(string absolutePath, ICheckOutService checkOutService, [NotNull]Encoding encoding)
        {
            if (absolutePath == null) throw new ArgumentNullException(nameof(absolutePath));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            checkOutService.Maybe(s => s.CheckOutFile(absolutePath));

            File.WriteAllText(absolutePath, this.Content, encoding);
        }

        public enum State : byte
        {
            Unknown = 0,
            NotModified,
            New,
            Modified
        }
    }
}
