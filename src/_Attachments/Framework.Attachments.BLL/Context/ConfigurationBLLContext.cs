//using System;

//using Framework.Persistent;

//namespace Framework.Attachments.BLL
//{
//    public partial class ConfigurationBLLContext
//    {
//        public bool HasAttachment<TDomainObject>(TDomainObject domainObject)
//            where TDomainObject : IIdentityObject<Guid>
//        {
//            return this.GetTargetSystemService(typeof(TDomainObject), false)
//                       .Maybe(s => this.Logics.AttachmentContainer
//                                              .GetSecureQueryable()
//                                              .Any(ac => ac.ObjectId == domainObject.Id
//                                                      && ac.DomainType.TargetSystem == s.TargetSystem
//                                                      && ac.Attachments.Any()));
//        }
//    }
//}
