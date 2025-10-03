using System.CodeDom;
using System.Runtime.Serialization;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Projection;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator;

public static class BaseCodeDomHelper
{
    private const string SourceParameterName = "source";

    public static CodeConstructor GenerateDefaultConstructor<TConfiguration>(this IFileFactory<TConfiguration> fileFactory)
            where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return new CodeConstructor
               {
                       Attributes = ((fileFactory as IFileTypeSource<MainDTOFileType>).Maybe(s => s.FileType.IsAbstract) || fileFactory.DomainType.IsAbstractDTO() ? MemberAttributes.Family : MemberAttributes.Public) | MemberAttributes.Override,
               };
    }

    public static CodeMemberMethod GenerateIdConstructor<TConfiguration>(this IFileFactory<TConfiguration> fileFactory)
            where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        var idParameter = new CodeParameterDeclarationExpression(typeof(string), "id");

        return new CodeConstructor
               {
                       Attributes = MemberAttributes.Public,
                       Parameters = { idParameter },
                       Statements =
                       {
                               typeof(Guid).ToTypeReference()
                                           .ToObjectCreateExpression(idParameter.ToVariableReferenceExpression())
                                           .ToAssignStatement(new CodeThisReferenceExpression().ToPropertyReference(fileFactory.Configuration.Environment.IdentityProperty))
                       }
               };
    }

    public static IEnumerable<CodeConstructor> GenerateStrictConstructors<TConfiguration>(this IDTOSource<TConfiguration> source)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        return source.GetActualStrictConstructorFileTypes()
                     .Concat(new[] { default(MainDTOFileType) })
                     .Windowed2((fileType, baseFileType) => new[] { false, true}.Select(withoutMappingParameter => source.GenerateStrictConstructor(fileType, baseFileType, withoutMappingParameter)))
                     .SelectMany();
    }

    public static IEnumerable<MainDTOFileType> GetStrictConstructorFileTypes<TConfiguration>(this IDTOSource<TConfiguration> source)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        yield return FileType.RichDTO;
        yield return FileType.FullDTO;
        yield return FileType.SimpleDTO;

        if (source.IsPersistent())
        {
            yield return FileType.BaseAuditPersistentDTO;
            yield return FileType.BasePersistentDTO;
        }
    }

    public static IEnumerable<MainDTOFileType> GetActualStrictConstructorFileTypes<TConfiguration>(this IDTOSource<TConfiguration> source)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.GetStrictConstructorFileTypes().Where(fileType => source.Configuration.GeneratePolicy.Used(source.DomainType, fileType));
    }

    private static CodeConstructor GenerateStrictConstructor<TConfiguration>(this IDTOSource<TConfiguration> source, MainDTOFileType sourceFileType, MainDTOFileType baseFileType, bool withoutMappingParameter)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (sourceFileType == null) throw new ArgumentNullException(nameof(sourceFileType));

        var interfaceSourceTypeRef = source.Configuration.GetCodeTypeReference(source.DomainType, sourceFileType);

        var sourceTypeParameter = new CodeParameterDeclarationExpression(interfaceSourceTypeRef, SourceParameterName);
        var sourceTypeParameterRefExpr = sourceTypeParameter.ToVariableReferenceExpression();

        if (withoutMappingParameter)
        {
            return new CodeConstructor
                   {
                           Parameters = { sourceTypeParameter },
                           Attributes = sourceFileType.IsAbstract ? MemberAttributes.Private : MemberAttributes.Public,
                           ChainedConstructorArgs = { sourceTypeParameterRefExpr, source.Configuration.GetDefaultClientDTOMappingServiceExpression()  }
                   };
        }
        else
        {
            var mappingServiceParameter = new CodeParameterDeclarationExpression(source.Configuration.GetCodeTypeReference(null, FileType.ClientDTOMappingServiceInterface), "mappingService");
            var mappingServiceParameterRefExpr = mappingServiceParameter.ToVariableReferenceExpression();

            var mapName = $"Map{sourceFileType.ShortName}To{source.FileType.ShortName}For{source.DomainType.Name}";

            var constructor = new CodeConstructor
                              {
                                      Parameters = { sourceTypeParameter, mappingServiceParameter },
                                      Attributes = sourceFileType.IsAbstract ? MemberAttributes.Private : MemberAttributes.Public,
                                      Statements =
                                      {
                                              new CodeThrowArgumentNullExceptionConditionStatement(mappingServiceParameter),
                                              mappingServiceParameterRefExpr.ToMethodInvokeExpression(mapName, new CodeThisReferenceExpression(), sourceTypeParameterRefExpr)
                                      }
                              };

            if (baseFileType != null)
            {
                if (!baseFileType.IsAbstract || source.IsPersistent())
                {
                    var baseInterfaceSourceTypeRef = source.Configuration.GetCodeTypeReference(source.DomainType, baseFileType);

                    constructor.ChainedConstructorArgs.AddRange(new CodeExpression[] { new CodeCastExpression(baseInterfaceSourceTypeRef, sourceTypeParameterRefExpr), mappingServiceParameterRefExpr});
                }
            }

            return constructor;
        }

    }

    /// <summary>
    /// Генерирование конструктора для UpdateDTO на основе разницы двух StrictDTO
    /// </summary>
    /// <returns></returns>
    public static CodeConstructor GenerateUpdateFromDiffStrictConstructor<TFileFactory>(
            this TFileFactory fileFactory,
            bool withoutMappingParameter)
            where TFileFactory : class, IFileFactory<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        var strictTypeRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, DTOType.StrictDTO);


        var currentSourceTypeParameter = new CodeParameterDeclarationExpression(strictTypeRef, "currentSource");
        var currentSourceTypeParameterRefExpr = currentSourceTypeParameter.ToVariableReferenceExpression();

        var baseSourceTypeParameter = new CodeParameterDeclarationExpression(strictTypeRef, "baseSource");
        var baseSourceTypeParameterRefExpr = baseSourceTypeParameter.ToVariableReferenceExpression();

        if (withoutMappingParameter)
        {
            return new CodeConstructor
                   {
                           Parameters = { currentSourceTypeParameter, baseSourceTypeParameter },
                           Attributes = MemberAttributes.Public,
                           ChainedConstructorArgs = { currentSourceTypeParameterRefExpr, baseSourceTypeParameterRefExpr, fileFactory.Configuration.GetDefaultClientDTOMappingServiceExpression() }
                   };
        }
        else
        {
            var mappingServiceParameter = new CodeParameterDeclarationExpression(fileFactory.Configuration.GetCodeTypeReference(null, FileType.ClientDTOMappingServiceInterface), "mappingService");
            var mappingServiceParameterRefExpr = mappingServiceParameter.ToVariableReferenceExpression();

            return new CodeConstructor
                   {
                           Parameters = { currentSourceTypeParameter, baseSourceTypeParameter, mappingServiceParameter },
                           Attributes = MemberAttributes.Public,
                           Statements =
                           {
                                   new CodeThrowArgumentNullExceptionConditionStatement(mappingServiceParameter),
                                   mappingServiceParameterRefExpr.ToMethodInvokeExpression($"Map{fileFactory.DomainType.Name}", new CodeThisReferenceExpression(), currentSourceTypeParameterRefExpr, baseSourceTypeParameterRefExpr)
                           }
                   };
        }
    }


    public static CodeConstructor GenerateUpdateFromStrictConstructor<TFileFactory>(this TFileFactory fileFactory, bool withoutMappingParameter)
            where TFileFactory : IFileFactory<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        var strictTypeRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, DTOType.StrictDTO);

        var currentSourceTypeParameter = new CodeParameterDeclarationExpression(strictTypeRef, "currentSource");
        var currentSourceTypeParameterRefExpr = currentSourceTypeParameter.ToVariableReferenceExpression();

        if (withoutMappingParameter)
        {
            return new CodeConstructor
                   {
                           Parameters = { currentSourceTypeParameter },
                           Attributes = MemberAttributes.Public,
                           ChainedConstructorArgs = { currentSourceTypeParameterRefExpr, fileFactory.Configuration.GetDefaultClientDTOMappingServiceExpression() }
                   };
        }
        else
        {
            var mappingServiceParameter = new CodeParameterDeclarationExpression(fileFactory.Configuration.GetCodeTypeReference(null, FileType.ClientDTOMappingServiceInterface), "mappingService");
            var mappingServiceParameterRefExpr = mappingServiceParameter.ToVariableReferenceExpression();

            return new CodeConstructor
                   {
                           Parameters = { currentSourceTypeParameter, mappingServiceParameter },
                           Attributes = MemberAttributes.Public,
                           Statements =
                           {
                                   new CodeThrowArgumentNullExceptionConditionStatement(mappingServiceParameter),
                                   mappingServiceParameterRefExpr.ToMethodInvokeExpression($"Map{fileFactory.DomainType.Name}", new CodeThisReferenceExpression(), currentSourceTypeParameterRefExpr)
                           }
                   };
        }
    }


    public static CodeTypeReference GetIdentityObjectContainerTypeReference<TConfiguration>(this IFileFactory<TConfiguration> fileFactory)
            where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return typeof(IIdentityObjectContainer<>).ToTypeReference(fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType.GetProjectionSourceTypeOrSelf(), FileType.IdentityDTO));
    }

    public static CodeTypeReference GetIdentityObjectTypeReference<TConfiguration>(this IFileFactory<TConfiguration> fileFactory)
            where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return typeof(IIdentityObject<>).ToTypeReference(fileFactory.Configuration.Environment.GetIdentityType());
    }

    public static CodeMemberProperty GetIdentityObjectContainerImplementation<TConfiguration>(this IFileFactory<TConfiguration> fileFactory, bool internalImplementation = false)
            where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        var identityRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType.GetProjectionSourceTypeOrSelf(), FileType.IdentityDTO);
        var identityImplRef = typeof(IIdentityObjectContainer<>).ToTypeReference(identityRef);

        return new CodeMemberProperty
               {
                       Attributes = MemberAttributes.Public | MemberAttributes.Final,
                       Name = "Identity",
                       Type = identityRef,
                       PrivateImplementationType = internalImplementation ? identityImplRef : null,
                       GetStatements =
                       {
                               identityRef.ToObjectCreateExpression(fileFactory.Configuration.GetIdentityPropertyCodeExpression()).ToMethodReturnStatement()
                       },
                       CustomAttributes = { new CodeAttributeDeclaration(typeof(IgnoreDataMemberAttribute).ToTypeReference()) } 
               };
    }

    public static CodeMemberProperty GetIdentityObjectImplementation<TConfiguration>(this IFileFactory<TConfiguration> fileFactory, bool internalImplementation = false)
            where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        var identityRef = fileFactory.Configuration.Environment.GetIdentityType().ToTypeReference();
        var identityImplRef = typeof(IIdentityObject<>).ToTypeReference(identityRef);

        return new CodeMemberProperty
               {
                       Attributes = MemberAttributes.Public | MemberAttributes.Final,
                       Name = fileFactory.Configuration.Environment.IdentityProperty.Name,
                       Type = identityRef,
                       PrivateImplementationType = internalImplementation ? identityImplRef : null,
                       GetStatements =
                       {
                               new CodeThisReferenceExpression().ToPropertyReference(fileFactory.Configuration.Environment.IdentityProperty).ToMethodReturnStatement()
                       }
               };
    }


    public static CodeTypeMember GenerateConvertMethod<TConfiguration>(this IFileFactory<TConfiguration, MainDTOFileType> fileFactory, FileType fileType)
            where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {

        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        var targetRef = fileFactory.Configuration.GetCodeTypeReference(fileFactory.DomainType, fileType);

        return new CodeMemberMethod
               {
                       Attributes = fileType != FileType.StrictDTO ? MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.New
                                            : fileFactory.FileType.ToMapToDomainObjectMemberAttributes(),

                       Name = "To" + fileType.Name.SkipLast("DTO", true),
                       ReturnType = targetRef,
                       Statements = { targetRef.ToObjectCreateExpression(new CodeThisReferenceExpression()).ToMethodReturnStatement() }
               };
    }
}
