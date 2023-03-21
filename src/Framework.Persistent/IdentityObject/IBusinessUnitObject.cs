namespace Framework.Persistent;

public interface IBusinessUnitObject<out TBusinessUnit>
{
    TBusinessUnit BusinessUnit { get; }
}
