namespace Framework.Core.LazyObject;

public class LazyObject<T>(Func<T> valueFactory) : Lazy<T>(valueFactory), ILazyObject<T>;
