using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.SecuritySystem;

namespace Framework.Configuration.WebApi
{
    public partial class ConfigSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSimpleAttachmentsByContainerReference))]
        public IEnumerable<AttachmentSimpleDTO> GetSimpleAttachmentsByContainerReference(AttachmentContainerReferenceStrictDTO attachmentContainerReference)
        {
            if (attachmentContainerReference == null) throw new ArgumentNullException(nameof(attachmentContainerReference));

            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var reference = attachmentContainerReference.ToDomainObject(evaluateData.MappingService);

                var bll = evaluateData.Context.Logics.AttachmentFactory.Create(reference.DomainType, BLLSecurityMode.View);

                return bll.GetObjectsBy(attachment => attachment.Container.ObjectId == attachmentContainerReference.ObjectId).ToSimpleDTOList(evaluateData.MappingService);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSimpleAttachment))]
        public AttachmentSimpleDTO GetSimpleAttachment(AttachmentIdentityDTO attachmentIdentity)
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var attachment = evaluateData.Context.Logics.Attachment.GetById(attachmentIdentity.Id, true);

                evaluateData.Context.GetPersistentTargetSystemService(attachment.Container.DomainType.TargetSystem)
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
                var attachment = evaluateData.Context.Logics.Attachment.GetById(attachmentIdentity.Id, true);

                evaluateData.Context.GetPersistentTargetSystemService(attachment.Container.DomainType.TargetSystem)
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
                var reference = request.Reference.ToDomainObject(evaluateData.MappingService);

                var container = evaluateData.Context.Logics.AttachmentContainer.GetObjectBy(attachmentContainer => attachmentContainer.ObjectId == reference.ObjectId)
                             ?? new AttachmentContainer { DomainType = reference.DomainType, ObjectId = reference.ObjectId };

                var attachment = evaluateData.Context.Logics.Attachment.GetByIdOrCreate(request.Attachment.Id, () => new Attachment(container))
                                                               .Self(a => request.Attachment.MapToDomainObject(evaluateData.MappingService, a));

                var bll = evaluateData.Context.Logics.AttachmentFactory.Create(container.DomainType, BLLSecurityMode.Edit);

                bll.Save(attachment);

                return attachment.ToIdentityDTO();
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(RemoveAttachment))]
        public void RemoveAttachment(AttachmentIdentityDTO attachmentIdent)
        {
            this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                var attachment = evaluateData.Context.Logics.Attachment.GetById(attachmentIdent.Id, true);

                var bll = evaluateData.Context.Logics.AttachmentFactory.Create(attachment.Container.DomainType, BLLSecurityMode.Edit);

                bll.Remove(attachment);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetRichAttachmentTagsEx))]
        public IEnumerable<AttachmentTagRichDTO> GetRichAttachmentTagsEx(AttachmentIdentityDTO attachmentIdentity)
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                var attachment = evaluateData.Context.Logics.Attachment.GetById(attachmentIdentity.Id, true);

                evaluateData.Context.GetPersistentTargetSystemService(attachment.Container.DomainType.TargetSystem)
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
                var attachment = evaluateData.Context.Logics.Attachment.GetById(request.Attachment.Id, true);

                var mapObj = new AttachmentStrictDTO
                {
                    Id = request.Attachment.Id,
                    Content = attachment.Content,
                    Name = attachment.Name,
                    Tags = request.Tags
                };

                mapObj.MapToDomainObject(evaluateData.MappingService, attachment);

                var bll = evaluateData.Context.Logics.AttachmentFactory.Create(attachment.Container.DomainType, BLLSecurityMode.Edit);

                bll.Save(attachment);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(SaveAttachment))]
        public AttachmentIdentityDTO SaveAttachment(string domainTypeName, Guid domainObjectId, AttachmentStrictDTO attachment)
        {
            var reference = this.EvaluateC(DBSessionMode.Read, context => new AttachmentContainerReferenceStrictDTO
            {
                ObjectId = domainObjectId,
                DomainType = context.Logics.DomainType.GetByName(domainTypeName, true).ToIdentityDTO()
            });

            return this.SaveAttachment(new SaveAttachmentRequest { Reference = reference, Attachment = attachment });
        }
    }
}
