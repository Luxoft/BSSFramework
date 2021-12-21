using System;

namespace Framework.DomainDriven
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreFetchAttribute : Attribute
    {

    }
}