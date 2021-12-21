using System;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public static class FileGeneratorExtensions
    {
        public static IEnumerable<FileInfo> GenerateFacadeServiceProxy(
                this IFileGenerator<ICodeFile, CodeDomRenderer> generator,
                string path,
                string filename,
                ICheckOutService checkOutService = null,
                bool parallel = true)
        {
            var generatedPattern = ".generated";

            var genFilenameInfo = filename.SkipLastMaybe(generatedPattern, StringComparison.CurrentCultureIgnoreCase)
                                          .Select(head => new { Head = head, Tail = generatedPattern })
                                          .GetValueOrDefault(() => new { Head = filename, Tail = "" });

            var buildFilename = FuncHelper.Create((string part) => $"{genFilenameInfo.Head}.{part}{genFilenameInfo.Tail}");

            return generator.GenerateGroup(path, decl =>
            {
                if (decl.IsInterface)
                {
                    return buildFilename ("Contract");
                }
                else if (decl.Name.EndsWith("Client"))
                {
                    return buildFilename("Client");
                }
                else if (decl.Name.EndsWith("ServiceProxy"))
                {
                    return buildFilename("ServiceProxy");
                }
                else
                {
                    return filename;
                }

            }, checkOutService, parallel);
        }
    }
}
