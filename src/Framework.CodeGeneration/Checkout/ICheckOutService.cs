

namespace Framework.DomainDriven.Generation;

public interface ICheckOutService
{
    void CheckOutFile(string fileName);
}

public abstract class CheckOutService : ICheckOutService
{
    public abstract void CheckOutFile(string fileName);



    public static readonly ICheckOutService Empty = new EmptyCheckOutService();

    public static readonly ICheckOutService Trace = Empty.WithTrace();


    private class EmptyCheckOutService : CheckOutService
    {
        public override void CheckOutFile(string fileName)
        {

        }
    }
}
