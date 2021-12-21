using System;
using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    internal static class CodeDomHelper
    {
        private const string DomainObjectParameterNameBase = "domainObject";


        public static CodeConstructor GenerateFromDomainObjectConstructor(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, DTOFileType> fileFactory, IPropertyAssigner propertyAssigner)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));
            if (propertyAssigner == null) throw new ArgumentNullException(nameof(propertyAssigner));

            return fileFactory.GenerateFromDomainObjectConstructor(propertyAssigner, fileFactory.DomainType);
        }

        public static CodeConstructor GenerateFromDomainObjectConstructor(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, DTOFileType> fileFactory, IPropertyAssigner propertyAssigner, Type domainType)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));
            if (propertyAssigner == null) throw new ArgumentNullException(nameof(propertyAssigner));

            var mappingServiceParameter = fileFactory.GetMappingServiceParameter();
            var mappingServiceParameterRefExpr = mappingServiceParameter.ToVariableReferenceExpression();

            var sourceDomainParameter = fileFactory.GetDomainTypeSourceParameter(domainType);
            var sourceDomainParameterRef = sourceDomainParameter.ToVariableReferenceExpression();

            return new CodeConstructor
            {
                Attributes = (domainType.IsAbstractDTO() ? MemberAttributes.Family : MemberAttributes.Public) | MemberAttributes.Final,
                Parameters = { mappingServiceParameter, sourceDomainParameter },
                Statements =
                {
                    mappingServiceParameterRefExpr.ToMethodInvokeExpression("Map" + domainType.Name, sourceDomainParameterRef, new CodeThisReferenceExpression())
                }
            }.Self(decl =>
            {
                if (fileFactory.FileType == FileType.ProjectionDTO || (fileFactory.FileType as MainDTOFileType).Maybe(fileType => fileType.BaseType != null))
                {
                    decl.BaseConstructorArgs.AddRange(new CodeExpression[]
                    {
                        mappingServiceParameterRefExpr,
                        sourceDomainParameter.ToVariableReferenceExpression()
                    });
                }
            });
        }



        public static CodeExpression ToHasAccessMethod(this IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase> configuration, CodeExpression contextRef, Enum securityOperationCode, Type domainType, CodeParameterDeclarationExpression domainObjectParameter)
        {
            return configuration.Environment.BLLCore.GetGetSecurityProviderMethodReferenceExpression(contextRef, domainType)
                                .ToMethodInvokeExpression(securityOperationCode.ToPrimitiveExpression())
                                .ToMethodInvokeExpression("HasAccess", domainObjectParameter.ToVariableReferenceExpression());
        }

        public static CodeParameterDeclarationExpression GetMappingServiceParameter(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>> fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            return new CodeParameterDeclarationExpression(fileFactory.Configuration.DTOMappingServiceInterfaceTypeReference, "mappingService");
        }

        public static CodeParameterDeclarationExpression GetDomainTypeTargetParameter(this IFileFactory fileFactory, Type domainType = null)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            return fileFactory.GetDomainObjectParameter(domainType);
        }

        public static CodeParameterDeclarationExpression GetDomainTypeSourceParameter(this IFileFactory fileFactory, Type domainType = null)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            return fileFactory.GetDomainObjectParameter(domainType);
        }

        public static CodeParameterDeclarationExpression GetDomainObjectParameter(this IFileFactory fileFactory, Type domainType = null)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            return new CodeParameterDeclarationExpression(domainType ?? fileFactory.DomainType, DomainObjectParameterNameBase);
        }

        public static CodeParameterDeclarationExpression GetDomainObjectParameter(this Type domainType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return new CodeParameterDeclarationExpression(domainType, DomainObjectParameterNameBase);
        }


        public static CodeMemberMethod GetToDomainObjectMethod(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>> fileFactory)
        {
            var mappingServiceParameter = fileFactory.GetMappingServiceParameter();
            var mappingServiceParameterRefExpr = mappingServiceParameter.ToVariableReferenceExpression();

            return new CodeMemberMethod
            {
                Name = fileFactory.Configuration.ToDomainObjectMethodName,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Parameters = { mappingServiceParameter },
                ReturnType = fileFactory.DomainType.ToTypeReference(),
                Statements =
                {
                    mappingServiceParameterRefExpr.ToMethodReferenceExpression($"To{fileFactory.DomainType.Name}")
                                                  .ToMethodInvokeExpression(new CodeThisReferenceExpression())
                                                  .ToMethodReturnStatement()
                }
            };
        }

        public static CodeMemberMethod GetToDomainObjectWithAllowCreateMethod(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>> fileFactory)
        {
            var mappingServiceParameter = fileFactory.GetMappingServiceParameter();
            var mappingServiceParameterRefExpr = mappingServiceParameter.ToVariableReferenceExpression();

            var allowCreateParameter = typeof(bool).ToTypeReference().ToParameterDeclarationExpression("allowCreate");
            var allowCreateParameterExpr = allowCreateParameter.ToVariableReferenceExpression();

            return new CodeMemberMethod
            {
                Name = fileFactory.Configuration.ToDomainObjectMethodName,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Parameters = { mappingServiceParameter, allowCreateParameter },
                ReturnType = fileFactory.DomainType.ToTypeReference(),
                Statements =
                {
                    mappingServiceParameterRefExpr.ToMethodReferenceExpression($"To{fileFactory.DomainType.Name}")
                                                  .ToMethodInvokeExpression(new CodeThisReferenceExpression(), allowCreateParameterExpr)
                                                  .ToMethodReturnStatement()
                }
            };
        }



        public static CodeParameterDeclarationExpression GetMappingServiceDomainObjectParameter(this IFileFactory fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            return fileFactory.CurrentReference.ToParameterDeclarationExpression(fileFactory.DomainType.Name.ToStartLowerCase() + fileFactory.FileTypeName);
        }

        public static CodeMemberMethod GetMappingServiceInterfaceToDomainObjectMethod(this IFileFactory fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var parameter = fileFactory.GetMappingServiceDomainObjectParameter();

            return new CodeMemberMethod
            {
                Name = $"To{fileFactory.DomainType.Name}",
                Parameters = { parameter },
                ReturnType = fileFactory.DomainType.ToTypeReference()
            };
        }

        public static CodeMemberMethod GetMappingServiceInterfaceToDomainObjectWithAllowCreateMethod(this IFileFactory fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var parameter = fileFactory.GetMappingServiceDomainObjectParameter();
            var allowCreateParameter = typeof(bool).ToTypeReference().ToParameterDeclarationExpression("allowCreate");

            return new CodeMemberMethod
            {
                Name = $"To{fileFactory.DomainType.Name}",
                Parameters = { parameter, allowCreateParameter },
                ReturnType = fileFactory.DomainType.ToTypeReference()
            };
        }

        public static CodeMemberMethod GetMappingServiceInterfaceToDomainObjectMethod(this IFileFactory fileFactory, Type masterType)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));
            if (masterType == null) throw new ArgumentNullException(nameof(masterType));

            var parameter = fileFactory.GetMappingServiceDomainObjectParameter();

            var masterParameter = masterType.ToTypeReference().ToParameterDeclarationExpression("master");

            return new CodeMemberMethod
            {
                Name = $"To{fileFactory.DomainType.Name}",
                Parameters = { parameter, masterParameter },
                ReturnType = fileFactory.DomainType.ToTypeReference()
            };
        }

        public static CodeMemberMethod GetMappingServiceToDomainObjectMethod<TConfiguration>(this IDTOFileFactory<TConfiguration, DTOFileType> fileFactory, Type masterType)
            where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));
            if (masterType == null) throw new ArgumentNullException(nameof(masterType));

            var parameter = fileFactory.GetMappingServiceDomainObjectParameter();
            var parameterExpr = parameter.ToVariableReferenceExpression();

            var masterParameter = masterType.ToTypeReference().ToParameterDeclarationExpression(masterType.Name.ToStartLowerCase());
            var masterParameterExpr = masterParameter.ToVariableReferenceExpression();

            var mappingServiceParameterRefExpr = new CodeThisReferenceExpression();

            var methodName = $"To{fileFactory.DomainType.Name}";

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = methodName,
                Parameters = { parameter, masterParameter },
                ReturnType = fileFactory.DomainType.ToTypeReference(),
                Statements =
                {
                    mappingServiceParameterRefExpr.ToMethodInvokeExpression(fileFactory.Configuration.ToDomainObjectMethodName, parameterExpr,
                        new CodeLambdaExpression
                        {
                            Statements = { fileFactory.DomainType.ToTypeReference().ToObjectCreateExpression(masterParameterExpr) }
                        }).ToMethodReturnStatement()
                }
            };
        }







        public static CodeMemberMethod GetMappingServiceToDomainObjectMethod<TConfiguration>(this IDTOFileFactory<TConfiguration, DTOFileType> fileFactory)
            where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var fileType = (fileFactory as IDTOSource).FileType;

            if (fileType == FileType.IdentityDTO || fileType == FileType.SimpleDTO || fileType == ServerFileType.SimpleIntegrationDTO)
            {
                if (fileType == ServerFileType.SimpleIntegrationDTO && fileFactory.DomainType.IsIntegrationVersion())
                {
                    return fileFactory.GetMappingServiceIdentityToIntegrationVersionDomainObjectMethod();
                }
                else
                {
                    return fileFactory.GetMappingServiceIdentityToDomainObjectMethod();
                }
            }
            else
            {
                return fileFactory.IsPersistent() ? fileFactory.GetMappingServicePersistentToDomainObjectMethod()
                                                  : fileFactory.GetMappingServiceUnpersistentToDomainObjectMethod();
            }
        }

        private static CodeMemberMethod GetMappingServiceIdentityToDomainObjectMethod(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, DTOFileType> fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var parameter = fileFactory.GetMappingServiceDomainObjectParameter();

            var parameterExpr = parameter.ToVariableReferenceExpression();
            var parameterIdExpr = parameterExpr.ToPropertyReference(fileFactory.Configuration.Environment.IdentityProperty.Name);

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = $"To{fileFactory.DomainType.Name}",
                Parameters = { parameter },
                ReturnType = fileFactory.DomainType.ToTypeReference(),
                Statements =
                {
                    new CodeThisReferenceExpression()
                   .ToMethodReferenceExpression($"{nameof(IDTOMappingService<object, object>.GetById)}", fileFactory.DomainType.ToTypeReference())
                   .ToMethodInvokeExpression(parameterIdExpr)
                   .ToMethodReturnStatement()
                },
            };
        }

        private static CodeMemberMethod GetMappingServiceIdentityToIntegrationVersionDomainObjectMethod(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, DTOFileType> fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var parameter = fileFactory.GetMappingServiceDomainObjectParameter();

            var parameterExpr = parameter.ToVariableReferenceExpression();
            var parameterIdExpr = parameterExpr.ToPropertyReference(fileFactory.Configuration.Environment.IdentityProperty.Name);

            return new CodeMemberMethod
                   {
                       Attributes = MemberAttributes.Public,
                       Name = $"To{fileFactory.DomainType.Name}",
                       Parameters = { parameter },
                       ReturnType = fileFactory.DomainType.ToTypeReference(),
                       Statements =
                       {
                           new CodeThisReferenceExpression()
                               .ToMethodReferenceExpression($"{nameof(IDTOMappingService<object, object>.GetById)}", fileFactory.DomainType.ToTypeReference())
                               .ToMethodInvokeExpression(parameterIdExpr, IdCheckMode.SkipEmpty.ToPrimitiveExpression(), LockRole.Update.ToPrimitiveExpression())
                               .ToMethodReturnStatement()
                       },
                   };
        }


        private static CodeMemberMethod GetMappingServiceUnpersistentToDomainObjectMethod(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, DTOFileType> fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var parameter = fileFactory.GetMappingServiceDomainObjectParameter();
            var parameterExpr = parameter.ToVariableReferenceExpression();

            var mappingServiceParameterRefExpr = new CodeThisReferenceExpression();

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = $"To{fileFactory.DomainType.Name}",
                Parameters = { parameter },
                ReturnType = fileFactory.DomainType.ToTypeReference(),
                Statements =
                {
                    mappingServiceParameterRefExpr.ToMethodReferenceExpression(
                        fileFactory.Configuration.ToDomainObjectMethodName + "Base",
                        fileFactory.CurrentReference,
                        fileFactory.DomainType.ToTypeReference())
                                                  .ToMethodInvokeExpression(parameterExpr)
                                                  .ToMethodReturnStatement()
                }
            };
        }

        private static CodeMemberMethod GetMappingServicePersistentToDomainObjectMethod(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, DTOFileType> fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var parameter = fileFactory.GetMappingServiceDomainObjectParameter();
            var parameterExpr = parameter.ToVariableReferenceExpression();

            var mappingServiceParameterRefExpr = new CodeThisReferenceExpression();

            var methodName = $"To{fileFactory.DomainType.Name}";


            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = methodName,
                Parameters = { parameter },
                ReturnType = fileFactory.DomainType.ToTypeReference(),
                Statements =
                {
                    mappingServiceParameterRefExpr.ToMethodReferenceExpression(fileFactory.Configuration.ToDomainObjectMethodName, fileFactory.CurrentReference, fileFactory.DomainType.ToTypeReference())
                                                  .ToMethodInvokeExpression(parameterExpr)
                                                  .ToMethodReturnStatement()
                }
            };
        }


        public static CodeMemberMethod GetMappingServiceToDomainObjectWithAllowCreateMethod(this IFileFactory<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>, DTOFileType> fileFactory)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var parameter = fileFactory.GetMappingServiceDomainObjectParameter();
            var parameterExpr = parameter.ToVariableReferenceExpression();

            var allowCreateParameter = typeof(bool).ToTypeReference().ToParameterDeclarationExpression("allowCreate");
            var allowCreateParameterExpr = allowCreateParameter.ToVariableReferenceExpression();

            var mappingServiceParameterRefExpr = new CodeThisReferenceExpression();

            var methodName = $"To{fileFactory.DomainType.Name}";


            var conditionStatement = new CodeConditionStatement
            {
                Condition = allowCreateParameterExpr,
                TrueStatements =
                {
                    mappingServiceParameterRefExpr.ToMethodInvokeExpression(fileFactory.Configuration.ToDomainObjectMethodName, parameterExpr, new CodeLambdaExpression { Statements = { fileFactory.DomainType.ToTypeReference().ToObjectCreateExpression() } })
                                                  .ToMethodReturnStatement()
                },
                FalseStatements =
                {
                    mappingServiceParameterRefExpr.ToMethodInvokeExpression(methodName, parameterExpr)
                                                  .ToMethodReturnStatement()
                }
            };


            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public,
                Name = methodName,
                Parameters = { parameter, allowCreateParameter },
                ReturnType = fileFactory.DomainType.ToTypeReference(),
                Statements = { conditionStatement }
            };
        }
    }
}
