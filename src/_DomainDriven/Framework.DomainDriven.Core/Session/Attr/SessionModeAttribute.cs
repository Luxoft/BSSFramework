using System;

namespace Framework.DomainDriven.BLL
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DBSessionModeAttribute : Attribute
    {
        public DBSessionModeAttribute(DBSessionMode sessionMode)
        {
            this.SessionMode = sessionMode;
        }

        public DBSessionMode SessionMode { get; }
    }
}
