using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Transfering;

namespace Framework.DomainDriven;

public class DTOFetchPathFactory : IFetchPathFactory<ViewDTOType>
{
    private readonly int maxRecurseLevel;

    private readonly Type persistentDomainObjectBaseType;


    public DTOFetchPathFactory(Type persistentDomainObjectBase, int maxRecurseLevel = 1)
    {
        if (persistentDomainObjectBase == null) throw new ArgumentNullException(nameof(persistentDomainObjectBase));
        if (maxRecurseLevel < 0) { throw new ArgumentOutOfRangeException(nameof(maxRecurseLevel)); }

        this.persistentDomainObjectBaseType = persistentDomainObjectBase;
        this.maxRecurseLevel = maxRecurseLevel;
    }


    public IEnumerable<PropertyPath> Create(Type startDomainType, ViewDTOType dtoType)
    {
        if (startDomainType == null) throw new ArgumentNullException(nameof(startDomainType));
        if (!Enum.IsDefined(typeof(ViewDTOType), dtoType)) throw new InvalidEnumArgumentException(nameof(dtoType), (int) dtoType, typeof(ViewDTOType));

        return this.GetLoadNode(startDomainType, dtoType)
                   .Pipe(node => this.ExpandNode(node))
                   .Pipe(node => this.GetPaths(node));
    }


    protected virtual PropertyLoadNode ExpandNode(PropertyLoadNode node)
    {
        return node;
    }


