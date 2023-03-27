namespace Framework.Notification;

public interface IExceptionStorage
{
    void Save(Exception exception);
}
