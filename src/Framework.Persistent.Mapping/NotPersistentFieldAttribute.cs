using System;

namespace Framework.Persistent.Mapping;

[AttributeUsage (AttributeTargets.Field)]
public class NotPersistentFieldAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Class)]
public class NotPersistentClassAttribute : Attribute
{

}