    protected virtual bool IsTransferType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return this.persistentDomainObjectBaseType.IsAssignableFrom(type);
    }

    protected IEnumerable<PropertyInfo> GetProperties(Type domainType, ViewDTOType dtoType)
    {
        var serializationProperties = domainType.GetSerializationProperties();

        switch (dtoType)
        {
            case ViewDTOType.VisualDTO:

                return from property in this.GetProperties(domainType, ViewDTOType.SimpleDTO)

                       where property.IsVisualIdentity()

                       select property;

            case ViewDTOType.SimpleDTO:

                return from property in serializationProperties

                       let type = property.PropertyType

                       where !type.IsCollection()
                             && !type.IsArray
                             && !type.IsAbstract
                             && !this.IsTransferType(property.PropertyType)

                       select property;

            case ViewDTOType.FullDTO:

                return from property in serializationProperties

                       where !property.IsDetail() && this.IsTransferType(property.PropertyType)

                       select property;

            case ViewDTOType.RichDTO:

                return from property in serializationProperties

                       let elementType = property.PropertyType.GetCollectionOrArrayElementType()

                       where (property.IsDetail() && this.IsTransferType(property.PropertyType))

                             || (elementType != null && this.IsTransferType(elementType))

                       select property;

            case ViewDTOType.ProjectionDTO:

                return from property in serializationProperties

                       let elementType = property.PropertyType.GetCollectionOrArrayElementType()

                       where this.IsTransferType(property.PropertyType)

                       select property;

            default:
                throw new ArgumentOutOfRangeException(nameof(dtoType));
        }
    }


    protected virtual PropertyLoadNode GetLoadNode(Type domainType, ViewDTOType maxDTOType, int recurseLevel = 0, Func<PropertyInfo, bool> subPropertyFilter = null)
    {
        var subNodesRequest = from property in domainType.GetSerializationProperties()

                              where !property.GetPrivateField().Maybe(field => field.HasAttribute<NotPersistentFieldAttribute>())

                              where subPropertyFilter == null || subPropertyFilter(property)

                              orderby property.Name

                              let subNode = this.GetLoadNode(domainType, property, maxDTOType, recurseLevel)

                              where subNode != null

                              select new { Property = property, SubNode = subNode };

        return new PropertyLoadNode(
                                    domainType,
                                    subNodesRequest.ToDictionary(pair => pair.Property, pair => pair.SubNode),
                                    this.GetProperties(domainType, ViewDTOType.SimpleDTO.Min(maxDTOType)).OrderBy(property => property.Name));
    }

    private PropertyLoadNode GetLoadNode(Type domainType, PropertyInfo property, ViewDTOType maxDTOType, int recurseLevel)
    {
        var isRecurse = domainType == property.GetNestedType();

        if (!isRecurse || recurseLevel != this.maxRecurseLevel)
        {
            var nextRecurseLevel = recurseLevel + (isRecurse ? 1 : 0);

            var nextMaxDTOType = maxDTOType == ViewDTOType.ProjectionDTO ? ViewDTOType.ProjectionDTO : (ViewDTOType)property.GetMainDTOType((MainDTOType)maxDTOType);

            if (maxDTOType >= ViewDTOType.FullDTO && this.IsTransferType(property.PropertyType))
            {
                if (!property.IsDetail() || maxDTOType >= ViewDTOType.RichDTO)
                {
                    return this.GetLoadNode(property.PropertyType, nextMaxDTOType, nextRecurseLevel, null);
                }
            }

            if (maxDTOType >= ViewDTOType.RichDTO && property.PropertyType.IsCollection(this.IsTransferType))
            {
                return this.GetLoadNode(property.GetNestedType(), nextMaxDTOType, nextRecurseLevel, subProperty =>

                                                subProperty.PropertyType.IsCollection()
                                                || subProperty.PropertyType != domainType
                                                || subProperty.IsNotMaster());
            }
        }

        return null;
    }


    protected virtual IEnumerable<PropertyPath> GetPaths(PropertyLoadNode node)
    {
        foreach (var pair in node.Properties.OrderBy(pair => pair.Key.Name))
        {
            yield return new PropertyPath(new[] { pair.Key });

            foreach (var subPath in this.GetPaths(pair.Value))
            {
                yield return pair.Key + subPath;
            }
        }
    }

    protected class PropertyLoadNode
    {
        public PropertyLoadNode(Type domainType, IReadOnlyDictionary<PropertyInfo, PropertyLoadNode> properties, IEnumerable<PropertyInfo> primitiveProperties)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            this.DomainType = domainType;
            this.Properties = properties;
            this.PrimitiveProperties = primitiveProperties.ToReadOnlyCollection();
        }


        public Type DomainType { get; private set; }

        public IReadOnlyDictionary<PropertyInfo, PropertyLoadNode> Properties { get; private set; }

        public ReadOnlyCollection<PropertyInfo> PrimitiveProperties { get; private set; }


        public PropertyLoadNode WhereP(Func<PropertyInfo, bool> propertyFilter, bool recurse)
        {
            if (propertyFilter == null) throw new ArgumentNullException(nameof(propertyFilter));

            return new PropertyLoadNode(

                                        this.DomainType,

                                        this.Properties.Where(pair => propertyFilter(pair.Key)).ToDictionary(pair => pair.Key, pair => recurse ? pair.Value.WhereP(propertyFilter, true) : pair.Value),

                                        this.PrimitiveProperties.Where(propertyFilter));
        }

        public PropertyLoadNode SelectN(Func<PropertyLoadNode, PropertyLoadNode> nodeSelector, bool recurse)
        {
            if (nodeSelector == null) throw new ArgumentNullException(nameof(nodeSelector));

            return new PropertyLoadNode(

                                        this.DomainType,

                                        this.Properties.ChangeValue(node => nodeSelector(node).Pipe(recurse, nextNode => recurse ? nextNode.SelectN(nodeSelector, true) : nextNode)),

                                        this.PrimitiveProperties);
        }


        public static PropertyLoadNode operator +(PropertyLoadNode node1, PropertyLoadNode node2)
        {
            if (node1 == null) throw new ArgumentNullException(nameof(node1));
            if (node2 == null) throw new ArgumentNullException(nameof(node2));

            if (node1.DomainType != node2.DomainType)
            {
                throw new Exception("Diff domainTypes");
            }

            var subNodesRequest = from pair in Enumerable.Concat(node1.Properties, node2.Properties)

                                  group pair.Value by pair.Key into propGroup

                                  let subNode = propGroup.Aggregate((state, subNode) => state + subNode)

                                  select new { Property = propGroup.Key, SubNode = subNode };


            return new PropertyLoadNode(
                                        node1.DomainType,
                                        subNodesRequest.ToDictionary(pair => pair.Property, pair => pair.SubNode),
                                        node1.PrimitiveProperties.Concat(node2.PrimitiveProperties).Distinct());
        }
    }
}
