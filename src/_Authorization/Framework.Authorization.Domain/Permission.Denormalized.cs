//using System;
//using System.Collections.Generic;
//using System.Linq;

//using Framework.Core;
//using Framework.DomainDriven.BLL;
//using Framework.DomainDriven.BLL.Security;
//using Framework.DomainDriven.Serialization;
//using Framework.Persistent;
//using Framework.Persistent.Mapping;
//using Framework.Restriction;
//using Framework.SecuritySystem;
//using Framework.Transfering;

//using JetBrains.Annotations;

//namespace Framework.Authorization.Domain
//{
//    public partial class Permission : IMaster<DenormalizedPermissionItem>
//    {
//        private readonly ICollection<DenormalizedPermissionItem> denormalizedItems = new List<DenormalizedPermissionItem>();

//        [UniqueGroup]
//        [CustomSerialization(CustomSerializationMode.Ignore)]
//        public virtual IEnumerable<DenormalizedPermissionItem> DenormalizedItems => this.denormalizedItems;

//        ICollection<DenormalizedPermissionItem> IMaster<DenormalizedPermissionItem>.Details => (ICollection<DenormalizedPermissionItem>)this.DenormalizedItems;

//        IEnumerable<IDenormalizedPermissionItem<Guid>> IPermission<Guid>.DenormalizedItems => this.DenormalizedItems;
//    }
//}
