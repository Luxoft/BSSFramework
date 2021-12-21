using System;

namespace Framework.DomainDriven.Serialization
{
    [Flags]
    public enum DTORole
    {
        Client = 1,

        Integration = 2,

        Event = 4,

        Report = 8,

        All = Client + Integration + Event + Report
    }
}