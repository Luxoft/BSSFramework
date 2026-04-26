using System.Collections.ObjectModel;

using Anch.Core;

namespace Framework.Core.AnonymousTypeBuilder;

public class TypeMap(string name, IEnumerable<TypeMapMember> members) : TypeMap<TypeMapMember>(name, members), IEquatable<TypeMap>, ISwitchNameObject<TypeMap>
{
    public TypeMap(string name, IEnumerable<KeyValuePair<string, Type>> members)
            : this(name, members.Select(pair => new TypeMapMember(pair.Key, pair.Value)))
    {

    }

    public new TypeMap SwitchName(string newName) => new(newName, this.Members);

    public bool Equals(TypeMap other) => base.Equals(other);
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




    public string Name { get; }

    public ReadOnlyCollection<TMember> Members { get; }


    public override int GetHashCode() => this.Name.GetHashCode() ^ this.Members.Count;

    public bool Equals(TypeMap<TMember>? other) => other != null && this.Name == other.Name && this.Members.SequenceEqual(other.Members);

    public TypeMap<TMember> SwitchName(string newName) => new(newName, this.Members);

    public override bool Equals(object obj) => this.Equals(obj as TypeMap<TMember>);

    IEnumerable<TMember> ITypeMap<TMember>.Members => this.Members;
}
