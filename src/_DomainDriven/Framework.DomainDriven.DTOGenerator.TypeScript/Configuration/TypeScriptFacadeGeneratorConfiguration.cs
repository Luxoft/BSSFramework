using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Facade;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;

/// <summary>
/// TypeScript generator configuration base
/// </summary>
/// <typeparam name="TEnvironment">The type of the environment.</typeparam>
public abstract class TypeScriptFacadeGeneratorConfiguration<TEnvironment> : GeneratorConfiguration<TEnvironment, TypeScriptFacadeFileType>, ITypeScriptFacadeGeneratorConfiguration<TEnvironment>
        where TEnvironment : class, ITypeScriptGenerationEnvironmentBase
{
    protected TypeScriptFacadeGeneratorConfiguration(TEnvironment generationEnvironment)
            : base(generationEnvironment)
    {
        this.GeneratePolicy = LazyInterfaceImplementHelper.CreateProxy(this.CreateGeneratePolicy);
    }

    /// <inheritdoc />
    public virtual bool UseObservable { get; } = true;

    public ITypeScriptMethodPolicy GeneratePolicy { get; }


    protected virtual ITypeScriptMethodPolicy CreateGeneratePolicy()
    {
        return new DefaultTypeScriptMethodPolicy(true);
    }



    /* Facade generation usage */
    public abstract IEnumerable<Type> GetFacadeTypes();

    public virtual string GetGenericFacadeMethodInvocation(bool isPrimitiveType)
    {
        return isPrimitiveType ? "createSimpleService" : "createService";
    }

    public virtual string GetFacadeAsyncFuncName(bool isPrimitiveType, int parametersCount)
    {
        return isPrimitiveType ? $"async.SimpleAsyncFunc{parametersCount + 1}" : $"async.AsyncFunc{parametersCount + 2}";
    }

    public virtual string GetFacadeFileFactoryName()
    {
        return "Environment.current.context.facadeFactory";
    }

    public IEnumerable<MethodInfo> GetFacadeMethods()
    {
        return this.GetFacadeTypes()
                   .SelectMany(facadeType => facadeType.ExtractContractMethods().Where(this.GeneratePolicy.Used))
                   .Distinct()
                   .OrderBy(x => x.Name);
    }

    /* End Facade generation usage */
    public virtual IEnumerable<string> GetNamespaces()
    {
        var result = new List<string>();

        // adding typescript interfaces referecne
        result.AddRange(
                        this.GetModules()
                            .Where(x => !string.IsNullOrEmpty(x.InterfacePath)).Select(x => $"/// <reference path=\"{x.InterfacePath}\"/>"));

        // adding require js modules reference
        result.AddRange(
                        this.GetModules()
                            .Select(x => $"import {x.Name} from '{x.ReferencePath}';"));
        return result;
    }

    protected override IEnumerable<ICodeFileFactoryHeader<TypeScriptFacadeFileType>> GetFileFactoryHeaders()
    {
        yield return new CodeFileFactoryHeader<TypeScriptFacadeFileType>(TypeScriptFacadeFileType.Facade, "", type => type.ToString());
    }

    public override string Namespace => string.Empty;

    protected override string NamespacePostfix => string.Empty;


    public virtual IEnumerable<RequireJsModule> GetModules()
    {
        var data = new List<RequireJsModule>
                   {
                           new RequireJsModule("{ Guid, Convert }", "luxite/system", string.Empty),
                           new RequireJsModule("* as Framework", "luxite/framework/framework", string.Empty),
                           new RequireJsModule("{ Core }", "luxite/framework/framework")
                   };
        return data;
    }
}
