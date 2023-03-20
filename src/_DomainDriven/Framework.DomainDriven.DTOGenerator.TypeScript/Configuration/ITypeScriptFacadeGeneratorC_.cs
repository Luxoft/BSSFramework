using System;
using System.Collections.Generic;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Facade;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

/// <summary>
/// IClient facade base generator configuration
/// </summary>
/// <typeparam name="TEnvironmentBase">The type of the environment base.</typeparam>
public interface ITypeScriptFacadeGeneratorConfiguration<out TEnvironmentBase> : ITypeScriptFacadeGeneratorConfiguration, IGeneratorConfiguration<TEnvironmentBase, TypeScriptFacadeFileType>
        where TEnvironmentBase : ITypeScriptGenerationEnvironmentBase
{
}

/// <summary>
/// IClient facade base generator configuration
/// </summary>
public interface ITypeScriptFacadeGeneratorConfiguration : IGeneratorConfiguration
{
    /// <summary>
    /// Использование в фасадах Observable-типов, при отключение будут использованы обычные Plain-типы
    /// </summary>
    bool UseObservable { get; }

    ITypeScriptMethodPolicy GeneratePolicy { get; }

    IEnumerable<Type> GetFacadeTypes();

    IEnumerable<MethodInfo> GetFacadeMethods();

    IEnumerable<string> GetNamespaces();

    IEnumerable<RequireJsModule> GetModules();

    string GetGenericFacadeMethodInvocation(bool isPrimitiveType);

    string GetFacadeAsyncFuncName(bool isPrimitiveType, int parametersCount);

    string GetFacadeFileFactoryName();
}
