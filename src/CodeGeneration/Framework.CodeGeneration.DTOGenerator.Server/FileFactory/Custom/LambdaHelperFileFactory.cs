using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.BLL.Domain.Serialization.Extensions;
using Framework.CodeDom.Extend;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.Server.FileType;
using Framework.Core;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Custom;

public class LambdaHelperFileFactory<TConfiguration>(TConfiguration configuration)
    : FileFactory<IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>, BaseFileType>(configuration, null)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public override BaseFileType FileType { get; } = ServerFileType.LambdaHelper;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new CodeTypeDeclaration
        {
            TypeAttributes = TypeAttributes.Public,
            Name = this.Name
        }.MarkAsStatic();

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        var convertMethods = from domainType in this.Configuration.DomainTypes

                             from preConvertMethod in this.GetConvertToDTOMethods(domainType)

                             select (CodeTypeMember)preConvertMethod;

        return convertMethods;
    }

    private bool CanLambdaConvert(Type baseDomainType, DTOFileType fileType)
    {
        if (baseDomainType == null) throw new ArgumentNullException(nameof(baseDomainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        var wrappedDomainType = this.Configuration.Environment.MetadataProxyProvider.Wrap(baseDomainType);

        if (fileType == BaseFileType.ProjectionDTO != wrappedDomainType.IsProjection())
        {
            return false;
        }

        if (fileType == BaseFileType.IdentityDTO && !this.Configuration.IsPersistentObject(baseDomainType))
        {
            return false;
        }

        if (fileType == BaseFileType.VisualDTO && !wrappedDomainType.HasVisualIdentityProperties())
        {
            return false;
        }

        return this.Configuration.GeneratePolicy.Used(baseDomainType, fileType);
    }

    private IEnumerable<CodeMemberMethod> GetConvertToDTOMethods(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        foreach (var fileType in this.Configuration.LambdaConvertTypes.Where(fileType => this.CanLambdaConvert(domainType, fileType)))
        {
            yield return this.GetConvertToDTOMethod(domainType, fileType);
            yield return this.GetConvertToDTOListMethod(domainType, fileType);
        }
    }


    private CodeMemberMethod GetConvertToDTOMethod(Type domainType, BaseFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        var configuration = this.Configuration;

        var sourceDomainParameter = this.GetDomainTypeSourceParameter(domainType);
        var dtoRef = configuration.GetCodeTypeReference(domainType, fileType);

        if (!fileType.NeedMappingServiceForConvert())
        {
            return new CodeMemberMethod
                   {
                           Attributes = MemberAttributes.Public | MemberAttributes.Static,
                           Name = "To" + fileType.Name,
                           ReturnType = dtoRef,
                           Parameters =
                           {
                                   new CodeParameterDeclarationExpression(domainType, sourceDomainParameter.Name)
                           },
                           Statements =
                           {
                                   dtoRef.ToObjectCreateExpression(sourceDomainParameter.ToVariableReferenceExpression())
                                         .ToMethodReturnStatement()
                           }
                   }.MarkAsExtension();
        }
        else
        {
            var mappingServiceParameter = this.GetMappingServiceParameter();

            return new CodeMemberMethod
                   {
                           Attributes = MemberAttributes.Public | MemberAttributes.Static,
                           Name = "To" + fileType.Name,
                           ReturnType = dtoRef,
                           Parameters =
                           {
                                   new CodeParameterDeclarationExpression(domainType, sourceDomainParameter.Name),
                                   mappingServiceParameter
                           },
                           Statements =
                           {
                                   dtoRef.ToObjectCreateExpression(mappingServiceParameter.ToVariableReferenceExpression(), sourceDomainParameter.ToVariableReferenceExpression())
                                         .ToMethodReturnStatement()
                           }
                   }.MarkAsExtension();
        }
    }

    private CodeMemberMethod GetConvertToDTOListMethod(Type domainType, BaseFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));


        var domainObjectsParameter = new CodeParameterDeclarationExpression(domainType, "domainObjects");

        var sourceRef = domainObjectsParameter.ToVariableReferenceExpression();

        var dtoRef = this.Configuration.GetCodeTypeReference(domainType, fileType);

        if (!fileType.NeedMappingServiceForConvert())
        {
            return new CodeMemberMethod
                   {
                           Attributes = MemberAttributes.Public | MemberAttributes.Static,
                           Name = "To" + fileType.Name + "List",
                           ReturnType = typeof(List<>).ToTypeReference(dtoRef),
                           Parameters =
                           {
                                   new CodeParameterDeclarationExpression(typeof(IEnumerable<>).MakeGenericType(domainType), domainObjectsParameter.Name)
                           },
                           Statements =
                           {
                                   typeof(CoreEnumerableExtensions)
                                           .ToTypeReferenceExpression()
                                           .ToMethodInvokeExpression("ToList",

                                                                     sourceRef,
                                                                     new CodeParameterDeclarationExpression { Name = "domainObject" }.Pipe(param =>

                                                                             new CodeLambdaExpression
                                                                             {
                                                                                     Parameters = { param },
                                                                                     Statements =
                                                                                     {
                                                                                             this.Configuration.GetConvertToDTOMethod(domainType, fileType)
                                                                                                     .ToMethodInvokeExpression(param.ToVariableReferenceExpression())

                                                                                     }
                                                                             }))
                                           .ToMethodReturnStatement()
                           }
                   }.MarkAsExtension();
        }
        else
        {
            var mappingServiceParameter = this.GetMappingServiceParameter();

            return new CodeMemberMethod
                   {
                           Attributes = MemberAttributes.Public | MemberAttributes.Static,
                           Name = "To" + fileType.Name + "List",
                           ReturnType = typeof(List<>).ToTypeReference(dtoRef),
                           Parameters =
                           {
                                   new CodeParameterDeclarationExpression(typeof(IEnumerable<>).MakeGenericType(domainType), domainObjectsParameter.Name),
                                   mappingServiceParameter
                           },
                           Statements =
                           {
                                   typeof(CoreEnumerableExtensions)
                                           .ToTypeReferenceExpression()
                                           .ToMethodInvokeExpression("ToList",

                                                                     sourceRef,
                                                                     new CodeParameterDeclarationExpression { Name = "domainObject" }.Pipe(param =>

                                                                             new CodeLambdaExpression
                                                                             {
                                                                                     Parameters = { param },
                                                                                     Statements =
                                                                                     {
                                                                                             this.Configuration.GetConvertToDTOMethod(domainType, fileType)
                                                                                                     .ToMethodInvokeExpression(param.ToVariableReferenceExpression(), mappingServiceParameter.ToVariableReferenceExpression())

                                                                                     }
                                                                             }))
                                           .ToMethodReturnStatement()
                           }
                   }.MarkAsExtension();
        }
    }
}
