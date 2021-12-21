namespace Framework.Persistent
{
    public interface ICountryObject<out TCountry>
    {
        TCountry Country { get; }
    }
}