using System.CodeDom;

using Framework.CodeDom.Rendering;
using Framework.Core;
using Framework.Core.Rendering;
using Framework.FileGeneration;
using Framework.FileGeneration.Checkout;

namespace Framework.CodeGeneration.Extensions;

public static class FileGeneratorExtensions
{
    public static IEnumerable<GeneratedFileInfo> Generate<TRenderData>(this IFileGenerator<IRenderingFile<TRenderData>, IFileRenderer<TRenderData, string>> generator, string path, ICheckOutService? checkOutService = null)
    {
        if (generator == null) throw new ArgumentNullException(nameof(generator));
        if (path == null) throw new ArgumentNullException(nameof(path));

        return generator.GetFileGenerators()
                        .Select(fileFactory => new GeneratedFileInfo(fileFactory.Filename + "." + generator.Renderer.FileExtension, generator.Renderer.Render(fileFactory)).WithSave(path, checkOutService))
                        .ToList();
    }

    private static GeneratedFileInfo GetSingle(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, string filename, bool parallel = true)
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

        return new GeneratedFileInfo(filename + "." + generator.Renderer.FileExtension, generator.Renderer.Render(compileUnit));
    }

    public static GeneratedFileInfo GenerateSingle(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, string path, string filename, ICheckOutService? checkOutService = null, bool parallel = true)
    {
        if (generator == null) throw new ArgumentNullException(nameof(generator));
        if (path == null) throw new ArgumentNullException(nameof(path));
        if (filename == null) throw new ArgumentNullException(nameof(filename));

        return generator.GetSingle(filename, parallel).WithSave(path, checkOutService);
    }

    private static IEnumerable<GeneratedFileInfo> GetGroupedFiles(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, Func<CodeTypeDeclaration, string> getFileNameFunc, bool parallel = true)
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

               select new GeneratedFileInfo(typeGroup.Key + "." + generator.Renderer.FileExtension, generator.Renderer.Render(compileUnit));
    }

    public static IEnumerable<GeneratedFileInfo> GenerateGroup(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, string path, Func<CodeTypeDeclaration, string> getFileNameFunc, ICheckOutService? checkOutService = null, bool parallel = true) =>
        generator.GetGroupedFiles(getFileNameFunc, parallel)
                 .Select(f => f.WithSave(path, checkOutService))
                 .ToList();

    public static IEnumerable<GeneratedFileInfo> GeneratePair(this IFileGenerator<ICodeFile, CodeDomRenderer> generator, string path, string mainFilename, string interfaceFileName, ICheckOutService? checkOutService = null, bool parallel = true) => generator.GenerateGroup(path, delc => delc.IsInterface ? interfaceFileName : mainFilename, checkOutService, parallel);
}
