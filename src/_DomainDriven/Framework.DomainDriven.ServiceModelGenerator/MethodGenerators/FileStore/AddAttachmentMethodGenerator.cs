using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DTOGenerator;

namespace Framework.DomainDriven.ServiceModelGenerator.MethodGenerators.FileStore
{
    public class AddAttachmentMethodGenerator<TConfiguration> : FileStoreMethodGeneratorBase<TConfiguration>
        where TConfiguration : class, IFileStoreGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public AddAttachmentMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override MethodIdentity Identity { get; } = MethodIdentityType.AddAttachment;


        protected override BLL.DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

        protected override string Name => $"Add{this.DomainType.Name}Attachment";

        protected override bool IsEdit { get; } = true;

        protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.Configuration.FileItemType, Transfering.DTOType.IdentityDTO);

        private CodeParameterDeclarationExpression FileItemParameter => new CodeParameterDeclarationExpression(this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.Configuration.FileItemType, Transfering.DTOType.StrictDTO), "fileItemDTO");


        protected override string GetComment()
        {
            return $"Add {this.DomainType.Name} attachment";
        }

        protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
        {
            var codeTypeReference = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, Transfering.DTOType.IdentityDTO);

            yield return codeTypeReference.ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");

            yield return this.FileItemParameter;
        }

        protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
        {
            var fileItem = this.Configuration.FileItemType.ToTypeReference().ToVariableDeclarationStatement("fileItem");

            yield return fileItem;

            var method = this.FileItemParameter.ToVariableReferenceExpression()
                .ToMethodInvokeExpression(
                    this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName,
                    evaluateDataExpr.GetMappingService(),
                    new CodePrimitiveExpression(true));


            yield return method.ToAssignStatement(fileItem.ToVariableReferenceExpression());


            var domainObjectDecl = this.ToDomainObjectVarDeclById(bllRefExpr);

            yield return domainObjectDecl;

            var objectFileContainerDecl = this.GetDefaultBLLVariableDeclaration(evaluateDataExpr, "objectFileContainerBLL", this.Configuration.ObjectFileContainerType);

            yield return objectFileContainerDecl;

            yield return objectFileContainerDecl.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.AddFileItemMethodName, domainObjectDecl.ToVariableReferenceExpression(), fileItem.ToVariableReferenceExpression()).ToExpressionStatement();

            yield return fileItem.ToVariableReferenceExpression()
                        .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(Transfering.DTOType.IdentityDTO))
                        .ToMethodReturnStatement();
        }
    }
}