﻿using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent.Mapping;
using Framework.SecuritySystem;

namespace SampleSystem.Domain;

[BLLRole]
[IgnoreHbmMapping]
public class ManagementUnitFluentMapping :
        CommonUnitBase,
        IUnit<ManagementUnitFluentMapping>,
        IPeriodObject,
        ISecurityContext
{
    private readonly IList<ManagementUnitFluentMapping> children = new List<ManagementUnitFluentMapping>();

    private ManagementUnitFluentMapping parent;

    private Period period;

    private bool isProduction;

    private MuComponent muComponent;

    public virtual Period Period
    {
        get => this.period;
        set => this.period = value;
    }

    public virtual bool IsProduction
    {
        get => this.isProduction;
        set => this.isProduction = value;
    }

    public virtual MuComponent MuComponent
    {
        get => this.muComponent;
        set => this.muComponent = value;
    }

    /// <summary>
    ///  Supposed to be set from dto only.
    /// </summary>
    public virtual ManagementUnitFluentMapping Parent
    {
        get => this.parent;
        protected internal set => this.parent = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client | DTORole.Report)]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
    public virtual IEnumerable<ManagementUnitFluentMapping> Children => this.children;

    ManagementUnitFluentMapping IUnit<ManagementUnitFluentMapping>.CurrentObject => this;
}
