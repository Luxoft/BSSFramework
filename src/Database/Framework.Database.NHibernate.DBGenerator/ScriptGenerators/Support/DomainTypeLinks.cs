namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

public record struct DomainTypeLinks(Type From, Type To)
{
    public static DomainTypeLinks Create(Type from, Type to) => new(from, to);

    public static DomainTypeLinks Create<TFrom, TTo>() => Create(typeof(TFrom), typeof(TTo));
}
