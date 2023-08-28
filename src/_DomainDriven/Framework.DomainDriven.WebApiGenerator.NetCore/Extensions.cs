using System.CodeDom;
using System.Collections;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.Projection;

namespace Framework.DomainDriven.WebApiGenerator.NetCore;

public static class Extensions
{
    public static CodeMemberMethod WithAttributeMethods<TAttribute>(
            this CodeMemberMethod source,
            params CodeAttributeArgument[] arguments)
            where TAttribute : Attribute
    {
        source.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(TAttribute).ToTypeReference(), arguments));

        return source;
    }

    public static CodeMemberMethod WithAttributeParameter<TAttribute>(this CodeMemberMethod source)
            where TAttribute : Attribute
    {
        var sourceParameters = source.Parameters.Cast<CodeParameterDeclarationExpression>().ToList();

        source.Parameters.Clear();

        foreach (var parameter in sourceParameters)
        {
            var next = new CodeParameterDeclarationExpression(parameter.Type, parameter.Name);

            next.CustomAttributes.Add(new CodeAttributeDeclaration(typeof(TAttribute).ToTypeReference()));

            source.Parameters.Add(next);
        }

        return source;
    }

    public static IGeneratorConfigurationBase<IGenerationEnvironmentBase> ToWebApiNetCore(
            this IGenerationEnvironmentBase environment,
            IEnumerable<IGeneratorConfigurationBase<IGenerationEnvironmentBase>> source,
            string nameSpace = null) =>
            new WebApiNetCoreCompositeGeneratorConfiguration(environment, source.ToList(), nameSpace);

    public static IFileGenerator<ICodeFile, CodeDomRenderer> DecorateProjectionsToRootControllerNetCore(this IFileGenerator<ICodeFile, CodeDomRenderer> source, string suffix = "") =>
            new CodeFileGeneratorDecorator(source, suffix);

    internal static CodeNamespace CombineMethods(
            this CodeNamespace targetNamespace,
            IList<CodeNamespace> addedNamespaces,
            string suffix)
    {
        var targetType = targetNamespace.Types.Cast<CodeTypeDeclaration>().Single(() => new Exception($"Expected one type in generated namespace. Namespace:{targetNamespace.Name}"));

        var baseTypes = targetType
                        .BaseTypes
                        .Cast<CodeTypeReference>()
                        .Concat(addedNamespaces.SelectMany(z => z.Types.Cast<CodeTypeDeclaration>()).SelectMany(z => z.BaseTypes.Cast<CodeTypeReference>()))
                        .Select(z => z.UserData["DomainType"] as Type)
                        .Distinct()
                        .ToList();

        if (baseTypes.Count > 1)
        {
            throw new InvalidOperationException($"{nameof(targetNamespace)}('{targetNamespace.Name}') and {nameof(addedNamespaces)}('{addedNamespaces.Select(z => z.Name).Join(",")}') must have one base type. "
                                                + $"Funded base types of {nameof(targetNamespace)} and {nameof(addedNamespaces)}' are '{baseTypes.Select(z => z.FullName).Join(",")}'");
        }

        var targetMethods = targetNamespace.Types.Cast<CodeTypeDeclaration>().SelectMany(z => z.Members.OfType<CodeMemberMethod>()).ToArray();

        var sourceMethods = addedNamespaces
                            .SelectMany(z => z.Types.Cast<CodeTypeDeclaration>())
                            .SelectMany(z => z.Members.OfType<CodeMemberMethod>())
                            .Where(z => !(z is CodeConstructor))
                            .Distinct()
                            .ToList();

        var addedSourceMethods = sourceMethods.Except(targetMethods, new CodeMemberMethodComparer()).ToArray();

        var result = new CodeTypeDeclaration()
                     .Self(z => z.Name = $"{targetType.Name}{suffix}Controller")
                     .Self(z => z.IsClass = targetType.IsPartial)
                     .Self(z => z.IsPartial = targetType.IsPartial)
                     .Self(z => z.BaseTypes.AddRange(targetType.BaseTypes))
                     .Self(z => z.Members.AddRange(targetType.Members))
                     .Self(z => z.Members.AddRange(addedSourceMethods))
                     .Self(z => z.CustomAttributes.AddRange(targetType.CustomAttributes))
                     .Self(z => targetType.UserData.Cast<DictionaryEntry>().Foreach(q => z.UserData.Add(q.Key, q.Value)));

        return new CodeNamespace(targetNamespace.Name)
               .Self(z => z.Types.Add(result))
               .Self(z => z.Imports.AddRange(targetNamespace.Imports.Cast<CodeNamespaceImport>().Concat(addedNamespaces.SelectMany(q => q.Imports.Cast<CodeNamespaceImport>())).Distinct(q => q.Namespace).ToArray()));
    }

    private class CodeFileGeneratorDecorator : IFileGenerator<ICodeFile, CodeDomRenderer>
    {
        private readonly IFileGenerator<ICodeFile, CodeDomRenderer> source;

        private readonly string suffix;

        public CodeFileGeneratorDecorator(IFileGenerator<ICodeFile, CodeDomRenderer> source, string suffix)
        {
            this.source = source;
            this.suffix = suffix;
        }

        public CodeDomRenderer Renderer => this.source.Renderer;

        public IEnumerable<ICodeFile> GetFileGenerators()
        {
            var groupedByPersistentDomainType = this.source.GetFileGenerators()
                                                    .Select(z => (generator: z, renderedData: z.GetRenderData()))
                                                    .Select(z => (z.generator, z.renderedData, domainType: z.renderedData.Types[0].UserData["DomainType"] as Type))
                                                    .Select(z => (z.generator, z.renderedData, z.domainType, mainDomainType: z.domainType.GetProjectionSourceTypeOrSelf()))
                                                    .GroupBy(z => z.mainDomainType)
                                                    .ToList();

            var pairs = groupedByPersistentDomainType
                        .Select(z => z.Partial(q => q.domainType == z.Key, (mainGenerators, otherGenerators) => (mainGenerator: mainGenerators.FirstOrDefault(), otherGenerators)))
                        .ToList();

            foreach (var valueTuple in pairs)
            {
                var mainRenderedData = valueTuple.mainGenerator.renderedData;

                if (null == mainRenderedData)
                {
                    throw new ArgumentException($"Main domain object controller must be exists for combine projections. Finded Projection:{valueTuple.otherGenerators.Select(z => z.domainType.Name).Join(',')}");
                }

                var resultNameSpace = mainRenderedData.CombineMethods(valueTuple.otherGenerators.Select(q => q.renderedData).ToList(), this.suffix);

                yield return new CodeFile(valueTuple.mainGenerator.generator.Filename + this.suffix + ".Generated", resultNameSpace);
            }
        }
    }

    private class CodeFile : ICodeFile
    {
        private readonly CodeNamespace renderedData;

        public CodeFile(string fileName, CodeNamespace renderedData)
        {
            this.Filename = fileName;
            this.renderedData = renderedData;
        }

        public string Filename { get; }

        public CodeNamespace GetRenderData() => this.renderedData;
    }

    private class CodeMemberMethodComparer : IEqualityComparer<CodeMemberMethod>
    {
        public bool Equals(CodeMemberMethod x, CodeMemberMethod y)
        {
            if (null == x && null == y)
            {
                return true;
            }

            if (null == x || null == y)
            {
                return false;
            }

            return string.Equals(x.Name, y.Name) && this.GetOrdered(x.Parameters).SequenceEqual(this.GetOrdered(y.Parameters), (arg1, arg2) => arg1.Type.BaseType.Equals(arg2.Type.BaseType) && arg1.Name.Equals(arg2.Name));
        }

        public int GetHashCode(CodeMemberMethod obj) =>
                obj.Name.GetHashCode() ^ obj.Parameters.Count.GetHashCode();

        private IEnumerable<CodeParameterDeclarationExpression> GetOrdered(CodeParameterDeclarationExpressionCollection collection) =>
                collection.Cast<CodeParameterDeclarationExpression>()
                          .OrderBy(z => z.Type.BaseType)
                          .ThenBy(z => z.Name);
    }
}
