using System;
using System.Collections.Generic;

using Framework.DomainDriven.Attributes;
using Framework.Persistent;

namespace Framework.Configuration.Domain
{
    [NotAuditedClass]
    public class ControlSettingsParam : AuditPersistentDomainObjectBase, IMaster<ControlSettingsParamValue>, IDetail<ControlSettings>, ITypeObject<ControlSettingParamType>
    {
        private readonly ICollection<ControlSettingsParamValue> controlSettingsParamValues = new List<ControlSettingsParamValue>();

        private readonly ControlSettings controlSettings;

        private ControlSettingParamType type;


        protected ControlSettingsParam()
        {

        }

        public ControlSettingsParam(ControlSettings controlSettings)
        {
            if (controlSettings == null) throw new ArgumentNullException(nameof(controlSettings));


            this.controlSettings = controlSettings;
            this.controlSettings.AddDetail(this);
        }

        public virtual IEnumerable<ControlSettingsParamValue> ControlSettingsParamValues
        {
            get { return this.controlSettingsParamValues; }
        }

        public virtual ControlSettings ControlSettings
        {
            get { return this.controlSettings; }
        }

        public virtual ControlSettingParamType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }


        ICollection<ControlSettingsParamValue> IMaster<ControlSettingsParamValue>.Details
        {
            get { return this.controlSettingsParamValues; }
        }


        ControlSettings IDetail<ControlSettings>.Master
        {
            get { return this.controlSettings; }
        }
    }
}
