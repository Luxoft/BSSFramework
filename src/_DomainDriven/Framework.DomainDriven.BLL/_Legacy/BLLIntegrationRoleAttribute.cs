using System;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    [Obsolete("Use BLLIntegrationSaveRoleAttribute or BLLIntegrationRemoveRoleAttribute", true)]
    public class BLLIntegrationRoleAttribute : BLLServiceRoleAttribute
    {
        public BLLIntegrationRoleAttribute()
        {
        }

        public BLLIntegrationRoleAttribute(bool fake)
        {
        }

        public bool AllowCreate { get; set; } = true;

        public CountType CountType { get; set; } = CountType.Single;

        public bool AllowRemove { get; set; } = false;
    }
}
