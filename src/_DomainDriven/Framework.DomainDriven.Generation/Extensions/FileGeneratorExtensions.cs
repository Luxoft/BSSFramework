using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.Generation;

public static class FileGeneratorExtensions
{
    public static IEnumerable<FileInfo> Generate<TRenderData>(this IFileGenerator<IRenderingFile<TRenderData>, IFileRenderer<TRenderData, string>> generator, string path, ICheckOutService checkOutService = null)
    {
        if (generator == null) throw new ArgumentNullException(nameof(generator));
        if (path == null) throw new ArgumentNullException(nameof(path));

        return generator.GetFileGenerators()
                        .Select(fileFactory => new FileInfo(fileFactory.Filename + "." + generator.Renderer.FileExtension, generator.Renderer.Render(fileFactory)).Save(path, checkOutService))
                        .ToList();
    }

    private static FileInfo GetSingle(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, string filename, bool parallel = true)
    {
        var renderedNamespaces = parallel
                                         ? generator.GetFileGenerators().AsParallel().AsOrdered().Select(file => file.GetRenderData())
                                         : generator.GetFileGenerators().Select(file => file.GetRenderData());

        var codeNamespaces = from ns in renderedNamespaces

                             group ns by ns.Name into nsGroup

                             let imports = nsGroup.SelectMany(ns => ns.Imports.Cast<CodeNamespaceImport>()).Distinct(import => import.Namespace)

                             let types = nsGroup.SelectMany(ns => ns.Types.Cast<CodeTypeDeclaration>())

                             select new CodeNamespace(nsGroup.Key).Self(ns => ns.Imports.AddRange(imports.ToArray()))
                                                                  .Self(ns => ns.Types.AddRange(types.ToArray()));

        var compileUnit = new CodeCompileUnit().Self(unit => unit.Namespaces.AddRange(codeNamespaces.ToArray()));

        return new FileInfo(filename + "." + generator.Renderer.FileExtension, generator.Renderer.Render(compileUnit));
    }

    public static FileInfo GenerateSingle(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, string path, string filename, ICheckOutService checkOutService = null, bool parallel = true)
    {
        if (generator == null) throw new ArgumentNullException(nameof(generator));
        if (path == null) throw new ArgumentNullException(nameof(path));
        if (filename == null) throw new ArgumentNullException(nameof(filename));

        return generator.GetSingle(filename, parallel).Save(path, checkOutService);
    }

    private static IEnumerable<FileInfo> GetGroupedFiles(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, Func<CodeTypeDeclaration, string> getFileNameFunc, bool parallel = true)
    {
        if (generator == null) throw new ArgumentNullException(nameof(generator));
        if (getFileNameFunc == null) throw new ArgumentNullException(nameof(getFileNameFunc));

        var renderedNamespaces = parallel
                                         ? generator.GetFileGenerators().AsParallel().AsOrdered().Select(file => file.GetRenderData())
                                         : generator.GetFileGenerators().Select(file => file.GetRenderData());

        return from ns in renderedNamespaces.ToArray()

               from CodeTypeDeclaration type in ns.Types

               group new { Ns = ns, Type = type } by getFileNameFunc(type) into typeGroup

               let codeNamespaces = from typeInfo in typeGroup

                                    group typeInfo by typeInfo.Ns.Name into nsGroup

                                    let imports = nsGroup.SelectMany(pair => pair.Ns.Imports.Cast<CodeNamespaceImport>()).Distinct(import => import.Namespace)

                                    let types = nsGroup.Select(pair => pair.Type)

                                    select new CodeNamespace(nsGroup.Key).Self(ns => ns.Imports.AddRange(imports.ToArray()))
                                                                         .Self(ns => ns.Types.AddRange(types.ToArray()))

               let compileUnit = new CodeCompileUnit().Self(unit => unit.Namespaces.AddRange(codeNamespaces.ToArray()))

               select new FileInfo(typeGroup.Key + "." + generator.Renderer.FileExtension, generator.Renderer.Render(compileUnit));
    }

    public static IEnumerable<FileInfo> GenerateGroup(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, string path, Func<CodeTypeDeclaration, string> getFileNameFunc, ICheckOutService checkOutService = null, bool parallel = true)
    {
        return generator.GetGroupedFiles(getFileNameFunc, parallel)
                        .Select(f => f.Save(path, checkOutService))
                        .ToList();
    }

    public static IEnumerable<FileInfo> GeneratePair(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, string path, string mainFilename, string interfaceFileName, ICheckOutService checkOutService = null, bool parallel = true)
    {
        return generator.GenerateGroup(path, delc => delc.IsInterface ? interfaceFileName : mainFilename, checkOutService, parallel);
    }
}
