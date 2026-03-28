namespace Framework.BLL;

public interface IBLLFactoryContainer<out TFactory>
{
    TFactory Default { get; }

    TFactory Implemented { get; }
}
