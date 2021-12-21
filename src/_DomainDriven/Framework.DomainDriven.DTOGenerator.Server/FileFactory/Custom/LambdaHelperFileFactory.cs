using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Projection;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class LambdaHelperFileFactory<TConfiguration> : FileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, FileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        public LambdaHelperFileFactory(TConfiguration configuration)
            : base(configuration, null)
        {
        }


        public override FileType FileType { get; } = ServerFileType.LambdaHelper;


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration
            {
                TypeAttributes = TypeAttributes.Public,
                Name = this.Name
            }.MarkAsStatic();
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            var convertMethods = from domainType in this.Configuration.DomainTypes

                                 from preConvertMethod in this.GetConvertToDTOMethods(domainType)

                                 select (CodeTypeMember)preConvertMethod;

            return convertMethods;
        }

        private bool CanLambdaConvert(Type domainType, DTOFileType fileType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            if (fileType == FileType.ProjectionDTO || domainType.IsProjection())
            {
                if (fileType != FileType.ProjectionDTO || !domainType.IsProjection())
                {
                    return false;
                }
            }

            if (fileType == FileType.IdentityDTO && !this.Configuration.IsPersistentObject(domainType))
            {
                return false;
            }

            if (fileType == FileType.VisualDTO && !domainType.HasVisualIdentityProperties())
            {
                return false;
            }

            return this.Configuration.GeneratePolicy.Used(domainType, fileType);
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


        private CodeMemberMethod GetConvertToDTOMethod(Type domainType, FileType fileType)
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

        private CodeMemberMethod GetConvertToDTOListMethod(Type domainType, FileType fileType)
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
                        typeof(Framework.Core.EnumerableExtensions)
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
                        typeof(Framework.Core.EnumerableExtensions)
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
}