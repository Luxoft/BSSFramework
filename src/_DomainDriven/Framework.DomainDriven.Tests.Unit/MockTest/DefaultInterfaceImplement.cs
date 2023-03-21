namespace Framework.DomainDriven.UnitTest.MockTest;

public class DefaultInterfaceImplement : IInterface
{
    private readonly int _value1;
    private readonly IRefValue _refValue;

    public DefaultInterfaceImplement(int value1, IRefValue refValue)
    {
        this._value1 = value1;
        this._refValue = refValue;
    }

    public int Value1 { get { return this._value1; } }
    public IRefValue RefValue { get { return this._refValue; } }
}
