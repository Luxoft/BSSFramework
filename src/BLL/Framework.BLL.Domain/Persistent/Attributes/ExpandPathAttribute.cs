namespace Framework.BLL.Domain.Persistent.Attributes;

/// <summary>
/// Expands a chain of virtual properties, turning virtual property calls into real ones. The attribute is needed to "flatten" the selected domain object, i.e. to pull part of the information from one or several nested complex objects into the root object. Required for building OData queries against virtual properties.
/// <see href="confluence/display/IADFRAME/ExpandPathAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ExpandPathAttribute(string path) : PathAttribute(path);
