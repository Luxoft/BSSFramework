using System;
using System.Collections.Generic;

using Framework.Attachments.BLL;
using Framework.Attachments.Domain;
using Framework.DomainDriven.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.SecuritySystem;

namespace Framework.Configuration.WebApi
{
    public class AttachmentController : ApiControllerBase<IServiceEnvironment<IConfigurationBLLContext>, IConfigurationBLLContext, EvaluatedData<IConfigurationBLLContext, IAttachmentsDTOMappingService>>
    {
        private readonly IAttachmentServiceEnvironmentModule attachmentServiceEnvironmentModule;

        public AttachmentController(IServiceEnvironment<IConfigurationBLLContext> environment, IExceptionProcessor exceptionProcessor, IAttachmentServiceEnvironmentModule attachmentServiceEnvironmentModule) : base(environment, exceptionProcessor)
        {
            this.attachmentServiceEnvironmentModule = attachmentServiceEnvironmentModule;
        }


        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSimpleAttachmentsByContainerReference))]
        public IEnumerable<AttachmentSimpleDTO> GetSimpleAttachmentsByContainerReference(AttachmentContainerReferenceStrictDTO attachmentContainerReference)
        {
            if (attachmentContainerReference == null) throw new ArgumentNullException(nameof(attachmentContainerReference));

            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var reference = attachmentContainerReference.ToDomainObject(evaluateData.MappingService);

                var contextModule = this.attachmentServiceEnvironmentModule.CreateContextModule(evaluateData.Context);

                var attachmentBLL = new AttachmentBLLFactory(contextModule).Create(reference.DomainType, BLLSecurityMode.View);

                return attachmentBLL.GetObjectsBy(attachment => attachment.Container.ObjectId == attachmentContainerReference.ObjectId).ToSimpleDTOList(evaluateData.MappingService);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSimpleAttachment))]
        public AttachmentSimpleDTO GetSimpleAttachment(AttachmentIdentityDTO attachmentIdentity)
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var contextModule = this.attachmentServiceEnvironmentModule.CreateContextModule(evaluateData.Context);

                var defaultAttachmentBLL = new AttachmentBLL(contextModule);

                var attachment = defaultAttachmentBLL.GetById(attachmentIdentity.Id, true);

                contextModule.GetPersistentTargetSystemService(attachment.Container.DomainType.TargetSystem)
                             .GetAttachmentSecurityProvider(attachment.Container.DomainType, BLLSecurityMode.View)
                             .CheckAccess(attachment);

                return attachment.ToRichDTO(evaluateData.MappingService);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetRichAttachment))]
        public AttachmentRichDTO GetRichAttachment(AttachmentIdentityDTO attachmentIdentity)
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var contextModule = this.attachmentServiceEnvironmentModule.CreateContextModule(evaluateData.Context);

                var defaultAttachmentBLL = new AttachmentBLL(contextModule);

                var attachment = defaultAttachmentBLL.GetById(attachmentIdentity.Id, true);

                contextModule.GetPersistentTargetSystemService(attachment.Container.DomainType.TargetSystem)
                             .GetAttachmentSecurityProvider(attachment.Container.DomainType, BLLSecurityMode.View)
                             .CheckAccess(attachment);

                return attachment.ToRichDTO(evaluateData.MappingService);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(SaveAttachment))]
        public AttachmentIdentityDTO SaveAttachment(SaveAttachmentRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                var contextModule = this.attachmentServiceEnvironmentModule.CreateContextModule(evaluateData.Context);

                var reference = request.Reference.ToDomainObject(evaluateData.MappingService);

                var attachmentContainerBLL = new AttachmentContainerBLL(contextModule);

                var defaultAttachmentBLL = new AttachmentBLL(contextModule);

                var container = attachmentContainerBLL.GetObjectBy(attachmentContainer => attachmentContainer.ObjectId == reference.ObjectId)
                                ?? new AttachmentContainer { DomainType = reference.DomainType, ObjectId = reference.ObjectId };

                var attachment = defaultAttachmentBLL.GetByIdOrCreate(request.Attachment.Id, () => new Attachment(container))
                                                     .Self(a => request.Attachment.MapToDomainObject(evaluateData.MappingService, a));

                var attachmentBLL = new AttachmentBLLFactory(contextModule).Create(reference.DomainType, BLLSecurityMode.Edit);

                attachmentBLL.Save(attachment);

                return attachment.ToIdentityDTO();
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(RemoveAttachment))]
        public void RemoveAttachment(AttachmentIdentityDTO attachmentIdent)
        {
            this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                var contextModule = this.attachmentServiceEnvironmentModule.CreateContextModule(evaluateData.Context);

                var attachment = evaluateData.MappingService.ToAttachment(attachmentIdent);

                var attachmentBLL = new AttachmentBLLFactory(contextModule).Create(attachment.Container.DomainType, BLLSecurityMode.Edit);

                attachmentBLL.Remove(attachment);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetRichAttachmentTagsEx))]
        public IEnumerable<AttachmentTagRichDTO> GetRichAttachmentTagsEx(AttachmentIdentityDTO attachmentIdentity)
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var contextModule = this.attachmentServiceEnvironmentModule.CreateContextModule(evaluateData.Context);

                var attachment = evaluateData.MappingService.ToAttachment(attachmentIdentity);

                contextModule.GetPersistentTargetSystemService(attachment.Container.DomainType.TargetSystem)
                             .GetAttachmentSecurityProvider(attachment.Container.DomainType, BLLSecurityMode.View)
                             .CheckAccess(attachment);

                return attachment.Tags.ToRichDTOList(evaluateData.MappingService);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(SetAttachmentTags))]
        public void SetAttachmentTags(SetAttachmentTagsRequest request)
        {
            this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                var contextModule = this.attachmentServiceEnvironmentModule.CreateContextModule(evaluateData.Context);

                var attachment = evaluateData.MappingService.ToAttachment(request.Attachment);

                var mapObj = new AttachmentStrictDTO
                {
                    Id = request.Attachment.Id,
                    Content = attachment.Content,
                    Name = attachment.Name,
                    Tags = request.Tags
                };

                mapObj.MapToDomainObject(evaluateData.MappingService, attachment);

                var attachmentBLL = new AttachmentBLLFactory(contextModule).Create(attachment.Container.DomainType, BLLSecurityMode.Edit);

                attachmentBLL.Save(attachment);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(SaveAttachment))]
        public AttachmentIdentityDTO SaveAttachment(string domainTypeName, Guid domainObjectId, AttachmentStrictDTO attachment)
        {
            var reference = this.EvaluateC(DBSessionMode.Read, context => new AttachmentContainerReferenceStrictDTO
            {
                ObjectId = domainObjectId,
                DomainType = new() { Id = context.Logics.DomainType.GetByName(domainTypeName, true).Id }
            });

            return this.SaveAttachment(new SaveAttachmentRequest { Reference = reference, Attachment = attachment });
        }

        protected override EvaluatedData<IConfigurationBLLContext, IAttachmentsDTOMappingService> GetEvaluatedData(IDBSession session, IConfigurationBLLContext context) =>
            new EvaluatedData<IConfigurationBLLContext, IAttachmentsDTOMappingService>(session, context, new AttachmentsServerPrimitiveDTOMappingService(context));
    }
}
