using CommonFramework;

namespace Framework.Validation;

public abstract class ClassValidationMap : IClassValidationMap, IValidatorCollection<IClassValidator>
{
    protected ClassValidationMap(string typeName)
    {
        if (typeName == null) throw new ArgumentNullException(nameof(typeName));

        this.TypeName = typeName;
    }

    public abstract Type Type { get; }

    public string TypeName { get; }

    protected abstract IReadOnlyCollection<IPropertyValidationMap> BasePropertyMaps { get; }

    protected abstract IReadOnlyCollection<IClassValidator> BaseValidators { get; }

    #region IClassValidationMap Members

    IReadOnlyCollection<IPropertyValidationMap> IClassValidationMap.PropertyMaps => this.BasePropertyMaps;

    IReadOnlyCollection<IClassValidator> IValidatorCollection<IClassValidator>.Validators => this.BaseValidators;

    #endregion
}

public class ClassValidationMap<TSource> : ClassValidationMap, IClassValidationMap<TSource>
{
    public ClassValidationMap(Func<IClassValidationMap<TSource>, IEnumerable<IPropertyValidationMap<TSource>>> getPropertyMaps)
            : this(getPropertyMaps, new IClassValidator<TSource>[0])
    {
    }

    public ClassValidationMap(Func<IClassValidationMap<TSource>, IEnumerable<IPropertyValidationMap<TSource>>> getPropertyMaps, IEnumerable<IClassValidator<TSource>> validators)
            : base(typeof(TSource).GetValidationName())
    {
        if (validators == null) throw new ArgumentNullException(nameof(validators));

        this.PropertyMaps = getPropertyMaps(this).ToReadOnlyCollection();
        this.Validators = validators.ToReadOnlyCollection();
    }

    public ClassValidationMap(IEnumerable<IPropertyValidationMap<TSource>> propertyMaps, IEnumerable<IClassValidator<TSource>> validators)
            : base(typeof(TSource).GetValidationName())
    {
        if (validators == null) throw new ArgumentNullException(nameof(validators));

        this.PropertyMaps = propertyMaps.ToReadOnlyCollection();
        this.Validators = validators.ToReadOnlyCollection();
    }

    public override Type Type => typeof(TSource);

    public IReadOnlyCollection<IPropertyValidationMap<TSource>> PropertyMaps { get; }

    public IReadOnlyCollection<IClassValidator<TSource>> Validators { get; }

    protected override IReadOnlyCollection<IPropertyValidationMap> BasePropertyMaps => this.PropertyMaps;

    protected override IReadOnlyCollection<IClassValidator> BaseValidators => this.Validators;

    public static readonly ClassValidationMap<TSource> Empty = new ClassValidationMap<TSource>(new IPropertyValidationMap<TSource>[0], new IClassValidator<TSource>[0]);
}
