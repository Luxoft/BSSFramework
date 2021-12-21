using System;

namespace Framework.DomainDriven.BLL
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DBSessionModeAttribute : Attribute
    {
        public DBSessionModeAttribute(DBSessionMode sessionMode)
        {
            this.SessionMode = sessionMode;
        }


        public DBSessionMode SessionMode { get; private set; }
    }
}