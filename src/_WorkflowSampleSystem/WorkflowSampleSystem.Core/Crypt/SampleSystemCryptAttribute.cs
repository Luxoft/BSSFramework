using System;
using Framework.Security.Cryptography;

namespace WorkflowSampleSystem
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WorkflowSampleSystemCryptAttribute : CryptAttribute
    {
        public WorkflowSampleSystemCryptAttribute()
            :this(CryptSystem.WorkflowSampleSystem)
        {
        }

        public WorkflowSampleSystemCryptAttribute(CryptSystem system)
            : base(system)
        {

        }
    }
}