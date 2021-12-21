using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.DomainDriven.WebApiNetCore;

namespace Framework.DomainDriven.WebApiGenerator.NetCore
{
    public class WebApiNetCoreFileFactoryBase<TConfiguration> : FileFactory<TConfiguration>
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public WebApiNetCoreFileFactoryBase(TConfiguration configuration, Type domainType)
                : base(configuration, domainType)
        {
        }

        public sealed override FileType FileType { get; } = FileType.Implement;

        protected sealed override IEnumerable<string> GetImportedNamespaces() => base.GetImportedNamespaces().Concat(new[] { this.Configuration.Environment.ServerDTO.Namespace });

        protected sealed override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration
            {
                Name = this.DomainType.Name,
                Attributes = MemberAttributes.Public,
                IsPartial = true,
                IsClass = true,
            };
        }

        protected sealed override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            var serviceEnvironmentTypeReference = new CodeTypeReference(typeof(IServiceEnvironment<>))
            {
                TypeArguments = { this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference }
            };

            var evaluateDataTypeReference = this.GetEvaluateDataTypeReference();

            var result = new CodeTypeReference(typeof(ApiControllerBase<,,>))
            {
                TypeArguments =
                                 {
                                         serviceEnvironmentTypeReference,
                                         this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference,
                                         evaluateDataTypeReference
                                 }
            };

            yield return result;
        }

        protected sealed override IEnumerable<CodeConstructor> GetConstructors()
        {
            var baseTypeReference = this.GetBaseTypes().First();

            var minParameters = new[]
                                {
                                        new CodeParameterDeclarationExpression(baseTypeReference.TypeArguments[0], "serviceEnvironment"),
                                        new CodeParameterDeclarationExpression(typeof(Framework.Exceptions.IExceptionProcessor), "exceptionProcessor")
                                };

            var parametersCollection = new[]
                                       {
                                           minParameters,
                                           ////minParameters
                                           ////    .Concat(
                                           ////            new[]
                                           ////            {
                                           ////                new CodeParameterDeclarationExpression(
                                           ////                                                       typeof(string),
                                           ////                                                       "principalName")
                                           ////            })
                                           ////    .ToArray()
                                       };

            foreach (var parameters in parametersCollection)
            {
                var result = new CodeConstructor
                {
                    Attributes = MemberAttributes.Public
                };

                result.Parameters.AddRange(parameters);

                result.BaseConstructorArgs.AddRange(parameters.Select(z => z.ToVariableReferenceExpression()).ToArray());

                yield return result;
            }
        }

        protected override IEnumerable<CodeTypeMember> GetMembers() => base.GetMembers().Concat(new[] { this.GetOverrideMethod() });

        /// <summary>
        /// Gets the override methods.
        /// </summary>
        /// <returns>System.CodeDom.CodeTypeMember.</returns>
        private CodeTypeMember GetOverrideMethod()
        {
            var implMappingServiceTypeName = this.Configuration.Environment.TargetSystemName + ServerFileType.ServerPrimitiveDTOMappingService;

            var sessionMethodParameter = new CodeParameterDeclarationExpression(typeof(IDBSession), "session");
            var contextMethodParameter = new CodeParameterDeclarationExpression(this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference, "context");

            var result = new CodeMemberMethod()
            {
                Name = "GetEvaluatedData",
#pragma warning disable S3265 // Remove this bitwise operation; the enum 'MemberAttributes' is not marked with 'Flags' attribute.
                Attributes = MemberAttributes.Override | MemberAttributes.Family,
#pragma warning restore S3265
                Parameters =
                {
                    sessionMethodParameter,
                    contextMethodParameter
                },
                ReturnType = this.GetEvaluateDataTypeReference(),
                Statements =
                {
                    new CodeObjectCreateExpression(
                            this.GetEvaluateDataTypeReference(),
                            sessionMethodParameter.ToVariableReferenceExpression(),
                            contextMethodParameter.ToVariableReferenceExpression(),
                            new CodeObjectCreateExpression(new CodeTypeReference(implMappingServiceTypeName), contextMethodParameter.ToVariableReferenceExpression()))
                        .ToMethodReturnStatement()
                }
            };

            return result;
        }

        private CodeTypeReference GetEvaluateDataTypeReference() => new CodeTypeReference(typeof(EvaluatedData<,>))
                                                                    {
                                                                        TypeArguments =
                                                                        {
                                                                            this.Configuration.Environment.BLLCore
                                                                                .BLLContextInterfaceTypeReference,
                                                                            this.Configuration.Environment.ServerDTO
                                                                                .DTOMappingServiceInterfaceTypeReference
                                                                        }
                                                                    };
    }
}
