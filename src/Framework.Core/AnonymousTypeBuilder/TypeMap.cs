using System.Collections.ObjectModel;

using CommonFramework;

namespace Framework.Core;

public class TypeMap : TypeMap<TypeMapMember>, IEquatable<TypeMap>, ISwitchNameObject<TypeMap>
{
    public TypeMap(string name, IEnumerable<KeyValuePair<string, Type>> members)
            : this(name, members.Select(pair => new TypeMapMember(pair.Key, pair.Value)))
    {

    }

    public TypeMap(string name, IEnumerable<TypeMapMember> members)
            :base (name, members)
    {

    }

    public new TypeMap SwitchName(string newName)
    {
        return new TypeMap(newName, this.Members);
    }


    public bool Equals(TypeMap other)
    {
        return base.Equals(other);
    }
}

public class TypeMap<TMember> : ITypeMap<TMember>, IEquatable<TypeMap<TMember>>, ISwitchNameObject<TypeMap<TMember>>
        where TMember : ITypeMapMember
{
    public TypeMap(string name, IEnumerable<TMember> members)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (members == null) throw new ArgumentNullException(nameof(members));

        this.Name = name;
        this.Members = members.ToReadOnlyCollection();
    }




    public string Name { get; private set; }

    public ReadOnlyCollection<TMember> Members { get; private set; }


    public override int GetHashCode()
    {
        return this.Name.GetHashCode() ^ this.Members.Count;
    }

    public bool Equals(TypeMap<TMember> other)
    {
        return other != null && this.Name == other.Name && this.Members.SequenceEqual(other.Members);
    }

    public TypeMap<TMember> SwitchName(string newName)
    {
        return new TypeMap<TMember>(newName, this.Members);
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as TypeMap<TMember>);
    }

    IEnumerable<TMember> ITypeMap<TMember>.Members
    {
        get { return this.Members; }
    }
}
