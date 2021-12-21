using System;

namespace Framework.DomainDriven.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NotAuditedClassAttribute : Attribute
    {
    }
}