using System;

namespace Framework.Persistent.Mapping
{
    /// <summary>
    /// Do not generate HBM files and DB Table
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreMappingAttribute : Attribute
    {
    }
}
